using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField]
    private Transform bulletSpawnPoint;
    [SerializeField]
    private GameObject bulletPrefab;
    [SerializeField]
    private float fireRate;
    [SerializeField]
    private int currentBullets;
    [SerializeField]
    private int maxMagazine;
    [SerializeField]
    private int totalBullets;
    [SerializeField]
    private float bulletSpeed;
    [SerializeField]
    private bool automatic;
    //MuzzleFlash
    //Damage


    public void Shoot()
    {
        if(currentBullets>0)
        {
            // Ray ray = Camera.main.ScreenPointToRay(new Vector2(Screen.width / 2, Screen.height / 2)); Desde un punto en pixeles de la pantalla
            Ray ray = Camera.main.ViewportPointToRay(new Vector2(0.5f, 0.5f)); // desde un porcentaje, donde 00 es la esqina inferior izquierda 
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
             Vector3 bulletDirection = (hit.point - bulletSpawnPoint.position).normalized;
             GameObject bulletClone = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
             bulletClone.GetComponent<Rigidbody>().linearVelocity=bulletDirection*bulletSpeed;
            }
            else
            {
                GameObject bulletClone = Instantiate(bulletPrefab, bulletSpawnPoint.position, bulletSpawnPoint.rotation);
                bulletClone.GetComponent<Rigidbody>().linearVelocity = bulletSpawnPoint.forward * bulletSpeed;
            }
            currentBullets--;
        }
    }
    
    public void Reload()
    {
        int bulletsToReload = maxMagazine - currentBullets;
        if (bulletsToReload < totalBullets)
        {
            currentBullets = maxMagazine;
            totalBullets -= bulletsToReload;
        }
        else
        {
            currentBullets += totalBullets;
            totalBullets = 0;
        }
    }
}

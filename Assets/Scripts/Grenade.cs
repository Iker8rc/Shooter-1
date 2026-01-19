using UnityEngine;

public class Grenade : MonoBehaviour
{
    [SerializeField]
    private float radio;
    [SerializeField]
    private float damage;
    [SerializeField]
    private GameObject explosionPrefab;
    [SerializeField]
    private float countDown;
    private float timePass;
    [SerializeField]
    private float knockBackForce;
    public bool countDownActive;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(countDownActive == true)
        {
            timePass += Time.deltaTime;
            if (timePass > countDown)
            {
                Boom();
            }
        }
    }
    void Boom()
    {
        //SFX
        GameObject vfx = Instantiate(explosionPrefab, transform.position, transform.rotation);
        Collider[] colliders = Physics.OverlapSphere(transform.position, radio);
        for(int i = 0; i < colliders.Length; i++)
        {
            Rigidbody rb = colliders[i].GetComponent<Rigidbody>();
            if(rb != null)
            {
                rb.AddExplosionForce(knockBackForce, transform.position, radio);
                if (colliders[i].gameObject.tag == "Enemy")
                {
                    colliders[i].GetComponent<EnemyController>().TakeDamage(damage);
                }
                else if (colliders[i].gameObject.tag == "Player")
                {
                    colliders[i].GetComponent<PlayerController>().TakeDamage(damage);
                }
            }
        }

        Destroy(gameObject);
    }
}

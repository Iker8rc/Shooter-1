using System.Collections;
using UnityEngine;

public class HordasController : MonoBehaviour
{
    public GameObject enemigosPrefab;
    public Transform[] spawnPoints;

    public float duratationTotal;   
    public float timeHordas;
    public int enemigosAparecen;
    
    private float tiempoOleada;
    private bool spawn = true;

    void Start()
    {
        StartCoroutine(SpawnHordas());
    }

    IEnumerator SpawnHordas()
    {
        while (spawn)
        {
            SpawnHord();

            yield return new WaitForSeconds(timeHordas);

            tiempoOleada += timeHordas;

            if (tiempoOleada >= duratationTotal)
            {
                spawn = false;
                Debug.Log("Fin de las oleadas");
            }
        }
    }

    void SpawnHord()
    {
        Debug.Log("Nueva oleada");

        for (int i = 0; i < enemigosAparecen; i++)
        {
            Transform spawn = spawnPoints[Random.Range(0, spawnPoints.Length)];
            Instantiate(enemigosPrefab, spawn.position, spawn.rotation);
        }
    }
}

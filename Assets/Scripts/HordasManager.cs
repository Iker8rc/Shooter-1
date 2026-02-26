using System.Collections;
using UnityEngine;
using TMPro;

public class HordasController : MonoBehaviour
{
    public GameObject enemigosPrefab;
    public Transform[] spawnPoints;

    public float duratationTotal;   
    public float timeHordas;
    public int enemigosAparecen;
    
    private float tiempoOleada;
    private bool spawn = true;

    public TextMeshProUGUI timerText;

    void Start()
    {
        StartCoroutine(SpawnHordas());
        StartCoroutine(CountdownTimer());
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

    // Para la UI
     IEnumerator CountdownTimer()
    {
        float tiempo = duratationTotal;

        while (tiempo > 0)
        {
            tiempo -= Time.deltaTime;

            // Para convertirlo a minutos y segundos sisas

            int minutos = Mathf.FloorToInt(tiempo / 60);
            int segundos = Mathf.FloorToInt(tiempo % 60);
            timerText.text = $"{minutos:00}:{segundos:00}";
            yield return null;
        }
        spawn = false;
        //Win();
    }
}

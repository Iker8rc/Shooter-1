using System.Collections;
using UnityEngine;
using TMPro;

public class HordasEnemigos : MonoBehaviour
{
    [Header("Prefabs enemigos")]
    public GameObject[] enemigosPrefabs = new GameObject[2];
    public GameObject bossPrefab;

    [Header("Spawns")]
    public Transform[] spawnPoints;

    [Header("Hordas")]
    public float duracionTotal = 600f;  
    public float tiempoEntreHordas = 120f;
    public int enemigosPorHorda = 5;  

    [Header("UI")]
    public TextMeshProUGUI timerText;

    private float tiempoPasado = 0f;
    private int hordasGeneradas = 0;
    private bool spawnActivo = true;

    void Start()
    {
        StartCoroutine(ControlHordas());
        StartCoroutine(ContadorTiempo());
    }

    IEnumerator ControlHordas()
    {
        while (spawnActivo)
        {
            GenerarHorda();
            hordasGeneradas++;

            if (hordasGeneradas >= 5) 
            {
                spawnActivo = false;
                Debug.Log("Fin de las hordas");
                yield break;
            }

            yield return new WaitForSeconds(tiempoEntreHordas);
            tiempoPasado += tiempoEntreHordas;

            if (tiempoPasado >= duracionTotal)
            {
                spawnActivo = false;
                Debug.Log("Fin por tiempo");
            }
        }
    }

    void GenerarHorda()
    {
        Debug.Log("Nueva horda lol" + (hordasGeneradas + 1));

        int enemigosEstaHorda = enemigosPorHorda + hordasGeneradas;
        bool Boss = hordasGeneradas >= 3; 

        for (int i = 0; i < enemigosEstaHorda; i++)
        {
            Transform punto = spawnPoints[Random.Range(0, spawnPoints.Length)];
            GameObject prefabAleatorio = enemigosPrefabs[Random.Range(0, enemigosPrefabs.Length)];
            Instantiate(prefabAleatorio, punto.position, punto.rotation);
        }

        if (Boss && bossPrefab != null)
        {
            Transform spawnBoss = spawnPoints[Random.Range(0, spawnPoints.Length)];
            Instantiate(bossPrefab, spawnBoss.position, spawnBoss.rotation);
        }
    }

    IEnumerator ContadorTiempo()
    {
        float tiempo = duracionTotal;

        while (tiempo > 0 && spawnActivo)
        {
            tiempo -= Time.deltaTime;

            int minutos = Mathf.FloorToInt(tiempo / 60);
            int segundos = Mathf.FloorToInt(tiempo % 60);

            timerText.text = $"{minutos:00}:{segundos:00}";

            yield return null;
        }
        spawnActivo = false;
    }
}

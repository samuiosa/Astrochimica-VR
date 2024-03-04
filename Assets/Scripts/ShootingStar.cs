using UnityEngine;
/*Questo script gestisce la creazione e il movimento di un sistema di particelle che rappresenta una stella cadente che va in una direzione casuale.
Intervallo di spawn, velocità, tempo di vita e distanza sono tutti regolabili a piacimento
Una coroutine di spawn verrà poi chiamata ogni spawnInterval secondi.

La posizione viene ottenuta tracciando una sfera intorno all'oggetto con un raggio pari a spawnDistance. La funzione posizionerà casualmente la stella su questa sfera*/
public class ShootingStar : MonoBehaviour
{
    public ParticleSystem particleSystemPrefab;
    public float spawnInterval = 30.0f;
    public float destroyTime = 5.0f;
    public float spawnDistance = 20f; // massima distanza in cui generare una stella, a partire dalla posizione del generatore
    public float speed = 5f;

    void Start()
    {
        InvokeRepeating("SpawnParticleSystem", 0.0f, spawnInterval);
    }

    void SpawnParticleSystem()
    {
        ParticleSystem newParticleSystem = Instantiate(particleSystemPrefab, GetRandomSpawnPosition(), Quaternion.identity, transform);
        Vector3 randomDirection = Random.onUnitSphere;
        newParticleSystem.GetComponent<Rigidbody>().velocity = randomDirection * speed;
        newParticleSystem.transform.LookAt(newParticleSystem.transform.position + -randomDirection); // Questa riga serve a fare in modo che la "testa" della stella segua la direzione del movimento
        Destroy(newParticleSystem.gameObject, destroyTime);
    }

    Vector3 GetRandomSpawnPosition()
    {
        Vector2 randomCircle = Random.insideUnitCircle * spawnDistance; //utilizza spawnDistance per ottenere una posizione casuale all'interno di un cerchio sulla superficie di una sfera
        return new Vector3(randomCircle.x, randomCircle.y, 0.0f);
    }
}

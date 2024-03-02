using UnityEngine;

public class ShootingStar : MonoBehaviour
{
    public ParticleSystem particleSystemPrefab;
    public float spawnInterval = 30.0f; // Intervallo tra la creazione di ciascun sistema di particelle
    public float destroyTime = 5.0f; // Tempo massimo prima della distruzione
    public float spawnDistance = 20f;
    public float speed = 5f; // Velocità di movimento dell'oggetto

    void Start()
    {
        // Avvia la creazione dei sistemi di particelle con l'intervallo specificato
        InvokeRepeating("SpawnParticleSystem", 0.0f, spawnInterval);
    }

    void SpawnParticleSystem()
    {
        // Crea il sistema di particelle direttamente nell'oggetto corrente
        ParticleSystem newParticleSystem = Instantiate(particleSystemPrefab, GetRandomSpawnPosition(), Quaternion.identity, transform);

        // Genera una direzione casuale
        Vector3 randomDirection = Random.onUnitSphere;

        // Modifica la velocità del sistema di particelle per riflettere la direzione casuale
        newParticleSystem.GetComponent<Rigidbody>().velocity = randomDirection * speed;

        // Ruota il sistema di particelle in modo che la "testa" segua la direzione del movimento
        newParticleSystem.transform.LookAt(newParticleSystem.transform.position + -randomDirection);

        // Distruggi il sistema di particelle dopo un certo periodo
        Destroy(newParticleSystem.gameObject, destroyTime);
    }

    Vector3 GetRandomSpawnPosition()
    {
        // Genera coordinate casuali in un cerchio
        Vector2 randomCircle = Random.insideUnitCircle * spawnDistance;

        // Imposta la coordinata Z a 0 (piano XY)
        return new Vector3(randomCircle.x, randomCircle.y, 0.0f);
    }
}

using UnityEngine;
using System.Collections;

public class AtomThrower : MonoBehaviour
{
    public GameObject atomPrefabO; // Prefab dell'atomo O
    public GameObject atomPrefabH; // Prefab dell'atomo H
    public Transform parent; // Parent degli atomi
    private AtomDestroyer destroyerO; // Importo lo script di distruzione atomi per gestire il timer
    private AtomDestroyer destroyerH;
    public float lifeTime=10;
    public float spawnInterval = 2f; // Intervallo tra gli atomi in secondi
    private bool waitingForSpawn = false; // Flag per il controllo dello spawn

    void Start()
    {
        destroyerO=atomPrefabO.GetComponent<AtomDestroyer>();
        destroyerO.timeLimit=lifeTime;
        destroyerH=atomPrefabH.GetComponent<AtomDestroyer>();
        destroyerH.timeLimit=lifeTime;
        StartCoroutine(SpawnAtoms());
    }

    IEnumerator SpawnAtoms()
    {
        while (true)
        {
            if (!waitingForSpawn)
            {
                // Scelgo casualmente tra atomi O e H - il rateo di spawn è 3:1 per gli H
                GameObject atomPrefab = Random.Range(0f, 1f) < 0.75f ? atomPrefabH : atomPrefabO;

                // Ottengo una posizione casuale all'interno dell'oggetto "SpawnArea"
                Vector3 spawnPosition = GetRandomSpawnPosition();

                // Creo una nuova istanza dell'atomo alla posizione specificata
                GameObject atomInstance = Instantiate(atomPrefab, spawnPosition, Quaternion.identity);

                // Imposto l'oggetto genitore dell'atomo
                atomInstance.transform.parent = parent;

                // Imposto il flag per indicare che è necessario attendere prima dello spawn successivo
                waitingForSpawn = true;

                // Aspetto per il tempo definito prima di consentire il prossimo spawn
                yield return new WaitForSeconds(spawnInterval);

                // Resetto il flag per consentire il prossimo spawn
                waitingForSpawn = false;
            }
            else
            {
                // Aspetto prima di controllare di nuovo per il prossimo spawn
                yield return null;
            }
        }
    }

    // Funzione per ottenere una posizione casuale all'interno dell'oggetto "SpawnArea"
    Vector3 GetRandomSpawnPosition()
    {
        Collider spawnCollider = GetComponent<Collider>();
        Vector3 randomPoint = new Vector3(
            Random.Range(spawnCollider.bounds.min.x, spawnCollider.bounds.max.x),
            Random.Range(spawnCollider.bounds.min.y, spawnCollider.bounds.max.y),
            Random.Range(spawnCollider.bounds.min.z, spawnCollider.bounds.max.z)
        );
        return randomPoint;
    }
}

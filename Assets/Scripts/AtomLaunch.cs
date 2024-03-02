using UnityEngine;
using System.Collections;

public class AtomLaunch : MonoBehaviour
{
    public GameObject atomPrefabO; // Prefab dell'atomo O
    public GameObject atomPrefabH; // Prefab dell'atomo H
    public float speed = 5f; // Velocità di movimento degli atomi
    public float spawnInterval = 0.1f; // Intervallo tra gli atomi
    public Transform parentObject; // Oggetto genitore degli atomi
    public Timer timer; // Riferimento al timer

    void Start()
    {
        // Chiamo una funzione che inizierà il movimento degli atomi
        StartCoroutine(SpawnAndMoveAtoms());
    }

    IEnumerator SpawnAndMoveAtoms()
    {
        while (true)
        {
            if (timer != null && !timer.timeUp)
            {
                // Scelgo casualmente tra atomi O e H - il rateo di spawn è 2:1 per gli H
                GameObject atomPrefab = Random.Range(0f, 1f) < 2/3f ? atomPrefabH : atomPrefabO;

                // Ottengo la posizione di spawn corrente
                Vector3 spawnPosition = transform.position;

                // Creo una nuova istanza dell'atomo alla posizione specificata
                GameObject atomInstance = Instantiate(atomPrefab, spawnPosition, Quaternion.identity);

                // Imposto l'oggetto genitore dell'atomo
                if (parentObject != null)
                    atomInstance.transform.parent = parentObject;

                // Ottengo il componente Rigidbody dell'atomo
                Rigidbody atomRigidbody = atomInstance.GetComponent<Rigidbody>();

                // Imposto la velocità iniziale dell'atomo verso il basso
                atomRigidbody.velocity = Vector3.down * speed;

                yield return new WaitForSeconds(spawnInterval);
            }
            else
            {
                // Se il timer è scaduto, esco dal ciclo
                break;
            }
        }
    }
}
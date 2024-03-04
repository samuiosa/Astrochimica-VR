using UnityEngine;
[RequireComponent (typeof(Collider))]
/*Questo script gestisce lo spawn degli atomi nel secondo minigioco,  esso va assegnato ad un oggetto spawner che ha bisogno di un collider.
Gli atomi vengono fatti apparire in posizioni casuali all'interno del collider dell'oggetto spawner a cui assegno questo script.
SpawnAtom verrà eseguita solo se il timer è ancora attivo. Se è scaduto, l'invocazione ripetuta verrà annullata, impedendo ulteriori chiamate
NB: In questo caso non serve uno spawner per ogni atomo in quanto le coordinate sono randomiche
Nel mio caso la lifeTime è molto inferiore rispetto all'altro gioco per evitare problemi, in quanto il giocatore è immobile e gli atomi appaiono a caso intorno a lui: Un numero eccessivo di atomi nell'ambiente lo disturberebbe e basta*/
public class AtomRandom : MonoBehaviour
{
    public GameObject atomPrefabO; //Prefab degli atomi da far spawnare
    public GameObject atomPrefabH;
    public Transform parent; // Parent degli atomi per mantenere ordinata la gerarchia della scena
    public float lifeTime=10; //Tempo di vita di un atomo inutilizzato
    public float spawnInterval = 2f; // Intervallo tra gli atomi
    public Timer timer; // Riferimento al timer per bloccare lo spawn quando arriva alla fine
    private AtomDestroyer destroyerO;
    private AtomDestroyer destroyerH;

    void Start()
    {
        destroyerO=atomPrefabO.GetComponent<AtomDestroyer>();
        destroyerO.timeLimit=lifeTime;
        destroyerH=atomPrefabH.GetComponent<AtomDestroyer>();
        destroyerH.timeLimit=lifeTime;
        InvokeRepeating("SpawnAtom", 0f, spawnInterval);
    }

    void SpawnAtom()
    {
        if(timer!=null && !timer.timeUp){
            GameObject atomPrefab = Random.Range(0f, 1f) < 0.75f ? atomPrefabH : atomPrefabO; //Questa riga serve a gestire il rateo di spawn degli atomi. Attualmente è 3:1 per gli Idrogeni
            Vector3 spawnPosition = GetRandomSpawnPosition();
            GameObject atomInstance = Instantiate(atomPrefab, spawnPosition, Quaternion.identity);
            atomInstance.transform.parent = parent;
        }
        else{
            CancelInvoke("SpawnAtom");
        }
    }

    // Funzione per ottenere una posizione casuale all'interno dell'oggetto
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

using UnityEngine;
/* Questo script serve a distruggere gli atomi che non vengono utilizzati. Ciò avviene in due casi:
    - se toccano un collider col tag "Destroyer" ai limiti della mappa
    - se un atomo resta alla sua posizione di spawn per per timeLimit tempo viene distrutto

Essendo lo script assegnato ai prefab degli atomi, in ogni minigioco sarà inserito un riferimento a questo script
in modo da avere la possibilità di poter gestire il timeLimit a seconda del gioco*/
public class AtomDestroyer : MonoBehaviour
{
    public float timeLimit=5f; // Tempo di vita massimo di un atomo inutilizzato
    private float lifeTime=0; // Tiene traccia da quanto è spawnato l'atomo
    private Vector3 spawnPosition; // Posizione di spawn dell'atomo
    void Start()
    {
        spawnPosition=transform.position;
    }
    void Update()
    {
        if(spawnPosition==transform.position)
        {
            lifeTime+=Time.deltaTime;
            if (lifeTime>=timeLimit)
                {
                    Destroy(gameObject);
                }
        }
        
    }
    void OnCollisionEnter(Collision other)
    {
        if (other.collider.CompareTag("Destroyer"))
        {
            Destroy(gameObject);
        }
    }
}

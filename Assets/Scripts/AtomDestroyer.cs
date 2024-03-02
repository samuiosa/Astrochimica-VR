using UnityEngine;

public class AtomDestroyer : MonoBehaviour
{
    private Vector3 spawnPosition;
    public float timeLimit=5f;
    private float lifeTime=0;
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
                    Debug.Log("Tempo limite trascorso");
                    Destroy(gameObject);
                }
        }
        
    }
    void OnCollisionEnter(Collision other)
    {
        if (other.collider.CompareTag("Destroyer"))
        {
            // Distruggo l'atomo quando tocca l'oggetto con il tag "destroyer"
            Destroy(gameObject);
        }
    }
}

using UnityEngine;
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(ParticleSystem))]
/*Lo script gestisce gli eventi da far partire quando una molecola appare a schermo
Vengono fatti partire un effetto particellare e una clip audio per dare un feedback al giocatore
Gestisce anche il caso in cui una molecola dovesse toccare un oggetto Destroyer, per evitare di avere molecole vaganti per la mappa
A differenza degli atomi, una molecola NON viene eliminata per inattività*/
public class Molecola : MonoBehaviour
{
    public ParticleSystem molecolaParticleSystem;
    public AudioClip audioMolecola;

    private void Start()
    {
        if (molecolaParticleSystem != null)
        {
            molecolaParticleSystem.Play();
        }
        if (audioMolecola != null)
        {
            AudioSource audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }

            audioSource.clip = audioMolecola;
            audioSource.Play();
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

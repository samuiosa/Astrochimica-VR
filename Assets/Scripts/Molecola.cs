using UnityEngine;
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(ParticleSystem))]
public class Molecola : MonoBehaviour
{
    public ParticleSystem molecolaParticleSystem;
    public AudioClip audioMolecola;

    private void Start()
    {
        // Avvia il Particle System
        if (molecolaParticleSystem != null)
        {
            molecolaParticleSystem.Play();
        }

        // Avvia l'audio della molecola
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
            // Distruggo la molecola quando tocca l'oggetto con il tag "destroyer"
            Destroy(gameObject);
        }
    }
}

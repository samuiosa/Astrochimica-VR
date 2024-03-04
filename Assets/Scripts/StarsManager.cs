using UnityEngine;
using UnityEngine.Rendering;
[RequireComponent(typeof(ParticleSystem))]
/*Questo script gestisce le stelle che decorano l'ambiente spaziale.
In particolare il sistema di particelle deve seguire tutti i movimenti dell'utente, in modo da dargli l'illusione di essere nello spazio.
Colore e luminosità vengono aggiornati ad ogni frame in modo da mantenere una coerenza visiva anche quando le particelle vengono pregenerate o quando il sistema di particelle è già attivo*/
public class StarsManager : MonoBehaviour
{
    private Camera m_mainCamera;
    private ParticleSystem starsParticleSystem;

    public float brightness = 1.0f; // Luminosità desiderata, valori troppo elevati possono disturbare il giocatore

    void Start()
    {
        //All'oggetto vengono assegnati la posizione e il parent della Camera principale, in modo da garantire che le stelle siano sempre nella vista della camera
        m_mainCamera = Camera.main;
        transform.position = m_mainCamera.transform.position;
        transform.parent = m_mainCamera.transform;

        starsParticleSystem = GetComponent<ParticleSystem>();
        var StarsRenderer = GetComponent<ParticleSystemRenderer>();
        StarsRenderer.material.renderQueue=(int) RenderQueue.Background; //Questa riga serve a farle apparire dietro agli altri oggetti. In sua assenza vedremmo le stelle sopra agli altri oggetti
    }

    void Update()
    {
        transform.rotation = Quaternion.identity; //La rotazione deve essere sempre allineata con la vista della camera
        var mainModule = starsParticleSystem.main;
        var startColor = mainModule.startColor.color;
        startColor.r = brightness;
        startColor.g = brightness;
        startColor.b = brightness;
        mainModule.startColor = startColor;
        ParticleSystem.Particle[] particles = new ParticleSystem.Particle[starsParticleSystem.particleCount]; //In questo modo posso aggiornare anche le particelle già spawnate
        int numParticlesAlive = starsParticleSystem.GetParticles(particles);
        for (int i = 0; i < numParticlesAlive; i++)
        {
            particles[i].startColor = startColor;
        }
        starsParticleSystem.SetParticles(particles, numParticlesAlive);
    }
}

using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(ParticleSystem))]

public class StarsManager : MonoBehaviour
{
    private Camera m_mainCamera;
    private ParticleSystem starsParticleSystem;

    public float brightness = 1.0f; // Imposta la luminosità desiderata

    void Start()
    {
        m_mainCamera = Camera.main;
        transform.position = m_mainCamera.transform.position;
        transform.parent = m_mainCamera.transform;

        // Ottiene il componente ParticleSystem
        starsParticleSystem = GetComponent<ParticleSystem>();
        var StarsRenderer = GetComponent<ParticleSystemRenderer>();
        StarsRenderer.material.renderQueue=(int) RenderQueue.Background;
    }

    void Update()
    {
        transform.rotation = Quaternion.identity;

        // Modifica la luminosità delle particelle create durante il prewarm
        var mainModule = starsParticleSystem.main;
        var startColor = mainModule.startColor.color;

        // Modifica il canale rosso, verde e blu
        startColor.r = brightness;
        startColor.g = brightness;
        startColor.b = brightness;

        mainModule.startColor = startColor;

        // Ottiene tutte le particelle attuali
        ParticleSystem.Particle[] particles = new ParticleSystem.Particle[starsParticleSystem.particleCount];
        int numParticlesAlive = starsParticleSystem.GetParticles(particles);

        // Modifica la luminosità delle particelle già create
        for (int i = 0; i < numParticlesAlive; i++)
        {
            particles[i].startColor = startColor;
        }

        // Riapplica le particelle modificate al sistema
        starsParticleSystem.SetParticles(particles, numParticlesAlive);
    }
}

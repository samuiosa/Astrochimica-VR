using UnityEngine;

public class SeguiGiocatore : MonoBehaviour
{
    public Transform target; // Riferimento alla main camera
    public float smoothSpeed = 0.125f; // Velocità di inseguimento
    public Vector3 offset; // Offset dalla posizione

    private void Start()
    {
        // Imposta la posizione iniziale con l'offset
        transform.position = target.position + offset;
    }

    private void Update()
    {
        // Calcola la posizione desiderata
        Vector3 desiredPosition = target.position + offset;
        desiredPosition.y = transform.position.y; // Mantieni la Y invariata

        // Segui il target con una velocità graduale
        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothSpeed);
    }

    private Vector3 velocity = Vector3.zero;
}
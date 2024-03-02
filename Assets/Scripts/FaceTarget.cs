using UnityEngine;

public class FaceTarget : MonoBehaviour
{
    void Update()
    {
        // Ottieni la posizione della telecamera principale.
        Vector3 posizioneCamera = Camera.main.transform.position;

        // Calcola la direzione dalla telecamera al piano.
        Vector3 direzione = transform.position - posizioneCamera;

        // Ruota il piano in modo che sia sempre rivolto verso la telecamera.
        Quaternion targetRotation = Quaternion.LookRotation(direzione, Vector3.up);
        // Aggiungi 90 gradi agli assi della rotazione.
        targetRotation *= Quaternion.Euler(0, 90, 90);

        // Assegna la nuova rotazione al transform dell'oggetto.
        transform.rotation = targetRotation;
    }
}
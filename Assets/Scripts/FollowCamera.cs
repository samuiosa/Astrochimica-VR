using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Camera UICamera;
    public float offsetDistance = 5f;
    public float smoothness=5f;
    void LateUpdate()
        {
        // Calcola la nuova posizione del Canvas UI in base alla telecamera
        Vector3 newPosition = UICamera.transform.position + UICamera.transform.forward * offsetDistance;

        // Applica la nuova posizione al Canvas UI
        transform.position = Vector3.Lerp(transform.position, newPosition, Time.unscaledDeltaTime * smoothness);

        // Mantieni la rotazione verso la telecamera (se necessario)
        transform.rotation=UICamera.transform.rotation;
        }
}

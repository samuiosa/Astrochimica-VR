using UnityEngine;

public class ResetCamera : MonoBehaviour
{
    // Posizioni di reset consentite
    public float[] resetXPositions = { 5.0f, -3.5f };
    public float[] resetYPositions = { -15.0f, 12.0f };
    public float[] resetZPositions = { -12.0f, 20.0f };

    // Posizione iniziale della telecamera
    private Vector3 initialPosition;

    void Start()
    {
        // Salva la posizione iniziale della telecamera
        initialPosition = transform.position;
    }

    void Update()
    {
        // Controlla se la telecamera ha raggiunto una delle posizioni di reset
        if (ShouldResetPosition())
        {
            Debug.Log("Out of bounds!");
            // Resetta la posizione della telecamera
            transform.position = initialPosition;
        }
    }

    bool ShouldResetPosition()
    {
        // Ottieni la posizione corrente della telecamera
        Vector3 currentPos = transform.position;

        // Controlla se la posizione corrente rientra nelle posizioni di reset consentite
        return IsWithinRange(currentPos.x, resetXPositions) &&
               IsWithinRange(currentPos.y, resetYPositions) &&
               IsWithinRange(currentPos.z, resetZPositions);
    }

    bool IsWithinRange(float value, float[] range)
    {
        // Controlla se il valore è compreso nell'intervallo specificato
        return value >= Mathf.Min(range) && value <= Mathf.Max(range);
    }
}
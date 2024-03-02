using UnityEngine;
public class DimensioneAtomo : MonoBehaviour
{
    public float vanDerWaalsRadius = 1.0f; // Inserisci il raggio di van der Waals dell'atomo

    void Start()
    {
        // Scala l'atomo in base al raggio di van der Waals
        float scaleFactor= vanDerWaalsRadius*0.1f;
        transform.localScale = new Vector3(scaleFactor,scaleFactor,scaleFactor);
    }
}

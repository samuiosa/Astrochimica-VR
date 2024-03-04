using UnityEngine;
//Dato in input il raggio di Van Der Waals di un atomo lo scala in modo da renderlo visibile
public class DimensioneAtomo : MonoBehaviour
{
    public float vanDerWaalsRadius = 1.0f; // Raggio dell'atomo

    void Start()
    {
        // Scala l'atomo per renderlo visibile. 1/10 mi è sembrato il valore più ragionevole ai fini del gioco
        float scaleFactor= vanDerWaalsRadius*0.1f;
        transform.localScale = new Vector3(scaleFactor,scaleFactor,scaleFactor);
    }
}

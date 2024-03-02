using UnityEngine;

public class MovGrano : MonoBehaviour
{
    public float velocitaFacile = 0.5f; // Velocità quando la difficoltà è impostata su Facile
    public float velocitaDifficile = 2f; // Range di movimento quando la difficoltà è impostata su Difficile
    private Vector3 posReset;
    public Vector3 posIniziale;
    public Vector3 posFinale;
    public Timer timer; // Riferimento al timer
    public Score score; // Riferimento al punteggio
    public TMPro.TMP_Dropdown dropdownDifficolta;

    // Start is called before the first frame update
    void Start()
    {
        posReset = transform.position;
        // Avvia la coroutine per far muovere il grano continuamente
        StartCoroutine(MuoviGranoContinuamente());
    }

    private System.Collections.IEnumerator MuoviGranoContinuamente()
    {
        while (true)
        {
            ImpostaDifficoltà(); // Aggiorna la difficoltà ogni iterazione
            yield return null;
        }
    }

    public void ImpostaDifficoltà()
    {
        // Ottieni il valore selezionato nel Dropdown
        string valoreDifficolta = dropdownDifficolta.options[dropdownDifficolta.value].text;

        // Chiamiamo la funzione corrispondente alla difficoltà selezionata
        switch (valoreDifficolta)
        {
            case "Grano Fermo":
                GranoFermo();
                break;

            case "Facile":
                GranoFacile();
                break;

            case "Difficile":
                GranoDifficile();
                break;

            default:
                Debug.LogWarning("Difficoltà non riconosciuta: " + valoreDifficolta);
                break;
        }
    }

    private void GranoFermo()
    {
        transform.position=posReset;
    }

    private void GranoFacile()
    {
        float t = Mathf.PingPong(Time.time * velocitaFacile, 1);
        transform.localPosition = Vector3.Lerp(posIniziale, posFinale, t);
    }

    private void GranoDifficile()
    {
        float t = Mathf.PingPong(Time.time * velocitaDifficile, 1);
        transform.localPosition = Vector3.Lerp(posIniziale, posFinale, t);
    }
}

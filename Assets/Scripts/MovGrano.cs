using UnityEngine;
/* Questo script gestisce i movimenti del grano nel secondo gioco, a seconda della difficoltà scelta dal giocatore dal menu dropdown
ogni volta che viene cambiato valore nel dropdown [OnValueChanged], viene chiamata la funzione ImpostaDifficoltà, che prende l'id dell'opzione scelta.
Ad ogni frame viene chiamata la funzione difficoltà corrispondente, in modo da mantenere il comportamento del grano coerente con l'opzione scelta.
A facile e difficile il grano si muove avanti e indietro (Mathf.PingPong) da posIniziale a posFinale ad una velocità regolabile.
A difficile, inoltre, il grano si muove simulando un onda sinusoidale sugli assi y e z, con un offset regolabile*/

public class MovGrano : MonoBehaviour
{
    public float velocitaFacile = 0.5f;
    public float velocitaDifficile = 2f;
    public float offsetY=0;
    public float offsetZ=0;
    public Vector3 posIniziale;
    public Vector3 posFinale;
    public Timer timer;
    public Score score;
    public TMPro.TMP_Dropdown dropdownDifficolta;
    private Vector3 posReset;
    private int valoreDifficolta;
    void Start()
    {
        posReset = transform.position;
    }

    public void ImpostaDifficoltà()
    {
        valoreDifficolta = dropdownDifficolta.value;
    }
    void Update(){
        switch (valoreDifficolta)
        {
            case 0:
                GranoFermo();
                break;

            case 1:
                GranoFacile();
                break;

            case 2:
                GranoDifficile();
                break;

            default:
                Debug.Log("Indice errato: " + dropdownDifficolta.value);
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
        transform.position = Vector3.Lerp(posIniziale, posFinale, t);
    }

    private void GranoDifficile()
    {
        float t = Mathf.PingPong(Time.time * velocitaDifficile, 1);
        Vector3 posizione = Vector3.Lerp(posIniziale, posFinale, t);
        posizione.y += Mathf.Sin(Time.time * velocitaDifficile)*offsetY;
        posizione.z+=Mathf.Sin(Time.time * velocitaDifficile)*offsetZ;
        transform.position = posizione;
    }
}

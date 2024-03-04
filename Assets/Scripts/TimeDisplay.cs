using UnityEngine;
using TMPro;
/*Questo script gestisce semplicemente il tempo, mostrando su un TextMeshPro solo la parte intera.
Update aggiorna ad ogni frame il testo, decrementando il tempo fino allo 0, quando scriverà "Fine"
La funzione ResetTimer viene chiamata dal grano quando il giocatore cambia la difficoltà, in modo da resettare il livello*/
public class Timer : MonoBehaviour
{
    public float timeRemaining = 120;
    public bool timeUp=false;
    private TextMeshProUGUI tempoText;
    private float timeStart;
    void Start()
    {
        timeStart=timeRemaining;
        tempoText = GetComponent<TextMeshProUGUI>();
    }
    void Update()
    {
        if (timeRemaining > 0)
        {
            timeRemaining -= Time.deltaTime;
            tempoText.text="Tempo: " + Mathf.Ceil(timeRemaining).ToString();
        }
        else{
            tempoText.text="Fine";
            timeUp=true;
            this.enabled=false;
        }
            
    }
    public void ResetTimer(){
        timeRemaining=timeStart;
    }
}
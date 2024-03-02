using UnityEngine;
using TMPro;
public class Timer : MonoBehaviour
{
    public float timeRemaining = 120;
    public bool timeUp=false;
    private TextMeshProUGUI tempoText;
    private float timeStart;
    void Start()
    {
        //Riferimento al componente Text
        timeStart=timeRemaining;
        tempoText = GetComponent<TextMeshProUGUI>();
    }
    void FixedUpdate()
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
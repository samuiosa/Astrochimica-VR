using UnityEngine;
using TMPro;

public class Score : MonoBehaviour
{
    public int score;
    public Timer timer;
    private TextMeshProUGUI scoreText;
    void Start()
    {
        // Inizializza il punteggio e ottiene il riferimento al componente Text
        score=0;
        scoreText = GetComponent<TextMeshProUGUI>();
    }
    // Metodo per ottenere il valore di score
    public int GetScore()
    {
        return score;
    }

    // Metodo per impostare il valore di score
    public void SetScore(int newScore)
    {
        score = newScore;
    }
    void Update()
    {
        if (timer !=null && timer.timeUp==false){
            scoreText.text=("Punteggio: " + score.ToString());
        }
    }
}

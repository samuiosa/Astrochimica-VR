using UnityEngine;
using TMPro;
/*Questo script gestisce semplicemente il punteggio, mostrandolo su un TextMeshPro che viene aggiornato a ogni frame
Le funzioni GetScore e SetScore vengono chiamate dal grano in base alle molecole che ci si attaccano*/
public class Score : MonoBehaviour
{
    public int score;
    public Timer timer;
    private TextMeshProUGUI scoreText;
    void Start()
    {
        score=0;
        scoreText = GetComponent<TextMeshProUGUI>();
    }
    public int GetScore()
    {
        return score;
    }

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

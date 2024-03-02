using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    // Coordinate di spawn desiderate
    public Vector3 spawnPosition = new Vector3(-0.30399999f, 0f, -0.0540000014f);

    public void formAcqua()
    {
        SetSpawnPosition();
        Time.timeScale = 1f;
        SceneManager.LoadScene("Cattura atomi");
    }

    public void lancioAcqua()
    {
        SetSpawnPosition();
        Time.timeScale = 1f;
        SceneManager.LoadScene("Lancia molecole");
    }

    public void mainMenu()
    {
        SetSpawnPosition();
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void RestartLevel()
    {
        SetSpawnPosition();

        // Ripristina il tempo di gioco al valore normale prima di riavviare
        Time.timeScale = 1f;

        // Riavvia il livello corrente
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }

    private void SetSpawnPosition()
    {
        // Trova il giocatore nella scena
        GameObject player = GameObject.FindGameObjectWithTag("VR Origin");

        if (player != null)
        {
            // Imposta le coordinate di spawn per il giocatore
            player.transform.position = spawnPosition;
        }
    }
}

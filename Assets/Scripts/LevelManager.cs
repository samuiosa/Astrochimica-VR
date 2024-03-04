using UnityEngine;
using UnityEngine.SceneManagement;
/* Il tempo va sempre settato a 1 perchè quando cambio scena dal menu di pausa il tempo è fermo, omettendo quella riga il tempo resterebbe a 0
Per passare da una scena all'altra ho usato gli ID delle scene piuttosto che i nomi
0: Menu principale
1: Cattura gli atomi
2: Lancia le molecole*/
public class LevelManager : MonoBehaviour
{
    public void ChangeScene(int sceneIndex)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneIndex);
    }
    public void RestartLevel()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}

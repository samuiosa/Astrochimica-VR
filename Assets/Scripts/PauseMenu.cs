using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    public InputActionReference pausaButton; //Riferimento al tasto sul controller
    public GameObject Pannello;
    public GameObject directInteractor;
    public GameObject rayInteractor;

    private bool isNotPaused = true; // Flag che controlla se sono già in pausa

    void Awake()
    {
        pausaButton.action.performed += TogglePauseMenu;
        pausaButton.action.Enable();
    }

    private void TogglePauseMenu(InputAction.CallbackContext context)
    {
        if (Pannello != null)
        {
            if (isNotPaused)
            {
                // Attiva il menu di pausa
                Pannello.SetActive(true);
                // Ferma il tempo di gioco
                Time.timeScale = 0f;
                // Disabilita il Direct Interactor
                directInteractor.SetActive(false);
                // Abilita il Ray Interactor
                rayInteractor.SetActive(true);
            }
            else
            {
                // Riabilita il Direct Interactor
                directInteractor.SetActive(true);
                // Disabilita il Ray Interactor
                rayInteractor.SetActive(false);
                // Chiudi il menu di pausa
                Pannello.SetActive(false);
                // Riprendi il tempo di gioco
                Time.timeScale = 1f;
            }
            // Inverti il flag
            isNotPaused = !isNotPaused;
        }
    }
    public void ResumeGame(){
        // Riabilita il Direct Interactor
        directInteractor.SetActive(true);
        // Disabilita il Ray Interactor
        rayInteractor.SetActive(false);
        // Chiudi il menu di pausa
        Pannello.SetActive(false);
        // Riprendi il tempo di gioco
        Time.timeScale = 1f;
        // Inverti il flag
        isNotPaused = !isNotPaused;
    }
}

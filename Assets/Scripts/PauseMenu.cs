using UnityEngine;
using UnityEngine.InputSystem;
/*Questo script gestisce il menu di pausa
Quando viene rilevato che il giocatore ha premuto il tasto pausa sul controller (tramite InputActionReference), viene lanciata TogglePauseMenu
Questa funzione innanzitutto verifica se il giocatore è già in pausa o meno, grazie al boolean isNotPaused:
    - se NON è in pausa attiva il pannello con le varie opzioni, ferma il tempo e scambia direct interactor con ray interactor
    - se è in pausa fa l'opposto
è fondamentale scambiare gli interactor in quanto il gioco si basa sull'afferrare gli oggetti con il direct interactor, che però non permetterebbe di selezionare le opzioni sul menu di pausa. In questo caso è quindi necessario il Ray
ResumeGame viene chiamata sia se l'utente seleziona "Riprendi", sia se preme il tasto pausa quando è già in pausa*/
public class PauseMenu : MonoBehaviour
{
    public InputActionReference pausaButton; //Riferimento al tasto sul controller
    public GameObject Pannello;
    public GameObject directInteractor;
    public GameObject rayInteractor;

    private bool isNotPaused = true;

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
                Pannello.SetActive(true);
                Time.timeScale = 0f;
                directInteractor.SetActive(false);
                rayInteractor.SetActive(true);
                isNotPaused = !isNotPaused;
            }
            else
            {
                ResumeGame();
            }
            
        }
    }
    public void ResumeGame(){
        directInteractor.SetActive(true);
        rayInteractor.SetActive(false);
        Pannello.SetActive(false);
        Time.timeScale = 1f;
        isNotPaused = !isNotPaused;
    }
}

using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public GameFlowController gameFlowController;
    public ExitConfirmationPanelController confirmationPanelController;
    public void PlayGame()
    {
        gameFlowController.OnPlayButton();
    }

    public void QuitGame()
    {
        confirmationPanelController.ToggleConfirmationPanel();
    }
    public void OptionsButton()
    {
        gameFlowController.OnOptionsButton();
    }
}
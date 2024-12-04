using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public GameFlowController gameFlowController; 

    public void PlayGame()
    {
        gameFlowController.OnPlayButton();
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void OptionsButton()
    {
        gameFlowController.OnOptionsButton();
    }
}
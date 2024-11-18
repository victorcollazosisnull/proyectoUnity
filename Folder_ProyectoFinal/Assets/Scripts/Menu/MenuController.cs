using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("ScenePrototypes");
        MusicManager.Instance.StopAllMusic();
        MusicManager.Instance.PlayGameMusic();
    }
    public void QuitGame()
    {
        Application.Quit();
    }
}

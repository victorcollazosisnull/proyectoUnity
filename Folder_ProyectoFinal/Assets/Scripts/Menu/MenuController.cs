using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("Game");
        MusicManager.Instance.StopAllMusic();
        MusicManager.Instance.PlayGameMusic();
    }
}

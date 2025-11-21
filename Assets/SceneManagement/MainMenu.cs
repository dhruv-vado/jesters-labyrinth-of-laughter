using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void LoadMainMenu()
    {
        SceneLoader.LoadScene(SceneLoader.Scene.MainMenu);
    }

    public void LoadLevel()
    {
        SceneLoader.LoadScene(SceneLoader.Scene.Level);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}

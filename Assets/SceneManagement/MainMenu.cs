using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void LoadLevel()
    {
        SceneLoader.LoadScene(SceneLoader.Scene.Level);
    }
}

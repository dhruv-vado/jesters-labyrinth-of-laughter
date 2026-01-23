using UnityEngine;

public enum Difficulty
{
    Amateur,
    Intermediate,
    Professional,
    Nightmare,
    Insanity
}

public class DifficultyManager : MonoBehaviour
{
    //using this script to later assign parameters to maze gen and game manager scripts, and storing data between scenes
    #region Singleton
    public static DifficultyManager Instance;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    #endregion

    [HideInInspector]public Difficulty currentDifficulty;
    [HideInInspector]public int mazeSize;
    [HideInInspector]public int noOfClowns;

    [Header("Parameters")]
    public int minMazeSize = 8;
    public int maxMazeSize = 20;
    public int minNoOfClowns = 4;
    public int maxNoOfClowns = 16;
    
    public void GetPlayerPrefs()
    {
        currentDifficulty = (Difficulty)PlayerPrefs.GetInt("Difficulty", (int)Difficulty.Amateur);
        mazeSize = PlayerPrefs.GetInt("MazeSize", minMazeSize);
        noOfClowns = PlayerPrefs.GetInt("NoOfClowns", minNoOfClowns);
    }
}

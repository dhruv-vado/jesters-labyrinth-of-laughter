using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MainMenu : MonoBehaviour
{
    /*using this script to later assign parameters to maze gen and game manager scripts
    #region Singleton
    public static MainMenu Instance;

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
    #endregion*/
    
    [Header("UI Elements")]
    public TextMeshProUGUI  difficultyText;
    public TextMeshProUGUI  mazeSizeText;
    public TextMeshProUGUI  noOfClownsText;

    public Button difficultyLeftButton;
    public Button difficultyRightButton;
    public Button mazeSizeLeftButton;
    public Button mazeSizeRightButton;
    public Button clownLeftButton;
    public Button clownRightButton;

    private int _minMazeSize;
    private int _maxMazeSize;
    private int _minNoOfClowns;
    private int _maxNoOfClowns;

    [HideInInspector]public Difficulty _currentDifficulty;
    [HideInInspector]public int _mazeSize;
    [HideInInspector]public int _noOfClowns;

    private DifficultyManager difficultyManager;

    private void Start()
    {
        difficultyManager = DifficultyManager.Instance;
        difficultyManager.GetPlayerPrefs();
        UpdateUI();
    }

    public void ChangeDifficulty(int i) //using i to decide whether to increase or decrease
    {
        if(i==0)
        {
            _currentDifficulty--;
            if ((int)_currentDifficulty == 0)
                difficultyLeftButton.interactable = false;
            difficultyText.text = _currentDifficulty.ToString();
            difficultyRightButton.interactable = true;
        }
        else if(i==1)
        {
            _currentDifficulty++;
            if ((int)_currentDifficulty == System.Enum.GetValues(typeof(Difficulty)).Length - 1)
                difficultyRightButton.interactable = false;
            difficultyText.text = _currentDifficulty.ToString();
            difficultyLeftButton.interactable = true;
        }
        else
        {
            Debug.Log("Difficulty Button Error");
        }

        PlayerPrefs.SetInt("Difficulty", (int)_currentDifficulty);
        PlayerPrefs.Save();
        difficultyManager.GetPlayerPrefs();
    }

    public void ChangeMazeSize(int i)
    {
        if(i==0)
        {
            _mazeSize--;
            if (_mazeSize == _minMazeSize)
                mazeSizeLeftButton.interactable = false;
            mazeSizeText.text = _mazeSize.ToString();
            mazeSizeRightButton.interactable = true;
        }
        else if(i==1)
        {
            _mazeSize++;
            if (_mazeSize == _maxMazeSize)
                mazeSizeRightButton.interactable = false;
            mazeSizeText.text = _mazeSize.ToString();
            mazeSizeLeftButton.interactable = true;
        }
        else
        {
            Debug.Log("Maze Size Button Error");
        }

        PlayerPrefs.SetInt("MazeSize", _mazeSize);
        PlayerPrefs.Save(); 
        difficultyManager.GetPlayerPrefs();
    }

    public void ChangeNoOfClowns(int i)
    {
        if(i==0)
        {
            _noOfClowns--;
            if (_noOfClowns == _minNoOfClowns)
                clownLeftButton.interactable = false;
            noOfClownsText.text = _noOfClowns.ToString();
            clownRightButton.interactable = true;
        }
        else if(i==1)
        {
            _noOfClowns++;
            if (_noOfClowns == _maxNoOfClowns)
                clownRightButton.interactable = false;
            noOfClownsText.text = _noOfClowns.ToString();
            clownLeftButton.interactable = true;
        }
        else
        {
            Debug.Log("No. Of Clowns Button Error");
        }
        
        PlayerPrefs.SetInt("NoOfClowns", _noOfClowns);
        PlayerPrefs.Save(); 
        difficultyManager.GetPlayerPrefs();
    }

    public void UpdateUI()
    {
        _minMazeSize = difficultyManager.minMazeSize;
        _maxMazeSize = difficultyManager.maxMazeSize;
        _minNoOfClowns = difficultyManager.minNoOfClowns;
        _maxNoOfClowns = difficultyManager.maxNoOfClowns;

        _currentDifficulty = difficultyManager.currentDifficulty;
        _mazeSize = difficultyManager.mazeSize;
        _noOfClowns = difficultyManager.noOfClowns;

        if ((int)_currentDifficulty == 0)
            difficultyLeftButton.interactable = false;
        if ((int)_currentDifficulty == System.Enum.GetValues(typeof(Difficulty)).Length - 1)
            difficultyRightButton.interactable = false;

        if (_mazeSize == _minMazeSize)
            mazeSizeLeftButton.interactable = false;
        if (_mazeSize == _maxMazeSize)
                mazeSizeRightButton.interactable = false;

        
        if (_noOfClowns == _minNoOfClowns)
            clownLeftButton.interactable = false;
        if (_noOfClowns == _maxNoOfClowns)
                clownRightButton.interactable = false;

        difficultyText.text = _currentDifficulty.ToString();
        mazeSizeText.text = _mazeSize.ToString();
        noOfClownsText.text = _noOfClowns.ToString();
    }

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

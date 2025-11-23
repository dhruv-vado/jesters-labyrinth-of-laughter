using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FlashlightManager : MonoBehaviour
{
     #region Singleton
    public static FlashlightManager Instance;

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

    public float BatteryPercentage;
    public TextMeshProUGUI  _batteryText;
    public Slider _enemyDeathSlider;

    private void Start()
    {
        BatteryPercentage = 100f;
        _enemyDeathSlider.value = 0f;
    }

    private void Update()
    {
        _batteryText.text = "Battery: " + BatteryPercentage.ToString() + " %"; 
    }

}

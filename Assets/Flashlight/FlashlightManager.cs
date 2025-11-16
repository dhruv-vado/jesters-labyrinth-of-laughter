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

    private void Start()
    {
        BatteryPercentage = 100f;
    }

    private void Update()
    {
        _batteryText.text = BatteryPercentage.ToString() + " %"; 
    }

}

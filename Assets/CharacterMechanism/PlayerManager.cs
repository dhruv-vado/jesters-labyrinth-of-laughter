using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    #region Singleton
    public static PlayerManager Instance;

    private void Awake()
    {
        Instance = this;
    }
    #endregion

    public GameObject Player;

    public void Spawn()
    {
        Instantiate(Player,new Vector3(0f,1f,0f),Quaternion.identity);
    }
    
}

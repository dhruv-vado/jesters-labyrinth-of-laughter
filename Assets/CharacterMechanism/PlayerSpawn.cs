using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerSpawn : MonoBehaviour
{
    public GameObject player;

    void Start()
    {
        Instantiate(player,new Vector3(0f, 1f, 0f), Quaternion.identity);
    }
}

using UnityEngine;

public class LevelManager : MonoBehaviour
{

    public static LevelManager instance;

    [HideInInspector]
    public Vector3 respawnPoint;

    private PlayerController player;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        player = Object.FindAnyObjectByType<PlayerController>();

        // El punto inicial del nivel es el primer respawn
        respawnPoint = player.transform.position;
    }

    public void RespawnPlayer()
    {
        player.charCon.enabled = false;

        player.transform.position = respawnPoint;

        player.charCon.enabled = true;
    }
}
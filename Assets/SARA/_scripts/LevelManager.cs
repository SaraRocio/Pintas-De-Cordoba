using UnityEngine;

public class LevelManagerRunner : MonoBehaviour
{

    public static LevelManagerRunner instance;

    [HideInInspector]
    public Vector3 respawnPoint;

    private PlayerRunner player;

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
        // Buscar al jugador actual de la escena.
        player = Object.FindAnyObjectByType<PlayerRunner>();

        if (player == null)
        {
            Debug.LogError("No se encontró un PlayerRunner en la escena.");
            return;
        }

        // El punto inicial del nivel es el primer respawn.
        respawnPoint = player.transform.position;
    }

    public void RespawnPlayer()
    {
        // Buscar nuevamente al jugador por si la referencia anterior
        // ya no es válida.
        player = Object.FindAnyObjectByType<PlayerRunner>();

        if (player == null)
        {
            Debug.LogError("No se encontró a Deiva para reaparecer.");
            return;
        }

        // Obtener el CharacterController actual.
        CharacterController charCon =
            player.GetComponent<CharacterController>();

        if (charCon == null)
        {
            Debug.LogError(
                "Deiva no tiene un componente CharacterController."
            );
            return;
        }

        // Desactivar temporalmente el CharacterController.
        charCon.enabled = false;

        // Mover a Deiva al último checkpoint.
        player.transform.position = respawnPoint;

        // Activar nuevamente el CharacterController.
        charCon.enabled = true;
    }
}
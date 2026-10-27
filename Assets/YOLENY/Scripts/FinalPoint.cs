
using UnityEngine;

public class FinalPoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Algo entró al punto final: " + other.name);

        if (!other.CompareTag("Player"))
        {
            Debug.LogWarning("El objeto no tiene el tag Player.");
            return;
        }

        PlayerRunner player = other.GetComponent<PlayerRunner>();

        if (player == null)
        {
            player = other.GetComponentInParent<PlayerRunner>();
        }

        if (player != null)
        {
            Debug.Log("PlayerRunner encontrado. Deteniendo jugador.");
            player.StopAtFinish();
        }
        else
        {
            Debug.LogError("No se encontró PlayerRunner en el jugador ni en sus padres.");
        }
    }
}

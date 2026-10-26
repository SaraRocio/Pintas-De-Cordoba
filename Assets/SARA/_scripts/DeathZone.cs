using UnityEngine;

public class DeathZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Player"))
        {
            Debug.Log("Player ha entrado en la zona de muerte");
            LevelManager.instance.RespawnPlayer();
            Debug.Log("Player ha muerto");
        }
    }
}

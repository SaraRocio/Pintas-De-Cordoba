using UnityEngine;

public class FinalPoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerRunner player = other.GetComponent<PlayerRunner>();

            if (player != null)
            {
                player.StopAtFinish();
            }
        }
    }
}
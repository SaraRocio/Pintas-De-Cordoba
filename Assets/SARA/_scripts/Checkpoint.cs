using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public GameObject effect;
    public Transform effectPoint;
    public Animator anim;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player ha entrado en el checkpoint");
            if (Vector3.Distance(
                LevelManager.instance.respawnPoint,
                transform.position) > 0.01f)
            {
                LevelManager.instance.respawnPoint = transform.position;

                Debug.Log(
                    "Checkpoint: " + gameObject.name +
                    " | Posición guardada: " + transform.position
                );
                if (effect != null && effectPoint != null)
                {
                    Instantiate(
                        effect,
                        effectPoint.position,
                        Quaternion.identity
                    );
                }

                Checkpoint[] allCP = Object.FindObjectsByType<Checkpoint>();

                foreach (Checkpoint cp in allCP)
                {
                    if (cp.anim != null)
                    {
                        cp.anim.SetBool("active", false);
                    }
                }

                if (anim != null)
                {
                    anim.SetBool("active", true);
                }


            }
        }
    }
}
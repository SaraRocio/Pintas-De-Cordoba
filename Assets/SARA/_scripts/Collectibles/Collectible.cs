using UnityEngine;

public class Collectible : MonoBehaviour
{
    [Header("Información del objeto")]
    [SerializeField] private CollectibleData data;

    [Header("Efectos")]
    [SerializeField] private GameObject pickupEffect;

    public CollectibleData Data => data;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        Collect();
    }

    private void Collect()
    {
        Debug.Log("Objeto recogido: " + data.ItemName);

        CollectibleManager.instance.AddCollectible(data);


        // Compruebar si este es el objeto final del livel
        if (data.IsFinalObjective)
        {
            Debug.Log("OBJETO FINAL RECOGIDO");

            if (ObjectiveManager.instance.AllObjectivesCollected())
            {
                Debug.Log("TODOS LOS OBJETIVOS ESTÁN COMPLETOS");
            }
            else
            {
                Debug.Log("FALTA UN OBJETIVO");
            }
        }


        if (pickupEffect != null)
        {
            Instantiate(
                pickupEffect,
                transform.position,
                Quaternion.identity
            );
        }

        Destroy(gameObject);
    }
}
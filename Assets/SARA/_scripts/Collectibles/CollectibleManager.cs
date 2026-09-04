using System.Collections.Generic;
using UnityEngine;

public class CollectibleManager : MonoBehaviour
{
    public static CollectibleManager instance;

    private List<CollectibleData> collectedItems = new List<CollectibleData>();

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

    public void AddCollectible(CollectibleData data)
    {
        if (!collectedItems.Contains(data))
        {
            collectedItems.Add(data);

            Debug.Log("Objeto agregado: " + data.ItemName);

            if (ObjectiveManager.instance != null)
            {
                ObjectiveManager.instance.CheckObjectives();
            }
        }
    }

    public bool HasCollectible(CollectibleData data)
    {
        return collectedItems.Contains(data);
    }

    public List<CollectibleData> GetCollectedItems()
    {
        return collectedItems;
    }
}
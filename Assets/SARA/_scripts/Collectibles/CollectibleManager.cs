using System.Collections.Generic;
using UnityEngine;

public class CollectibleManager : MonoBehaviour
{
    public static CollectibleManager instance;

    private Dictionary<CollectibleData, int> collectedItems = new Dictionary<CollectibleData, int>();

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
        if (!collectedItems.ContainsKey(data))
        {
            collectedItems.Add(data, 0);
        }

        collectedItems[data]++;

        Debug.Log(
            "Objeto recogido: " + data.ItemName +
            " (" + collectedItems[data] + "/" + data.RequiredAmount + ")"
        );

        if (ObjectiveManager.instance != null)
        {
            ObjectiveManager.instance.CheckObjectives();
        }
    }

    public int GetCollectedAmount(CollectibleData data)
    {
        if (collectedItems.ContainsKey(data))
        {
            return collectedItems[data];
        }

        return 0;
    }

    public bool HasCollectible(CollectibleData data)
    {
        return GetCollectedAmount(data) >= data.RequiredAmount;
    }
}
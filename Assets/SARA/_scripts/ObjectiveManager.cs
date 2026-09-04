using System.Collections.Generic;
using UnityEngine;

public class ObjectiveManager : MonoBehaviour
{
    public static ObjectiveManager instance;

    [Header("Objetos necesarios para este nivel")]
    [SerializeField] private List<CollectibleData> objectives;

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
        CheckObjectives();
    }
    
    public List<CollectibleData> GetObjectives()
    {
        return objectives;
    }

    public bool IsCollected(CollectibleData data)
    {
        return CollectibleManager.instance.HasCollectible(data);
    }

    public void CheckObjectives()
    {
        foreach (CollectibleData objective in objectives)
        {
            if (IsCollected(objective))
            {
                Debug.Log(" Recogido: " + objective.ItemName);
            }
            else
            {
                Debug.Log(" Falta: " + objective.ItemName);
            }
        }
    }
}

using System.Collections.Generic;
using UnityEngine;

public class ObjectiveManager : MonoBehaviour
{
    public static ObjectiveManager instance;

    [Header("Objetos necesarios para este nivel")]
    [SerializeField] private List<CollectibleData> objectives;

    [Header("HUD")]
    [SerializeField] private Transform objectiveContainer;
    [SerializeField] private GameObject objectiveItemPrefab;

    private List<ObjectiveUIItem> uiItems = new List<ObjectiveUIItem>();

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
        CreateObjectiveUI();
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

    public bool AllObjectivesCollected()
    {
        foreach (CollectibleData objective in objectives)
        {
            if (!IsCollected(objective))
            {
                return false;
            }
        }

        return true;
    }


    public void CheckObjectives()
    {
        for (int i = 0; i < objectives.Count; i++)
        {
            CollectibleData objective = objectives[i];

            // Consultar cuantas unidades se han recogido
            int collectedAmount =
                CollectibleManager.instance.GetCollectedAmount(objective);

            // Comprobar si se completo el objetivo
            bool collected =
                collectedAmount >= objective.RequiredAmount;

            if (collected)
            {
                Debug.Log("Recogido: " + objective.ItemName);
            }
            else
            {
                Debug.Log("Pendiente: " + objective.ItemName);
            }

            // Actualizar la casilla del HUD
            if (i < uiItems.Count)
            {
                uiItems[i].Setup(objective, collectedAmount);
            }
        }
    }


    private void CreateObjectiveUI()
    {
        foreach (CollectibleData objective in objectives)
        {
            GameObject newItem = Instantiate(
                objectiveItemPrefab,
                objectiveContainer
            );

            ObjectiveUIItem uiItem = newItem.GetComponent<ObjectiveUIItem>();

            uiItems.Add(uiItem);
        }

    }



}
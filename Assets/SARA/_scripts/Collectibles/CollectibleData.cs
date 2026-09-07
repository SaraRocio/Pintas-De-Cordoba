using UnityEngine;

[CreateAssetMenu(fileName = "NewCollectibleData", menuName = "PInstas/Collectible Data")]
public class CollectibleData : ScriptableObject
{
    [Header("Información del objeto")]
    [SerializeField] private string itemName;
    [SerializeField] private CollectibleType type;


    [Header("Objeto final del nivel")]
    [SerializeField] private bool isFinalObjective;


    [Header("UI")]
    [SerializeField] private Sprite icon;

    public string ItemName => itemName;
    public CollectibleType Type => type;
    public bool IsFinalObjective => isFinalObjective;
    public Sprite Icon => icon;
}
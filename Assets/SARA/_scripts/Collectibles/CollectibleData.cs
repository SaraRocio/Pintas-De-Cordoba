using UnityEngine;

[CreateAssetMenu(fileName = "NewCollectibleData", menuName = "PInstas/Collectible Data")]
public class CollectibleData : ScriptableObject
{
    [Header("Información del objeto")]
    [SerializeField] private string itemName;
    [SerializeField] private CollectibleType type;

    [Header("UI")]
    [SerializeField] private Sprite icon;

    public string ItemName => itemName;
    public CollectibleType Type => type;
    public Sprite Icon => icon;
}
using UnityEngine;
using UnityEngine.UI;

public class ObjectiveUIItem : MonoBehaviour
{
    [SerializeField] private Image icon;
    [SerializeField] private GameObject checkMark;

    public void Setup(CollectibleData data, bool collected)
    {
        icon.sprite = data.Icon;
        checkMark.SetActive(collected);
    }
}

using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ObjectiveUIItem : MonoBehaviour
{
    [Header("Icono del objeto")]
    [SerializeField] private Image icon;

    [Header("Estado de recoleccion")]
    [SerializeField] private GameObject checkMark;

    [Header("Cantidad pendiente")]
    [SerializeField] private TMP_Text amountText;

    public void Setup(CollectibleData data, int collectedAmount)
    {
        // Mostrar el icono del objeto
        icon.sprite = data.Icon;

        // Calcular cuantas unidades faltan
        int remainingAmount = Mathf.Max(
            0,
            data.RequiredAmount - collectedAmount
        );

        // Mostrar la cantidad solo si se necesitan
        // varias unidades y aun quedan por recoger
        bool showAmount =
            data.RequiredAmount > 1 &&
            remainingAmount > 0;

        amountText.gameObject.SetActive(showAmount);

        if (showAmount)
        {
            amountText.text = "x" + remainingAmount;
        }

        // Mostrar el check cuando se completa el objetivo
        bool completed = collectedAmount >= data.RequiredAmount;
        checkMark.SetActive(completed);
    }
}

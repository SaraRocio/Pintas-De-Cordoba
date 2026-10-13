using UnityEngine;

public class TutorialPintas : MonoBehaviour
{
    public GameObject tutorialPanel;

    void Start()
    {
        tutorialPanel.SetActive(true);
    }

    public void Continuar()
    {
        tutorialPanel.SetActive(false);
    }
}
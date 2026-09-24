using System.Collections;
using UnityEngine;
using TMPro;

public class LevelStartManager : MonoBehaviour
{
    [Header("Tutorial")]
    [SerializeField] private GameObject tutorialPanel;

    [Header("Cuenta regresiva")]
    [SerializeField] private GameObject countdownPanel;
    [SerializeField] private TMP_Text countdownText;

    [Header("Configuración")]
    [SerializeField] private float numberTime = 1f;

    private bool tutorialActive = true;

    private void Start()
    {
        Time.timeScale = 0f;

        tutorialPanel.SetActive(true);
        countdownPanel.SetActive(false);
    }

    public void ContinueFromTutorial()
    {
        if (!tutorialActive)
            return;

        tutorialActive = false;

        tutorialPanel.SetActive(false);

        StartCoroutine(Countdown());
    }

    private IEnumerator Countdown()
    {
        countdownPanel.SetActive(true);

        countdownText.text = "3";
        yield return new WaitForSecondsRealtime(numberTime);

        countdownText.text = "2";
        yield return new WaitForSecondsRealtime(numberTime);

        countdownText.text = "1";
        yield return new WaitForSecondsRealtime(numberTime);

        countdownText.text = "¡YA!";
        yield return new WaitForSecondsRealtime(numberTime);

        countdownPanel.SetActive(false);

        Time.timeScale = 1f;
    }
}
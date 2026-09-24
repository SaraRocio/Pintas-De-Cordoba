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

    [Header("Player - SOLO activar si este nivel lo necesita")]
    [SerializeField] private GameObject player;
    [SerializeField] private bool bloquearPlayer = false;

    [Header("Configuración")]
    [SerializeField] private float numberTime = 1f;

    private bool tutorialActive = true;

    private void Start()
    {
        Time.timeScale = 0f;

        tutorialPanel.SetActive(true);
        countdownPanel.SetActive(false);

        // Solo bloquea el Player en los niveles donde lo actives
        if (bloquearPlayer && player != null)
        {
            player.SetActive(false);
        }
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

        // Solo reactiva el Player si lo habíamos bloqueado
        if (bloquearPlayer && player != null)
        {
            player.SetActive(true);
        }

        Time.timeScale = 1f;
    }
}


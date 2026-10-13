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

    [Header("Jugador")]
    [SerializeField] private PlayerRunner playerRunner;

    [Header("Configuración")]
    [SerializeField] private float numberTime = 1f;

    private bool tutorialActive = true;

    private void Start()
    {
        // La partida comienza pausada.
        Time.timeScale = 0f;

        // Deiva espera mientras se muestra el tutorial.
        if (playerRunner != null)
            playerRunner.StopRunning();

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

        // Reactivar el tiempo del juego.
        Time.timeScale = 1f;

        // ¡Ahora sí, Deiva empieza a correr!
        if (playerRunner != null)
            playerRunner.StartRunning();
        else
            Debug.LogError("Falta asignar PlayerRunner en LevelStartManager.");
    }
}
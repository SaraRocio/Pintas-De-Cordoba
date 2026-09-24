using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelectButton : MonoBehaviour
{
    [Header("Configuración del nivel")]
    [SerializeField] private int levelNumber;
    [SerializeField] private string sceneName;

    [Header("Modelos")]
    [SerializeField] private GameObject activatedModel;
    [SerializeField] private GameObject deactivatedModel;

    [Header("Paneles")]
    [SerializeField] private GameObject availablePanel;
    [SerializeField] private GameObject lockedPanel;

    [Header("Tiempo")]
    [SerializeField] private float panelTime = 3f;

    private bool isWaiting = false;

    private void Start()
    {
        UpdateLevelVisual();

        availablePanel.SetActive(false);
        lockedPanel.SetActive(false);
    }

    private void UpdateLevelVisual()
    {
        bool unlocked = LevelUnlockManager.instance.IsLevelUnlocked(levelNumber);

        activatedModel.SetActive(unlocked);
        deactivatedModel.SetActive(!unlocked);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        if (isWaiting)
            return;

        StartCoroutine(LevelInteraction());
    }

    private IEnumerator LevelInteraction()
    {
        isWaiting = true;

        bool unlocked = LevelUnlockManager.instance.IsLevelUnlocked(levelNumber);

        if (unlocked)
        {
            availablePanel.SetActive(true);

            Debug.Log("PANEL DISPONIBLE ACTIVADO: " + availablePanel.name);
            Debug.Log("¿Está activo?: " + availablePanel.activeSelf);
            Debug.Log("¿Está activo en jerarquía?: " + availablePanel.activeInHierarchy);

            yield return new WaitForSeconds(panelTime);

            SceneManager.LoadScene(sceneName);
        }
        else   
        {
            lockedPanel.SetActive(true);

            Debug.Log("PANEL BLOQUEADO ACTIVADO");

            availablePanel.SetActive(false);

            yield return new WaitForSeconds(panelTime);

            lockedPanel.SetActive(false);

            isWaiting = false;
        }
    }
}
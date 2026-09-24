using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelCompletionManager : MonoBehaviour
{
    public static LevelCompletionManager instance;

    [Header("Panel de resultado")]
    [SerializeField] private GameObject resultPanel;

    [Header("Paneles")]
    [SerializeField] private GameObject successPanel;
    [SerializeField] private GameObject failurePanel;

    [Header("Siguiente nivel")]
    [SerializeField] private int currentLevel;

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
        resultPanel.SetActive(false);
    }

    public void CheckLevelCompletion()
    {
        resultPanel.SetActive(true);

        if (ObjectiveManager.instance.AllObjectivesCollected())
        {
            ShowSuccess();
        }
        else
        {
            ShowFailure();
        }
    }

    private void ShowSuccess()
    {
        successPanel.SetActive(true);
        failurePanel.SetActive(false);

        Debug.Log("NIVEL COMPLETADO");
    }

    private void ShowFailure()
    {
        successPanel.SetActive(false);
        failurePanel.SetActive(true);

        Debug.Log("NIVEL FALLIDO");
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(
            SceneManager.GetActiveScene().buildIndex
        );
    }


public void ContinueLevel()
    {
        // Si es el Nivel 4, ir directamente a la escena final
        if (currentLevel == 4)
        {
            SceneManager.LoadScene("Final");
            return;
        }

        // Para los niveles anteriores
        int nextUnlockedLevel = currentLevel + 1;

        int unlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", 1);

        if (nextUnlockedLevel > unlockedLevel)
        {
            PlayerPrefs.SetInt("UnlockedLevel", nextUnlockedLevel);
        }

        PlayerPrefs.Save();

        SceneManager.LoadScene("PruebaPintas");
    }
}
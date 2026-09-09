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
    [SerializeField] private string nextLevel;

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
        // Guardar el siguiente nivel
        PlayerPrefs.SetString("CurrentLevel", nextLevel);

        PlayerPrefs.Save();

        // Cargar siguiente nivel
        SceneManager.LoadScene(nextLevel);
    }
}
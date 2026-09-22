using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("Escenas")]
    [SerializeField] private string levelSelectScene;

    [Header("Botones")]
    [SerializeField] private GameObject continueButton;

    private void Start()
    {
        CheckContinueButton();
    }

    private void CheckContinueButton()
    {
        if (PlayerPrefs.HasKey("UnlockedLevel"))
        {
            continueButton.SetActive(true);
        }
        else
        {
            continueButton.SetActive(false);
        }
    }

    public void NewGame()
    {
        // Borra todo el progreso anterior
        PlayerPrefs.DeleteAll();

        // El primer nivel queda desbloqueado
        PlayerPrefs.SetInt("UnlockedLevel", 1);

        PlayerPrefs.Save();

        // Va al selector de niveles
        SceneManager.LoadScene(levelSelectScene);
    }

    public void ContinueGame()
    {
        // Conserva todo el progreso
        SceneManager.LoadScene(levelSelectScene);
    }

    public void QuitGame()
    {
        Debug.Log("Salir del juego");

        Application.Quit();
    }
}
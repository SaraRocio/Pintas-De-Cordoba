using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("Escenas")]
    [SerializeField] private string firstLevel;

    [Header("Botones")]
    [SerializeField] private GameObject continueButton;

    private void Start()
    {
        CheckContinueButton();
    }

    private void CheckContinueButton()
    {
        if (PlayerPrefs.HasKey("CurrentLevel"))
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
        // Borrar progreso anterior
        PlayerPrefs.DeleteAll();

        // Guardar el primer nivel
        PlayerPrefs.SetString("CurrentLevel", firstLevel);

        PlayerPrefs.Save();

        // Iniciar juego
        SceneManager.LoadScene(firstLevel);
    }

    public void ContinueGame()
    {
        if (PlayerPrefs.HasKey("CurrentLevel"))
        {
            string savedLevel = PlayerPrefs.GetString("CurrentLevel");

            SceneManager.LoadScene(savedLevel);
        }
    }

    public void QuitGame()
    {
        Debug.Log("Salir del juego");

        Application.Quit();
    }
}
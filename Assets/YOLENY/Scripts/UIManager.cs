using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("Panel de Felicidades")]
    public GameObject panelFelicidades;

    void Awake()
    {
        Instance = this;

        // Ocultar el panel al comenzar
        if (panelFelicidades != null)
        {
            panelFelicidades.SetActive(false);
        }
    }

    // Mostrar panel de felicidades
    public void MostrarFelicidades()
    {
        if (panelFelicidades != null)
        {
            panelFelicidades.SetActive(true);
        }
    }

    // Salir del juego
    public void SalirDelJuego()
    {
        Debug.Log("Saliendo del juego...");

        Application.Quit();
    }
}


using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Fuentes de audio")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource footstepsSource;

    [Header("Musicas")]
    [SerializeField] private AudioClip musicaMenuSelector;
    [SerializeField] private AudioClip musicaNivel1;
    [SerializeField] private AudioClip musicaNivel2;
    [SerializeField] private AudioClip musicaNivel3;
    [SerializeField] private AudioClip musicaNivel4;
    [SerializeField] private AudioClip musicaRompecabezas;

    [Header("Nombres exactos de las escenas")]
    [SerializeField] private string escenaMenu = "MenuPrincipal";
    [SerializeField] private string escenaSelector = "SelectorNiveles";
    [SerializeField] private string escenaNivel1 = "Nivel1";
    [SerializeField] private string escenaNivel2 = "Nivel2";
    [SerializeField] private string escenaNivel3 = "Nivel3";
    [SerializeField] private string escenaNivel4 = "Nivel4";
    [SerializeField] private string escenaRompecabezas = "Rompecabezas";

    [Header("Efectos de sonido")]
    [SerializeField] private AudioClip sonidoRecolectar;
    [SerializeField] private AudioClip sonidoSaltar;
    [SerializeField] private AudioClip sonidoAterrizar;
    [SerializeField] private AudioClip sonidoCorrer;
    [SerializeField] private AudioClip sonidoBoton;
    [SerializeField] private AudioClip sonidoPiezaRompecabezas;
    [SerializeField] private AudioClip sonidoObjetivoCompletado;
    [SerializeField] private AudioClip sonidoNivelDesbloqueado;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        if (musicSource == null || sfxSource == null ||
            footstepsSource == null)
        {
            Debug.LogError(
                "AudioManager: asigna las tres fuentes de audio en el Inspector."
            );
            enabled = false;
            return;
        }

        musicSource.loop = true;
        footstepsSource.loop = true;

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Start()
    {
        CambiarMusica(SceneManager.GetActiveScene().name);
    }

    private void OnDestroy()
    {
        if (Instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            Instance = null;
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        CambiarMusica(scene.name);
        DetenerCarrera();
    }

    private void CambiarMusica(string nombreEscena)
    {
        AudioClip musica = null;

        if (nombreEscena == escenaMenu ||
            nombreEscena == escenaSelector)
        {
            musica = musicaMenuSelector;
        }
        else if (nombreEscena == escenaNivel1)
        {
            musica = musicaNivel1;
        }
        else if (nombreEscena == escenaNivel2)
        {
            musica = musicaNivel2;
        }
        else if (nombreEscena == escenaNivel3)
        {
            musica = musicaNivel3;
        }
        else if (nombreEscena == escenaNivel4)
        {
            musica = musicaNivel4;
        }
        else if (nombreEscena == escenaRompecabezas)
        {
            musica = musicaRompecabezas;
        }

        if (musica == null)
        {
            musicSource.Stop();
            musicSource.clip = null;

            Debug.LogWarning(
                "AudioManager: no hay musica asignada para la escena: "
                + nombreEscena
            );
            return;
        }

        // Si es la misma canción, continúa sin reiniciarse.
        if (musicSource.clip == musica && musicSource.isPlaying)
            return;

        musicSource.Stop();
        musicSource.clip = musica;
        musicSource.Play();
    }

    public void ReproducirRecolectar()
    {
        ReproducirEfecto(sonidoRecolectar);
    }

    public void ReproducirSalto()
    {
        ReproducirEfecto(sonidoSaltar);
    }

    public void ReproducirAterrizaje()
    {
        ReproducirEfecto(sonidoAterrizar);
    }

    public void ReproducirBoton()
    {
        ReproducirEfecto(sonidoBoton);
    }

    public void ReproducirPiezaRompecabezas()
    {
        ReproducirEfecto(sonidoPiezaRompecabezas);
    }

    public void ReproducirObjetivoCompletado()
    {
        ReproducirEfecto(sonidoObjetivoCompletado);
    }

    public void ReproducirNivelDesbloqueado()
    {
        ReproducirEfecto(sonidoNivelDesbloqueado);
    }

    private void ReproducirEfecto(AudioClip clip)
    {
        if (clip != null && sfxSource != null)
        {
            sfxSource.PlayOneShot(clip);
        }
    }

    public void IniciarCarrera()
    {
        if (sonidoCorrer == null || footstepsSource == null)
            return;

        if (footstepsSource.clip != sonidoCorrer)
        {
            footstepsSource.Stop();
            footstepsSource.clip = sonidoCorrer;
        }

        if (!footstepsSource.isPlaying)
            footstepsSource.Play();
    }

    public void DetenerCarrera()
    {
        if (footstepsSource != null && footstepsSource.isPlaying)
            footstepsSource.Stop();
    }
}


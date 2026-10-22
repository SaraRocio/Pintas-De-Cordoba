using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Fuentes de audio")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("Musica")]
    public AudioClip menuMusic;
    public AudioClip levels13Music;
    public AudioClip levels24Music;
    public AudioClip puzzleMusic;

    [Header("Efectos")]
    public AudioClip buttonClick;
    public AudioClip jumpSFX;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        musicSource.loop = true;
        musicSource.playOnAwake = false;
        sfxSource.playOnAwake = false;
    }

    public void PlayMusic(AudioClip clip)
    {
        if (musicSource == null || clip == null)
            return;

        if (musicSource.clip == clip && musicSource.isPlaying)
            return;

        musicSource.clip = clip;
        musicSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        if (sfxSource == null || clip == null)
            return;

        sfxSource.PlayOneShot(clip);
    }

    public void PlayMenuMusic()
    {
        PlayMusic(menuMusic);
    }

    public void PlayLevels13Music()
    {
        PlayMusic(levels13Music);
    }

    public void PlayLevels24Music()
    {
        PlayMusic(levels24Music);
    }

    public void PlayPuzzleMusic()
    {
        PlayMusic(puzzleMusic);
    }

    public void PlayButtonClick()
    {
        PlaySFX(buttonClick);
    }

    public void PlayJump()
    {
        PlaySFX(jumpSFX);
    }
}

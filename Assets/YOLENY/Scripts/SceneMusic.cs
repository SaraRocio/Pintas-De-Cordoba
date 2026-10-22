using UnityEngine;

public class SceneMusic : MonoBehaviour
{
    public enum MusicType
    {
        Menu,
        Levels13,
        Levels24,
        Puzzle
    }

    [SerializeField] private MusicType musicType;

    private void Start()
    {
        if (AudioManager.Instance == null)
            return;

        switch (musicType)
        {
            case MusicType.Menu:
                AudioManager.Instance.PlayMenuMusic();
                break;

            case MusicType.Levels13:
                AudioManager.Instance.PlayLevels13Music();
                break;

            case MusicType.Levels24:
                AudioManager.Instance.PlayLevels24Music();
                break;

            case MusicType.Puzzle:
                AudioManager.Instance.PlayPuzzleMusic();
                break;
        }
    }
}
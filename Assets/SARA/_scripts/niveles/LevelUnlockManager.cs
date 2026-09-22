using UnityEngine;

public class LevelUnlockManager : MonoBehaviour
{
    public static LevelUnlockManager instance;

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

    public void UnlockNextLevel(int currentLevel)
    {
        int nextLevel = currentLevel + 1;

        int unlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", 1);

        if (nextLevel > unlockedLevel)
        {
            PlayerPrefs.SetInt("UnlockedLevel", nextLevel);
            PlayerPrefs.Save();
        }
    }

    public bool IsLevelUnlocked(int level)
    {
        int unlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", 1);

        return level <= unlockedLevel;
    }
}
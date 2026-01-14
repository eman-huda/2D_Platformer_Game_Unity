using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    [System.Serializable]
    public class LevelInfo
    {
        public string levelName;
        public string fruitName;
        public Sprite fruitIcon;
        public int sceneIndex; // Scene build index
    }

    [Header("Level Setup")]
    public LevelInfo[] levels;
    private int currentLevel = 0;

    [Header("UI References")]
    public TMP_Text levelDetailsText;
    public Image fruitImage;
    public Button playButton;
    public TMP_Text playButtonText;

    private int unlockedLevel;

    private void Start()
    {
        if (!PlayerPrefs.HasKey("UnlockedLevel"))
        {
            PlayerPrefs.SetInt("UnlockedLevel", 1);
            PlayerPrefs.Save();
        }

        unlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", 1);

        if (unlockedLevel < 1)
            unlockedLevel = 1;

        UpdateLevelUI();
    }

    public void NextLevel()
    {
        currentLevel++;
        if (currentLevel >= levels.Length)
            currentLevel = 0;
        UpdateLevelUI();
    }

    public void PreviousLevel()
    {
        currentLevel--;
        if (currentLevel < 0)
            currentLevel = levels.Length - 1;
        UpdateLevelUI();
    }

    private void UpdateLevelUI()
    {
        LevelInfo level = levels[currentLevel];
        bool isUnlocked = (currentLevel + 1) <= unlockedLevel;

        levelDetailsText.text =
            $"Level {currentLevel + 1}\n" +
            $"{level.fruitName} — {level.levelName}\n" +
            (isUnlocked ? "<color=green>Unlocked</color>" : "<color=red>Locked</color>");

        if (fruitImage != null && level.fruitIcon != null)
            fruitImage.sprite = level.fruitIcon;

        playButton.interactable = isUnlocked;
        playButtonText.text = isUnlocked ? "PLAY" : "LOCKED";

        // Remove old listeners before adding new ones
        playButton.onClick.RemoveAllListeners();

        if (isUnlocked)
        {
            int index = level.sceneIndex;
            playButton.onClick.AddListener(() => LoadLevel(index));
        }
    }

    private void LoadLevel(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }

    public void OnPlayButton()
    {
        int unlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", 1);

        if (currentLevel + 1 <= unlockedLevel)
        {
            SceneManager.LoadScene(levels[currentLevel].sceneIndex);
        }
        else
        {
            Debug.Log("Level is locked!");
            playButtonText.text = "Level Locked!";
        }
    }
    public void UnlockNextLevel()
    {
        int unlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", 1);
        int currentScene = SceneManager.GetActiveScene().buildIndex;

        if (currentScene >= unlockedLevel)
        {
            PlayerPrefs.SetInt("UnlockedLevel", currentScene + 1);
            PlayerPrefs.Save();
            Debug.Log("Unlocked Level " + (currentScene + 1));
        }
    }
}

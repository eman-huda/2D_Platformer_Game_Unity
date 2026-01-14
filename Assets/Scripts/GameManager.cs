using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public TMP_Text scoreText;
    private int totalScore = 0;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void AddScore(int value)
    {
        totalScore += value;
        UpdateScoreUI();
        
    }
    public void SaveHighScore()
    {
        int highestScore = PlayerPrefs.GetInt("Highest Score", 0);
        if (totalScore > highestScore)
        {
            PlayerPrefs.SetInt("Highest Score", totalScore);
            PlayerPrefs.Save();
        }
    }
    public void UpdateCollected()
    {
        int collectedFruit = PlayerPrefs.GetInt("Collected Fruit", 0);
            PlayerPrefs.SetInt("Collected Fruit", totalScore+ collectedFruit);
            PlayerPrefs.Save();
        
    }
    private void UpdateScoreUI()
    {
        if (scoreText != null)
            scoreText.text = "Score: " + totalScore;
    }

    public int GetScore()
    {
        return totalScore;
    }

    public  void LevelSelector_UnlockNextLevel()
    {
        int currentUnlocked = PlayerPrefs.GetInt("UnlockedLevel", 1);
        int currentLevel = SceneManager.GetActiveScene().buildIndex;
        if (currentLevel >= currentUnlocked)
        {
            PlayerPrefs.SetInt("UnlockedLevel", currentLevel + 1);
            PlayerPrefs.Save();
        }
    }
}

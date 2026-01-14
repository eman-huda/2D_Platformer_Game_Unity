using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScreen : MonoBehaviour
{
    public GameObject[] panel;

    public TMP_Text highscoreText;
    public TMP_Text TotalfruitText;
    public void SwitchScene(int sceneNo)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneNo);
    }
    public void SelfSwitch(int sceneNo)
    {
        SceneManager.LoadScene(sceneNo);
        foreach (GameObject go in panel)
        {
            go.SetActive(false);
        }
        Time.timeScale = 1f;
    }
    private void Start()
    {
        
        int highestScore = PlayerPrefs.GetInt("Highest Score");
        int fruitCollected = PlayerPrefs.GetInt("Collected Fruit");
        Debug.Log(highestScore);
        if (highscoreText != null)
            highscoreText.text = "HIGHEST SCORE: " + highestScore;
        if (TotalfruitText != null)
            TotalfruitText.text = "FRUIT COLLECTED: " + fruitCollected;
    }
}



using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterSelection : MonoBehaviour
{
    [Header("Characters")]
    public GameObject[] charactersArray;           // your existing prefabs / display objects
    public int currentCharacterIndex = 0;
    public int previousCharacterIndex = 0;
    public static int selectedCharacterIndex = 0;

    [Header("Unlock Data")]
    public int[] unlockCost;                       // set size = charactersArray.Length in Inspector
    public bool[] unlockedByDefault;               // set which characters are unlocked at start

    [Header("UI")]
    public Button selectButton;                    // the main button
    public TMP_Text selectButtonText;              // text on the main button
    public TMP_Text fruitText;                     // display total fruit
    public GameObject padlockOverlay;              // optional padlock image (or use per-character overlay)

    private void Awake()
    {
        // initialize default unlocks in PlayerPrefs (only first run)
        for (int i = 0; i < charactersArray.Length; i++)
        {
            string key = "char_unlocked_" + i;
            if (!PlayerPrefs.HasKey(key))
            {
                PlayerPrefs.SetInt(key, unlockedByDefault != null && i < unlockedByDefault.Length && unlockedByDefault[i] ? 1 : 0);
            }
        }

        // ensure selectedCharacterIndex default set
        selectedCharacterIndex = PlayerPrefs.GetInt("SelectedCharacterIndex", 0);
        currentCharacterIndex = selectedCharacterIndex;
        previousCharacterIndex = currentCharacterIndex;
    }

    private void Start()
    {
        //ResetCharacterUnlocks();
        UpdateFruitUI();
        UpdateCharacterDisplay(); // show current character on start
    }
    public void ResetCharacterUnlocks()
    {
        for (int i = 0; i < charactersArray.Length; i++)
        {
            string key = "char_unlocked_" + i;

            // Restore default unlock state
            bool defaultState = (unlockedByDefault != null && i < unlockedByDefault.Length && unlockedByDefault[i]);
            PlayerPrefs.SetInt(key, defaultState ? 1 : 0);
        }

        // Reset selected character to default (usually index 0)
        PlayerPrefs.SetInt("SelectedCharacterIndex", 0);

        PlayerPrefs.SetInt("Collected Fruit", 10);

        PlayerPrefs.Save();

        Debug.Log("Character unlocks reset and fruit set to 10.");

        UpdateFruitUI();
        UpdateCharacterDisplay();
    }


    // button
    public void OnSelectButton()
    {
        if (IsUnlocked(currentCharacterIndex))
        {
            selectedCharacterIndex = currentCharacterIndex;
            PlayerPrefs.SetInt("SelectedCharacterIndex", selectedCharacterIndex);
            PlayerPrefs.Save();
            Debug.Log("Selected character: " + selectedCharacterIndex);
            
            SceneManager.LoadScene(7);
        }
        else
        {
            TryUnlockCurrent();
        }

        UpdateCharacterDisplay();
    }

    public void leftArrow()
    {
        previousCharacterIndex = currentCharacterIndex;
        currentCharacterIndex--;
        if (currentCharacterIndex < 0)
        {
            currentCharacterIndex = charactersArray.Length - 1;
        }
        UpdateCharacterDisplay();
    }

    public void rightArrow()
    {
        previousCharacterIndex = currentCharacterIndex;
        currentCharacterIndex++;
        if (currentCharacterIndex >= charactersArray.Length)
        {
            currentCharacterIndex = 0;
        }
        UpdateCharacterDisplay();
    }

    void UpdateCharacterDisplay()
    {
        // enable only the current character for preview
        for (int i = 0; i < charactersArray.Length; i++)
        {
            charactersArray[i].SetActive(i == currentCharacterIndex);
        }

        // update button text depending on lock state
        if (IsUnlocked(currentCharacterIndex))
        {
            selectButtonText.text = "Select";
            if (padlockOverlay != null) padlockOverlay.SetActive(false);
        }
        else
        {
            int cost = GetCost(currentCharacterIndex);
            selectButtonText.text = "Unlock (" + cost + ")";
            if (padlockOverlay != null) padlockOverlay.SetActive(true);
        }
    }

    bool IsUnlocked(int index)
    {
        string key = "char_unlocked_" + index;
        return PlayerPrefs.GetInt(key, 0) == 1;
    }

    int GetCost(int index)
    {
        if (unlockCost != null && index < unlockCost.Length) return unlockCost[index];
        return 0;
    }

    void TryUnlockCurrent()
    {
        int cost = GetCost(currentCharacterIndex);
        int fruit = PlayerPrefs.GetInt("Collected Fruit", 0);

        if (fruit >= cost)
        {
            fruit -= cost;
            PlayerPrefs.SetInt("Collected Fruit", fruit);
            PlayerPrefs.SetInt("char_unlocked_" + currentCharacterIndex, 1);
            PlayerPrefs.Save();

            UpdateFruitUI();
            Debug.Log("Unlocked character index: " + currentCharacterIndex);
        }
        else
        {
            Debug.Log("Not enough fruit to unlock.");
        }
    }

    void UpdateFruitUI()
    {
        int fruit = PlayerPrefs.GetInt("Collected Fruit", 0);
        if (fruitText != null) fruitText.text = "FRUIT COLLECTED: " + fruit;
    }
}


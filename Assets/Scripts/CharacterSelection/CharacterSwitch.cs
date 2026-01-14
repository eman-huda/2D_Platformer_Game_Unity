//using System.Collections;
//using System.Collections.Generic;
//using UnityEditor;
//using UnityEngine;

//public class CharacterSwitch : MonoBehaviour
//{
//    public GameObject[] charactersArray;
//    void Start()
//    {
//        switchCharacter();
//    }
//    public void switchCharacter()
//    {
//        for (int i = 0; i < charactersArray.Length; i++)
//        {
//            GameObject character = charactersArray[i];

//            if (i == CharacterSelection.selectedCharacterIndex)
//            {
//                character.SetActive(true);
//                foreach (Transform child in character.transform)
//                {
//                    child.gameObject.SetActive(true);
//                }
//            }
//            else
//            {
//                foreach (Transform child in character.transform)
//                {
//                    child.gameObject.SetActive(false);
//                }
//                character.SetActive(false);
//            }
//        }

//    }

//}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSwitch : MonoBehaviour
{
    public GameObject[] charactersArray;

    void Start()
    {
        int selectedIndex = PlayerPrefs.GetInt("SelectedCharacterIndex", 0);
        SwitchCharacter(selectedIndex);
    }

    public void SwitchCharacter(int selectedIndex)
    {
        for (int i = 0; i < charactersArray.Length; i++)
        {
            GameObject character = charactersArray[i];
            bool active = (i == selectedIndex);
            character.SetActive(active);
            // ensure children state consistent:
            foreach (Transform child in character.transform)
            {
                child.gameObject.SetActive(active);
            }
        }
    }
}

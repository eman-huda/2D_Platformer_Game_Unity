using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    [SerializeField] private Health[] playerHealthArray;  
    [SerializeField] private Image totalHealthBar;
    [SerializeField] private Image currentHealthBar;

    private Health activePlayerHealth;

    private void Start()
    {
        FindActivePlayer();
        if (activePlayerHealth != null)
            totalHealthBar.fillAmount = activePlayerHealth.currentHealth / 10;
    }

    private void Update()
    {
        // Continuously check which player is active (in case you switch)
        FindActivePlayer();

        if (activePlayerHealth != null)
        {
            currentHealthBar.fillAmount = activePlayerHealth.currentHealth / 10;
        }
    }

    private void FindActivePlayer()
    {
        foreach (Health h in playerHealthArray)
        {
            if (h != null && h.gameObject.activeInHierarchy)
            {
                activePlayerHealth = h;
                return;
            }
        }
        activePlayerHealth = null;
    }
}

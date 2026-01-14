using UnityEngine;

public class InputManagerUI : MonoBehaviour
{
    private PlayerMovement activePlayer;
    private playerAttack playerattack;
    void Update()
    {
        // Keep checking which player is currently active
        if (activePlayer == null)
        {
            activePlayer = FindActivePlayer();
            playerattack = FindActivePlayer2();
        }
            
    }

    private PlayerMovement FindActivePlayer()
    {
        // This assumes each player has a PlayerMovement script
        PlayerMovement[] players = FindObjectsOfType<PlayerMovement>();
        foreach (var player in players)
        {
            if (player.gameObject.activeInHierarchy)
                return player;
        }
        return null;
    }
    private playerAttack FindActivePlayer2()
    {
        // This assumes each player has a PlayerMovement script
        playerAttack[] players = FindObjectsOfType<playerAttack>();
        foreach (var player in players)
        {
            if (player.gameObject.activeInHierarchy)
                return player;
        }
        return null;
    }

    // ----------------
    // Button Functions
    // ----------------

    public void OnJumpButton()
    {
        if (activePlayer != null)
            activePlayer.OnJumpButton();
    }

    public void OnCrouchDown()
    {
        if (activePlayer != null)
            activePlayer.OnCrouchButtonDown();
    }
    public void OnCrouchUp()
    {
        if (activePlayer != null)
            activePlayer.OnCrouchButtonUp();
    }


    public void OnFireButton()
    {
        if (playerattack != null)
            playerattack.attackUI();
    }
}

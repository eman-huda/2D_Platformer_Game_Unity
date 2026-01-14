using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [Header("Player Settings")]
    public GameObject[] targetPlayers; // array of all player prefabs
    private GameObject activePlayer;   // currently active player

    [Header("Camera Settings")]
    public float lookaheadDistance = 1f;
    public float cameraSmoothSpeed = 0.15f;
    private Vector3 velocity = Vector3.zero;

    void Start()
    {
        FindActivePlayer();
    }

    void FixedUpdate()
    {
        FindActivePlayer();
        if (activePlayer == null)
        {
            FindActivePlayer(); // In case player changes or respawns
            return;
        }

        Vector3 targetPosition = new Vector3(
            activePlayer.transform.position.x + lookaheadDistance,
            transform.position.y,
            transform.position.z
        );

        transform.position = Vector3.SmoothDamp(
            transform.position,
            targetPosition,
            ref velocity,
            cameraSmoothSpeed
        );
    }

    // Finds the currently active player
    void FindActivePlayer()
    {
        foreach (GameObject player in targetPlayers)
        {
            if (player.activeInHierarchy)
            {
                activePlayer = player;
                Debug.Log("Camera following: " + player.name);
                return;
            }
        }
        Debug.LogWarning("No active player found!");
    }
}

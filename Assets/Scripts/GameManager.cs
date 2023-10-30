using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [HideInInspector] public int score;
    [HideInInspector] public bool gamePaused;

    public static GameManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        //Cancel = Esc
        if (Input.GetButtonDown("Cancel"))
        {
            UpdateGamePaused();
        }
    }

    private void UpdateGamePaused()
    {
        //Change pause state
        gamePaused = !gamePaused;
        // If paused freeze the game else unpause
        Time.timeScale = (gamePaused ? 0 : 1);
        // If game paused true activate the mouse, else deactivate
        Cursor.lockState = (gamePaused) ? CursorLockMode.None : CursorLockMode.Locked;

        //TODO: Hud controller call game pause window
    }
}

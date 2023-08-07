using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorBehaviour : MonoBehaviour
{
    void Start()
    {
        // Lock and hide the cursor at the start of the game
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // If you want to toggle cursor visibility with a specific key (e.g., escape key)
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleCursorVisibility();
        }
    }

    // Function to toggle cursor visibility on/off
    void ToggleCursorVisibility()
    {
        if (Cursor.lockState == CursorLockMode.Locked)
        {
            // If cursor is locked, unlock and make it visible
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            // If cursor is unlocked, lock and make it invisible
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}
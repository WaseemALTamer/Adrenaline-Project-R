using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CamEnable : NetworkBehaviour
{

    public Camera mainCamera; // Reference to the main camera in the scene

    void Start()
    {
        if (IsLocalPlayer)
        {
            mainCamera.gameObject.SetActive(true);
        }
        else
        {
            mainCamera.gameObject.SetActive(false);
        }
    }
}

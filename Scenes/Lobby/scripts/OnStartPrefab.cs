using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class OnStartPrefab : NetworkBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if(!IsOwner){
            gameObject.SetActive(false);
        }
    }
}

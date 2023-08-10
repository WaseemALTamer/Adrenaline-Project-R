using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using Unity.Netcode;
using UnityEngine;

public class SpawnPlayer : NetworkBehaviour
{
    public GameObject NetManager;

    private void Start(){
        NetManager = GameObject.Find("NetworkManager");
        NetManager.GetComponent<NetworkPrefabsList>();
        
    }
}

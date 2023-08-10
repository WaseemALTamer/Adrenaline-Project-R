using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class HostStarter : NetworkBehaviour
{

    public GameObject ObjectToSpawn;
    

    public void run(){
        SpawnServerRpc("0","0","0");
    }
    
    [ServerRpc]
    public void SpawnServerRpc(string name,string ip ,string port){
        gameObject.GetComponent<GameOnList>().StoreGame(name,ip,port);
        SpawnClientRPC(name,ip,port);
    }

    [ClientRpc]
    public void SpawnClientRPC(string name,string ip ,string port){
        GameObject Game = Instantiate(ObjectToSpawn);
        Game.GetComponent<ButtonSetup>().name = name;
        Game.GetComponent<ButtonSetup>().ip = ip;
        Game.GetComponent<ButtonSetup>().port = port;
    }
}

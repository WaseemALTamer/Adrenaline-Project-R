using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class OnJoin : NetworkBehaviour
{
    public GameObject ObjectToSpawn;
    public GameObject ScrollContent;
    public float TimerToUpdate;
    // Start is called before the first frame update
    void Start()
    {
        if (!IsOwner) return;
        if (IsOwnedByServer == true){
            return;
        }
        RequestServerRpc();
    }

    void FixedUpdate(){
        if (TimerToUpdate <= Time.fixedTime){
            RequestServerRpc();
            TimerToUpdate = Time.fixedTime + 3;
        }
    }

    [ServerRpc]
    public void SpawnServerRpc(string name,string ip ,string port){
        GameObject ListObject;
        ListObject = GameObject.Find("OnStart");
        ListObject.GetComponent<GameOnList>().StoreGame(name,ip,port);
        SpawnClientRPC(name,ip,port);
    }

    [ClientRpc]
    public void SpawnClientRPC(string name,string ip ,string port){
        GameObject Game = Instantiate(ObjectToSpawn);
        Game.GetComponent<ButtonSetup>().name = name;
        Game.GetComponent<ButtonSetup>().ip = ip;
        Game.GetComponent<ButtonSetup>().port = port;
        Game.transform.parent = ScrollContent.transform;
    }

    [ServerRpc]
    public void RequestServerRpc(){
        DestoryContentClientRPC(OwnerClientId);
        GameObject ListObject;
        ListObject = GameObject.Find("OnStart");
        for (int i = 0; i < ListObject.GetComponent<GameOnList>().GamesAavalibaleIP.Length;i++){
            if (ListObject.GetComponent<GameOnList>().GamesAavalibaleIP[i] != null){
                InitialiseClientRPC(OwnerClientId,$"{ListObject.GetComponent<GameOnList>().GamesAavalibaleIP[i]}",$"{ListObject.GetComponent<GameOnList>().GamesAavalibalePort[i]}",$"{ListObject.GetComponent<GameOnList>().GamesAavalibaleName[i]}");
            }
        }
    }
    [ClientRpc]
    public void InitialiseClientRPC(ulong targetClientId,string ip ,string port ,string name){
        if(!IsOwner) return;
        GameObject Game = Instantiate(ObjectToSpawn);
        Game.GetComponent<ButtonSetup>().name = name;
        Game.GetComponent<ButtonSetup>().ip = ip;
        Game.GetComponent<ButtonSetup>().port = port;
        Game.transform.parent = ScrollContent.transform;
    }
    [ClientRpc]
    public void DestoryContentClientRPC(ulong targetClientId){
        if(!IsOwner) return;
        int childCount = ScrollContent.transform.childCount;
        for (int i = childCount - 1; i >= 0; i--){
            Transform child = ScrollContent.transform.GetChild(i);
            Destroy(child.gameObject);
        }
    } 
}

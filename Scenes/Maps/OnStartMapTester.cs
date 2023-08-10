using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Unity.Netcode.Transports.UTP;
using Unity.Netcode;

public class OnStartMapTester : NetworkBehaviour
{
    public GameObject NewPrefab;
    private GameObject Network;
    private GameObject Information;

    void Start(){
        NetworkManager.Singleton.Shutdown();
        Information = GameObject.Find("Information");
        Network = GameObject.Find("NetworkManager");

        string PORT = Information.GetComponent<InfoBetweenSence>().port;
        string IP = Information.GetComponent<InfoBetweenSence>().Ip;

        NetworkManager.Singleton.NetworkConfig.PlayerPrefab = NewPrefab;

        Network.GetComponent<UnityTransport>().ConnectionData.Port = ushort.Parse(PORT);
        Network.GetComponent<UnityTransport>().ConnectionData.Address = IP;

        if (Information.GetComponent<InfoBetweenSence>().is_host == true)NetworkManager.Singleton.StartHost();
        if (Information.GetComponent<InfoBetweenSence>().is_client== true)NetworkManager.Singleton.StartClient();
        if (Information.GetComponent<InfoBetweenSence>().is_server == true)NetworkManager.Singleton.StartServer();
        
    }
}

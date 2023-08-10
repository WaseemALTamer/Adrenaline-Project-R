using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using Unity.Netcode;
using Unity.Networking.Transport;
using Unity.Netcode.Transports.UTP;
using UnityEngine.SceneManagement;
using System;
using System.Net;

public class NetworkManagerUI  : NetworkBehaviour
{

    public string sceneToLoad;
    private string PORT_local;
    private string IP_local;
    private GameObject Network;
    private GameObject DontDestory;
    public GameObject Hoster;

    void Start(){
        Network = GameObject.Find("NetworkManager");
        DontDestory = GameObject.Find("Information");
        IP_local = "127.0.0.1";
        PORT_local = "5005"; 
    }




    public void ButtonJoin(){
        if (PORT_local != null && IP_local != null)
        {
            Network.GetComponent<UnityTransport>().ConnectionData.Address = IP_local;
            Network.GetComponent<UnityTransport>().ConnectionData.Port = ushort.Parse(PORT_local);
            DontDestory.GetComponent<InfoBetweenSence>().is_client = true;
            DontDestory.GetComponent<InfoBetweenSence>().port = PORT_local;
            DontDestory.GetComponent<InfoBetweenSence>().Ip = Network.GetComponent<UnityTransport>().ConnectionData.Address;
            NetworkManager.Singleton.Shutdown();
            SceneManager.LoadScene(sceneToLoad);
        }
    }
    public void ButtonHost(){
        if (PORT_local != null){
            TextIPInsert(GrapIp());
            Network.GetComponent<UnityTransport>().ConnectionData.Port = ushort.Parse(PORT_local);
            DontDestory.GetComponent<InfoBetweenSence>().is_host = true;
            DontDestory.GetComponent<InfoBetweenSence>().port = PORT_local;
            DontDestory.GetComponent<InfoBetweenSence>().Ip = Network.GetComponent<UnityTransport>().ConnectionData.Address;
            Hoster.GetComponent<OnJoin>().SpawnServerRpc("Name",$"{Network.GetComponent<UnityTransport>().ConnectionData.Address}",$"{PORT_local}");
            NetworkManager.Singleton.Shutdown();
            SceneManager.LoadScene(sceneToLoad);
        }
    }


    public void TextIPInsert(string Text){
        IP_local = Text;
        Network.GetComponent<UnityTransport>().ConnectionData.Address = IP_local;
    }


    public void TextPortInsert(string Text){
        PORT_local = Text;
        Network.GetComponent<UnityTransport>().ConnectionData.Port = ushort.Parse(PORT_local);
    }
    public void ServerLaunch(){
        if (PORT_local != null){
            TextIPInsert(GrapIp());
            Network.GetComponent<UnityTransport>().ConnectionData.Port = ushort.Parse(PORT_local);
            DontDestory.GetComponent<InfoBetweenSence>().is_server = true;
            DontDestory.GetComponent<InfoBetweenSence>().port = PORT_local;
            DontDestory.GetComponent<InfoBetweenSence>().Ip = Network.GetComponent<UnityTransport>().ConnectionData.Address;
            Application.targetFrameRate = 128;
            Hoster.GetComponent<OnJoin>().SpawnServerRpc("Name",$"{Network.GetComponent<UnityTransport>().ConnectionData.Address}",$"{PORT_local}"); 
            NetworkManager.Singleton.Shutdown();
            SceneManager.LoadScene(sceneToLoad);
        }
    }

    public void SearchButton(){
        Network.GetComponent<UnityTransport>().ConnectionData.Port = ushort.Parse(PORT_local);
        Network.GetComponent<UnityTransport>().ConnectionData.Address = IP_local;
        NetworkManager.Singleton.StartClient();
    }

    private string GrapIp()
    {
        string hostName = Dns.GetHostName();
        IPAddress[] localIPAddresses = Dns.GetHostEntry(hostName).AddressList;

        // Find and print the first IPv4 address (excluding loopback addresses)
        foreach (IPAddress ipAddress in localIPAddresses)
        {
            if (ipAddress.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork
                && !IPAddress.IsLoopback(ipAddress))
            {
                return ipAddress.ToString();
            }
        }
        return "None";
    }
}

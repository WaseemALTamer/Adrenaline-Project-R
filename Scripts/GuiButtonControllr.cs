using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;

public class GuiButtonControllr : MonoBehaviour
{
    
    public void ServerButton(){
        NetworkManager.Singleton.StartServer();
    }
    public void HostButton(){
        NetworkManager.Singleton.StartHost();
    }
    public void ClientButton(){
        NetworkManager.Singleton.StartClient();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GuiButtonControllr : MonoBehaviour
{
    public GameObject GUI;

    public void ServerButton(){
        NetworkManager.Singleton.StartServer();
        GUI.SetActive(false);
    }
    public void HostButton(){
        NetworkManager.Singleton.StartHost();
        GUI.SetActive(false);
    }
    public void ClientButton(){
        NetworkManager.Singleton.StartClient();
        GUI.SetActive(false);
    }
    public void HostGame(){
        GameObject INFO = GameObject.Find("Information");
        INFO.GetComponent<InfoBetweenSence>().Ip = "127.0.0.1";
        INFO.GetComponent<InfoBetweenSence>().port = "5005";
        INFO.GetComponent<InfoBetweenSence>().is_host = true;
        SceneManager.LoadScene("MapTester");
    }
}

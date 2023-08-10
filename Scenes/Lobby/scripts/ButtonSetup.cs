using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.Netcode;
using TMPro;

public class ButtonSetup : NetworkBehaviour
{
    public TMP_Text ButtonText;

    public string Name;
    public string ip;
    public string port;
    public int GameNum;
    private GameObject DontDestory;

    public void Start(){
        ButtonText.text = $"{ip}";
    }

    public void ButtonFunction(){
        DontDestory = GameObject.Find("Information");
        DontDestory.GetComponent<InfoBetweenSence>().Ip = ip;
        DontDestory.GetComponent<InfoBetweenSence>().name = Name;
        DontDestory.GetComponent<InfoBetweenSence>().port = port;
        DontDestory.GetComponent<InfoBetweenSence>().is_client = true;
        NetworkManager.Singleton.Shutdown();
        SceneManager.LoadScene("MapTester");
    }
}
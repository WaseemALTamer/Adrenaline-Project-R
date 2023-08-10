using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class InfoBetweenSence : NetworkBehaviour
{
    public bool is_host;
    public bool is_client;
    public bool is_server;

    [Header("ConnectTo")]
    public string Ip;
    public string port;
    private void Update(){
        gameObject.name = "Information";
    }
}

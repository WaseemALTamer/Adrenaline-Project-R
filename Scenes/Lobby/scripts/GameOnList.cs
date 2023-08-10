using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOnList : MonoBehaviour
{
    
    public string[] GamesAavalibaleIP;
    public string[] GamesAavalibalePort;
    public string[] GamesAavalibaleName;
    public int NumberOfGames;
    private void Start(){
        GamesAavalibaleIP = new string[1024];
        GamesAavalibalePort = new string[1024];
        GamesAavalibaleName = new string[1024];
    }

    public void StoreGame(string name,string ip, string port){
        for (int i = 0; i < GamesAavalibaleIP.Length; i++){
            if (GamesAavalibaleIP[i] == null){
                GamesAavalibaleIP[i] = ip;
                GamesAavalibalePort[i] = port;
                GamesAavalibaleName[i] = name;
                return;
            }
        }
    }
}

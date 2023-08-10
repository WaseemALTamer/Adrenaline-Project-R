using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class SceneCurrent : MonoBehaviour
{
    void Update()
    {

        if(SceneManager.GetActiveScene().buildIndex == 0){
            Destroy(GameObject.Find("Player(Clone)"));
        }
    }
}

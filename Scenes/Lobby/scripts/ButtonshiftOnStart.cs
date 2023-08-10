using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonshiftOnStart : MonoBehaviour
{
    public GameObject object1;
    public GameObject object2;
    public int NumOfHostsAvalable;

    public void ShiftButton(){
        if (object1 != null && object2 != null){
            object1.transform.parent = object2.transform;
            //object1.transform.position = object2.transform.position + new Vector3(100,-30 - (NumOfHostsAvalable * 50),0);
            NumOfHostsAvalable ++;
        }
    }
}

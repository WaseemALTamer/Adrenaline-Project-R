using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Contacts : MonoBehaviour
{
    
    public GameObject NetworkMovement;


    
    private float contactAngleWithY;
    public bool Colliding_On_y;

    void OnCollisionStay(Collision collision){

        CalculateCollisionAngles(collision);
        foreach (ContactPoint contact in collision.contacts)
        {
            if (Vector3.Dot(contact.normal, Vector3.up) > 0.1f && contactAngleWithY <= 20){
                Colliding_On_y = true;
            }
        }
    }

    void OnCollisionExit(Collision collision){
        Colliding_On_y = false;
        }


    private void CalculateCollisionAngles(Collision collision)
    {
        if (collision.contacts.Length > 0){
            Vector3 localNormal = collision.contacts[0].normal;
            Vector3 worldNormal = collision.transform.TransformDirection(localNormal);
            contactAngleWithY = Vector3.Angle(worldNormal, Vector3.up);
        }
    }
    private void Start(){
        Colliding_On_y = true;
    }
}

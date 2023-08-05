using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumbMovement : MonoBehaviour
{
    
    public GameObject NetworkMovement;


    
    private Movement MovementScript;
    private float contactAngleWithY;
    private bool Colliding_On_y = false;
    private float timer;

    void OnCollisionStay(Collision collision){

        CalculateCollisionAngles(collision);
        foreach (ContactPoint contact in collision.contacts)
        {
            if (Vector3.Dot(contact.normal, Vector3.up) > 0.1f && contactAngleWithY <= 20){
                // Collision occurred along the Y-axis
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

    // Start is called before the first frame update
    void Start()
    {
        MovementScript = NetworkMovement.GetComponent<Movement>();
        timer = Time.fixedTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (Colliding_On_y == true && Time.fixedTime >= timer){
            MovementScript.JumbAvaliblity = true;
            timer = Time.fixedTime + 0.1f;
            }else{
            MovementScript.JumbAvaliblity = false;
            
        }
    }
}

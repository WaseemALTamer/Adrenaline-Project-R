using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LongJump : MonoBehaviour
{
    public bool Available;
    public float jumpForwardForce;
    public float jumpUPForce;
    public float TimeBetween;
    public KeyCode jump_key;
    private float timer;
    private Rigidbody rb;
    
    // Start is called before the first frame update

    Vector3 move_foraward(float axis_local,float angle, bool GetLocalAxis){
        float yRotation = 0;
        if (GetLocalAxis == true){
            yRotation = transform.eulerAngles.y;
        }
        return new Vector3(axis_local*Mathf.Sin((yRotation + angle) * Mathf.Deg2Rad),0,axis_local*Mathf.Cos((yRotation + angle) * Mathf.Deg2Rad));
        
    } 

    void Start()
    {
        timer = Time.fixedTime;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(jump_key) && Available == true){
            if (timer <= Time.fixedTime){
                rb.AddForce(move_foraward(1*jumpForwardForce,0,true) + new Vector3(0,jumpUPForce,0),ForceMode.Impulse);
                timer = Time.fixedTime + TimeBetween;
            }
        }
    }
}
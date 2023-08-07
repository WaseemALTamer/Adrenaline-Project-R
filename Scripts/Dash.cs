using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : MonoBehaviour
{
    public bool Available;
    public float PushForce;
    public KeyCode InstaDashKey;
    public bool DoubleClick;
    public KeyCode DashKeyDoubleClick;
    public float TimeDuration;
    public float TimeAfter;
    public KeyCode Forward;
    public KeyCode Backward;
    public KeyCode Right;
    public KeyCode Left;

    private Rigidbody rb;
    private bool KeyPress;
    private float timer;
    private float contactAngleWithY;
    private bool Colliding_On_y;
    private float OldDrag;
    private bool state;
    private float TempDrag;
    private int x;
    private int z;

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
        contactAngleWithY = 0;
    }
    
    void CalculateCollisionAngles(Collision collision)
    {
        if (collision.contacts.Length > 0)
        {
            Vector3 localNormal = collision.contacts[0].normal;
            Vector3 worldNormal = collision.transform.TransformDirection(localNormal);
            contactAngleWithY = Vector3.Angle(worldNormal, Vector3.up);
        }
    }

    void Dash_Force(int z, int x){
        if (z == 1){
            rb.AddForce(move_foraward(PushForce,0,true),ForceMode.Impulse);
        }
        if (z == -1){
            rb.AddForce(move_foraward(-PushForce,0,true),ForceMode.Impulse);
        }
        if (x == 1){
            rb.AddForce(move_foraward(PushForce,90,true),ForceMode.Impulse);
        }
        if (x == -1){
            rb.AddForce(move_foraward(PushForce,-90,true),ForceMode.Impulse);
        }
        if(x == 0 && z ==  0){
            rb.AddForce(move_foraward(PushForce,0,true),ForceMode.Impulse);
        } 
    }





    Vector3 move_foraward(float axis_local,float angle, bool GetLocalAxis){
        float yRotation = 0;
        if (GetLocalAxis == true){
            yRotation = transform.eulerAngles.y;
        }
        return new Vector3(axis_local*Mathf.Sin((yRotation + angle) * Mathf.Deg2Rad),0,axis_local*Mathf.Cos((yRotation + angle) * Mathf.Deg2Rad)); 
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        timer = Time.fixedTime;
        OldDrag = rb.drag;
        TempDrag = rb.drag;
        state = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Available == true){
            if (Input.GetKeyDown(Forward)){
                z += 1;
            }
            if (Input.GetKeyDown(Backward)){
                z += -1;
            }
            if (Input.GetKeyDown(Right)){
                x += 1;
            }
            if (Input.GetKeyDown(Left)){
                x += -1;
            }
            if (Input.GetKeyUp(Forward)){
                z -= 1;
            }
            if (Input.GetKeyUp(Backward)){
                z -= -1;
            }
            if (Input.GetKeyUp(Right)){
                x -= 1;
            }
            if (Input.GetKeyUp(Left)){
                x -= -1;
            }
            
            

            if (Input.GetKeyDown(InstaDashKey) && timer <= Time.fixedTime && state == false){
                rb.drag = 0.2f;
                TempDrag = rb.drag;
                Dash_Force(z,x);
                timer = Time.fixedTime + TimeDuration;
                state = true;
            }
            if (Time.fixedTime >= timer && state == true){
                rb.drag = OldDrag;
                TempDrag = rb.drag;
                timer = Time.fixedTime + TimeAfter;
                state = false;
            }
        }
    }
}

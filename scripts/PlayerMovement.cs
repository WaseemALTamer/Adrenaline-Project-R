using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movement : MonoBehaviour
{
    public float rotationSpeed = 3f;
    public float speed = 5;
    public float jumpForce;
    public float MovingForce;
    
    private bool Colliding_On_y = false;
    private bool Colliding_On_x = false;
    private bool Colliding_On_z = false;
    private float contactAngleWithX = 0f;
    private float contactAngleWithY = 0f;

    private float rotationX = 0f;
    private Rigidbody rb;
    private float OldDrag;
    private float x = 0;
    private float z = 0;
    private bool dragSwitcherOnce;
    private float fixedUpdateTime;
    private bool DoubleJump;

    // Start is called before the first frame update

    


    void OnCollisionStay(Collision collision){

        CalculateCollisionAngles(collision);
        foreach (ContactPoint contact in collision.contacts)
        {
            if (Vector3.Dot(contact.normal, Vector3.up) > 0.1f && contactAngleWithY <= 20){
                // Collision occurred along the Y-axis
                Colliding_On_y = true;
            }
            if (Mathf.Abs(collision.contacts[0].normal.x) > 0.1f){
                Colliding_On_x = true;
            }
            if (Mathf.Abs(collision.contacts[0].normal.z) > 0.1f){
                Colliding_On_z = true;
            }
        }
    }

    private void CalculateCollisionAngles(Collision collision)
    {
        if (collision.contacts.Length > 0)
        {
            Vector3 localNormal = collision.contacts[0].normal;
            Vector3 worldNormal = collision.transform.TransformDirection(localNormal);
            contactAngleWithX = Vector3.Angle(worldNormal, Vector3.right);
            contactAngleWithY = Vector3.Angle(worldNormal, Vector3.up);
        }
    }


    Vector3 move_foraward(float axis_local,float angle, bool GetLocalAxis){
        float yRotation = 0;
        if (GetLocalAxis == true){
            yRotation = transform.eulerAngles.y;
        }
        return new Vector3(axis_local*Mathf.Sin((yRotation + angle) * Mathf.Deg2Rad),0,axis_local*Mathf.Cos((yRotation + angle) * Mathf.Deg2Rad));
        
    } 

    void OnCollisionExit(Collision collision){
        Colliding_On_x = false;
        Colliding_On_y = false;
        Colliding_On_z = false;
        contactAngleWithX = 0;
        contactAngleWithY = 0;
    }

    void Start(){
        Colliding_On_y = true;
        rb = GetComponent<Rigidbody>();
        OldDrag = rb.drag;
        rb.freezeRotation = true;
        fixedUpdateTime = Time.fixedTime;
        dragSwitcherOnce = true;
    }



    // Update is called once per frame
    void Update()
    {
        if (Colliding_On_y)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                rb.AddForce(new Vector3(0,jumpForce,0), ForceMode.Impulse);
            }
        }

        rotationX += Input.GetAxis("Mouse X") * rotationSpeed;
        transform.localRotation = Quaternion.Euler(0, rotationX, 0f);
        


        if (Input.GetKeyDown(KeyCode.W)){
            z += 1;
        }
        if (Input.GetKeyUp(KeyCode.W)){
            z -= 1;
        }
        if (Input.GetKeyDown(KeyCode.S)){
            z += -1;
        }
        if (Input.GetKeyUp(KeyCode.S)){
            z -= -1;
        }
        if (Input.GetKeyDown(KeyCode.A)){
            x += -1;
        }
        if (Input.GetKeyUp(KeyCode.A)){
            x -= -1;
        }
        if (Input.GetKeyDown(KeyCode.D)){
            x += +1;
        }
        if (Input.GetKeyUp(KeyCode.D)){
            x -= +1;
            }

        Vector3 movement = move_foraward(z,0,true)*MovingForce + move_foraward(x,90,true)*MovingForce;


        if (Vector3.Distance(rb.velocity,new Vector3 (0,0,0)) >= speed){
            movement = new Vector3 (0,0,0);
        }
        if (Colliding_On_y == false){
            float jumpTime;
            jumpTime = Time.fixedTime - fixedUpdateTime;
            rb.drag = 0.2f;
            movement *=0.4f;
            dragSwitcherOnce = true;
            if (jumpTime >= 0.2f){
                movement *= (0.4f/jumpTime);
            }
            if (Input.GetKeyDown(KeyCode.Space) && DoubleJump == false)
            {
                if (Colliding_On_x == true || Colliding_On_z == true){
                    rb.AddForce(new Vector3(0,jumpForce,0), ForceMode.Impulse);
                    fixedUpdateTime = Time.fixedTime;
                    DoubleJump = true;
                }
            }
            if (Vector3.Distance(rb.velocity,new Vector3 (0,0,0)) >= speed){
                movement = new Vector3 (0,0,0);
            }
        }
        else{
            if (dragSwitcherOnce == true){
            rb.drag = OldDrag;
                dragSwitcherOnce = false;
            }
            
            fixedUpdateTime = Time.fixedTime;
            DoubleJump = false;
        }

        rb.AddForce(movement,ForceMode.Acceleration);
    }
}
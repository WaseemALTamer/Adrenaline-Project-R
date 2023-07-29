using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movement : MonoBehaviour
{
    public float rotationSpeed = 3f;
    public float speed = 5;
    public float jumpForce;
    public float MovingForce;
    public float ContactFriction;

    private bool Colliding_On_y = false;
    private float rotationX = 0f;
    private Rigidbody rb;
    private float OldDrag;
    private float x = 0;
    private float z = 0;
    // Start is called before the first frame update
    void Start(){
        Colliding_On_y = true;
        rb = GetComponent<Rigidbody>();
        OldDrag = rb.drag;
    }

    void OnCollisionEnter(Collision collision){
        foreach (ContactPoint contact in collision.contacts)
        {
            if (Vector3.Dot(contact.normal, Vector3.up) > 0.1f)
            {
                // Collision occurred along the Y-axis
                Colliding_On_y = true;
                break;
            }
        }
    }

    void OnCollisionStay(Collision collision){
        foreach (ContactPoint contact in collision.contacts)
        {
            if (Vector3.Dot(contact.normal, Vector3.up) > 0.1f)
            {
                // Collision occurred along the Y-axis
                Colliding_On_y = true;
                break;
            }
        }
    }

    void OnCollisionExit(Collision collision){
        Colliding_On_y = false;
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
        
        float yRotation = transform.eulerAngles.y;
        


        Vector3 movement = new Vector3(z * MovingForce * Mathf.Sin(yRotation * Mathf.Deg2Rad) + x * MovingForce* Mathf.Sin((yRotation+90) * Mathf.Deg2Rad) , 0, z * MovingForce* Mathf.Cos(yRotation * Mathf.Deg2Rad)+ x *speed* Mathf.Cos((yRotation+90) * Mathf.Deg2Rad));
        Debug.Log(movement);
        if (Vector3.Distance(rb.velocity,new Vector3 (0,0,0)) >= speed){
            movement = new Vector3 (0,0,0);
        }
        if (Colliding_On_y == true && x == 0 && z == 0){
            rb.drag = ContactFriction;
        }
        else{
            rb.drag = OldDrag;
        }
        rb.AddForce(movement,ForceMode.Acceleration);
        
        rb.freezeRotation = true;
    }


}
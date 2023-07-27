using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movement : MonoBehaviour
{
    public float rotationSpeed = 3f;
    public float speed = 5;
    public float jumpForce;
    bool isColliding = false;
    private float rotationX = 0f;


    Rigidbody rb;


    // Start is called before the first frame update
    void Start()
    {
        isColliding = true;
        rb = GetComponent<Rigidbody>();
    }

    void OnCollisionEnter(Collision collision)
    {
        foreach (ContactPoint contact in collision.contacts)
        {
            if (Vector3.Dot(contact.normal, Vector3.up) > 0.5f)
            {
                // Collision occurred along the Y-axis
                isColliding = true;
                break;
            }
        }
    }

    void OnCollisionStay(Collision collision)
    {
        foreach (ContactPoint contact in collision.contacts)
        {
            if (Vector3.Dot(contact.normal, Vector3.up) > 0.5f)
            {
                // Collision occurred along the Y-axis
                isColliding = true;
                break;
            }
        }
    }

    void OnCollisionExit(Collision collision)
    {
        isColliding = false;
    }
    // Update is called once per frame
    void Update()
    {
        if (isColliding)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            }
        }

        rotationX += Input.GetAxis("Mouse X") * rotationSpeed;
        transform.localRotation = Quaternion.Euler(0, rotationX, 0f);

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        float yRotation = transform.eulerAngles.y;

        

        Vector3 movement = new Vector3(z * speed * Mathf.Sin(yRotation * Mathf.Deg2Rad) + x * speed* Mathf.Sin((yRotation+90) * Mathf.Deg2Rad) , rb.velocity.y, z * speed* Mathf.Cos(yRotation * Mathf.Deg2Rad)+ x *speed* Mathf.Cos((yRotation+90) * Mathf.Deg2Rad));
        rb.velocity = movement;
        rb.freezeRotation = true;
    }


}
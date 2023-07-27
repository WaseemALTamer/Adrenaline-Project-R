using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunPickUP : MonoBehaviour
{
    public GameObject Player;
    bool attachment = false;
    private float rotationSpeed = 3f;
    private float rotationX = 0f;
    private float rotationY = 0f;
    private float distance = 0f;
    private Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            var pos = GameObject.Find("Player").transform.position;
            Vector3 objectPosition = transform.position;
            float distance = Mathf.Sqrt(Mathf.Pow(pos.x - objectPosition.x, 2) +
                            Mathf.Pow(pos.y - objectPosition.y, 2) +
                            Mathf.Pow(pos.z - objectPosition.z, 2));
            
            if (distance < 2 && attachment == false)
            {
                attachment = true;
                rb.useGravity = false;
            }
        }



        if (attachment == true){
            var angle = GameObject.Find("Camera").transform.eulerAngles;

            rotationX = angle.x;
            rotationY = angle.y;
            transform.localRotation = Quaternion.Euler(rotationX, rotationY, 0f);

            float yRotation = transform.eulerAngles.y;

            var pos = GameObject.Find("Player").transform.position;
            transform.position = new Vector3(pos.x + 0.6f*Mathf.Sin((yRotation+90) * Mathf.Deg2Rad), pos.y, pos.z + 0.6f*Mathf.Cos((yRotation+90) * Mathf.Deg2Rad));
            
        }


        if (Input.GetKeyDown(KeyCode.G) && attachment == true)
        {
            attachment = false;
            rb.useGravity = true;
            rb.AddForce(Vector3.up * 5, ForceMode.Impulse);
            Debug.Log(transform.eulerAngles.y);
            Vector3 Forward = new Vector3(Mathf.Sin(transform.eulerAngles.y * Mathf.Deg2Rad)*5,0f,Mathf.Cos((transform.eulerAngles.y) * Mathf.Deg2Rad)*5);
            rb.AddForce(Forward, ForceMode.Impulse);
        }


    }
}

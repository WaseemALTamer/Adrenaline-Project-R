using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowObject : MonoBehaviour
{
    public bool Track;
    public GameObject TargetObjectPosition;
    public GameObject TargetObjectAngle;
    public float smoothSpeed = 0.125f;



    private Renderer or;
    private Rigidbody rb;
    private BoxCollider bc;
    public float X;
    public float Y;
    public float Z;



    void Start(){
        rb = GetComponent<Rigidbody>();
        bc = GetComponent<BoxCollider>();
        or = GetComponent<Renderer>();

        if (TargetObjectPosition != null){
            Vector3 desiredPosition = TargetObjectPosition.transform.position + new Vector3(X, Y, Z);
            transform.position = desiredPosition;
        }
    }

    void LateUpdate()
    {

        if (TargetObjectPosition != null){
            if (Track == true){
                Vector3 desiredPosition = TargetObjectPosition.transform.position + new Vector3(X, Y, Z);
                Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
                if (TargetObjectAngle != null){
                    Vector3 desiredAngle = TargetObjectAngle.transform.eulerAngles;
                    transform.eulerAngles = desiredAngle;
                }
                transform.position = smoothedPosition;
                rb.useGravity = false;
                rb.isKinematic = true;
                bc.isTrigger = true;
                or.enabled = false;
            }

            if (Track == false){
                if (rb.useGravity == false);{
                    rb.useGravity = true;
                    rb.isKinematic = false;
                    bc.isTrigger = false;
                    or.enabled = true;
                }
            }
        }
    }
}
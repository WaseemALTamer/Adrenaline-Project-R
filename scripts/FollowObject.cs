using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowObject : MonoBehaviour
{
    public bool Track;
    public GameObject TargetObjectPosition;
    public GameObject TargetObjectAngle;
    public float smoothSpeed = 0.125f;
    public bool ThrowObject;
    public float ThrowForce;
    public float X_Position;
    public float Y_Position;
    public float Z_Position;
    public bool orbit;
    public float OrbitDistance;
    public float x_Orbit_Position;
    public float y_Orbit_Position;
    public Camera targetCamera;
    public LayerMask newCullingMask;
    private LayerMask oldCullingMask;



    private Renderer or;
    private Rigidbody rb;
    private BoxCollider bc;




    void Start(){
        rb = GetComponent<Rigidbody>();
        bc = GetComponent<BoxCollider>();
        or = GetComponent<Renderer>();
        oldCullingMask = targetCamera.cullingMask;
    }

    void LateUpdate()
    {

        if (TargetObjectPosition != null){
            if (Track == true){
                rb.useGravity = false;
                rb.isKinematic = true;
                bc.isTrigger = true;
                or.enabled = false;
                Vector3 desiredPosition = TargetObjectPosition.transform.position;
                if (orbit == true){
                    desiredPosition = TargetObjectPosition.transform.position + new Vector3(OrbitDistance*Mathf.Sin(Mathf.Deg2Rad*(TargetObjectAngle.transform.eulerAngles.y))*Mathf.Cos(Mathf.Deg2Rad*(TargetObjectAngle.transform.eulerAngles.x)),
                                                                                            -OrbitDistance*Mathf.Sin(Mathf.Deg2Rad*(TargetObjectAngle.transform.eulerAngles.x)),
                                                                                            OrbitDistance*Mathf.Cos(Mathf.Deg2Rad*(TargetObjectAngle.transform.eulerAngles.y))*Mathf.Cos(Mathf.Deg2Rad*(TargetObjectAngle.transform.eulerAngles.x)));
                }
                else{
                    desiredPosition += new Vector3(X_Position, Y_Position, Z_Position);
                }
                Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
                if (TargetObjectAngle != null){
                    Vector3 desiredAngle = TargetObjectAngle.transform.eulerAngles;
                    transform.eulerAngles = desiredAngle;
                }
                else{
                    transform.eulerAngles = new Vector3(0,0,0);
                }
                transform.position = smoothedPosition;
                if (targetCamera != null){
                    targetCamera.cullingMask = newCullingMask.value;
                }

            }

            if (Track == false){
                if (rb.useGravity == false){
                    rb.useGravity = true;
                    rb.isKinematic = false;
                    bc.isTrigger = false;
                    or.enabled = true;
                    if (targetCamera != null){
                        targetCamera.cullingMask = oldCullingMask.value;
                    }
                    if (ThrowObject == true){
                        float angleY = transform.eulerAngles.y;
                        float angleX = transform.eulerAngles.x;
                        transform.eulerAngles +=new Vector3(0,90,0);
                        rb.AddForce(new Vector3(ThrowForce * Mathf.Sin(angleY*Mathf.Deg2Rad) * Mathf.Cos(Mathf.Deg2Rad*angleX),
                        -ThrowForce * Mathf.Sin(angleX*Mathf.Deg2Rad),
                        ThrowForce * Mathf.Cos(angleY*Mathf.Deg2Rad) * Mathf.Cos(Mathf.Deg2Rad*angleX)),ForceMode.Impulse);
                    } 
                }
            }
        }
    }
}
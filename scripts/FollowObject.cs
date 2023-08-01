using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowObject : MonoBehaviour
{
    public bool Track;
    public GameObject TargetObjectPosition;
    public GameObject TargetObjectAngle;
    public GameObject TargetPlayer;
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
    private Renderer or;
    private Rigidbody rb;
    private BoxCollider bc;
    private int OrginalLayer;



public void ChangeLayersInChildren(Transform parent,string newLayerName)
    {

        gameObject.layer = LayerMask.NameToLayer(newLayerName);
        foreach (Transform child in transform)
        {
            GameObject childObject = child.gameObject;
            childObject.layer = LayerMask.NameToLayer(newLayerName);
        }
        
    }




    void Start(){
        rb = GetComponent<Rigidbody>();
        bc = GetComponent<BoxCollider>();
        or = GetComponent<Renderer>();
        OrginalLayer = gameObject.layer; 
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
                ChangeLayersInChildren(transform,"TransparentFX");


            }

            if (Track == false){
                if (rb.useGravity == false){
                    rb.useGravity = true;
                    rb.isKinematic = false;
                    bc.isTrigger = false;
                    or.enabled = true;
                    ChangeLayersInChildren(transform,"Default");
                    if (ThrowObject == true){
                        float angleY = transform.eulerAngles.y;
                        float angleX = transform.eulerAngles.x;
                        transform.eulerAngles +=new Vector3(0,90,0);
                        rb.velocity = TargetPlayer.GetComponent<Rigidbody>().velocity;
                        rb.AddForce(new Vector3(ThrowForce * Mathf.Sin(angleY*Mathf.Deg2Rad) * Mathf.Cos(Mathf.Deg2Rad*angleX),
                        -ThrowForce * Mathf.Sin(angleX*Mathf.Deg2Rad),
                        ThrowForce * Mathf.Cos(angleY*Mathf.Deg2Rad) * Mathf.Cos(Mathf.Deg2Rad*angleX)),ForceMode.Impulse);
                    }
                }
            }
        }
    }
}
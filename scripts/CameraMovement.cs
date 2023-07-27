using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{

    public bool TrackRotation;
    public bool TrackPosition;
    public bool Y_axisRotation;
    public GameObject TrackObject;

    
    public float rotationSpeed = 3f;
    private float rotationX = 0f;
    private float rotationY = 0f;
    private float smoothSpeed = 0.125f;

    void TrackPositionFun()
    {
        if (TrackObject != null)
        {
            Vector3 desiredPosition = TrackObject.transform.position + new Vector3(0f, 0.8f, 0f);
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = smoothedPosition;
        }
    }

    void Update()
    {

        if (TrackObject != null){
            if (TrackRotation == true){
                rotationX = TrackObject.transform.eulerAngles.y;
                if (Y_axisRotation == true){
                    rotationY -= Input.GetAxis("Mouse Y") * rotationSpeed;
                    rotationY = Mathf.Clamp(rotationY, -90f, 90f);
                }
                transform.localRotation = Quaternion.Euler(rotationY, rotationX, 0f);
            }
            if (TrackPosition == true){
                TrackPositionFun();
            }

        }
    }
}
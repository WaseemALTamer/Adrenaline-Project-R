using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCast : MonoBehaviour
{
    private float raycastDistance = Mathf.Infinity;
    public Color rayColor = Color.red;
    private bool RaycastSwitch = false;
    private FollowObject fo;
    public LayerMask ignoreLayer;

    // Start is called before the first frame update
    void Start()
    {
        fo = GetComponent<FollowObject>();
    }



    void Update()
    {
        // Get the camera's transform
        Camera cam = Camera.main; // Assuming there is a main camera in the scene, you can also reference it directly
        
        if (cam != null)
        {
            if (Input.GetMouseButtonDown(0)){
                RaycastSwitch = true;
            }
            if (Input.GetMouseButtonUp(0)){
                RaycastSwitch = false;
            }
            


            if (RaycastSwitch == true && fo.Track == true){
                Vector3 screenCenter = new Vector3(Screen.width / 2f, Screen.height / 2f, 0f);
                Ray ray = cam.ScreenPointToRay(screenCenter);
                Debug.DrawRay(ray.origin, ray.direction * raycastDistance, rayColor);
                if (Physics.Raycast(ray, out RaycastHit hit, raycastDistance,~ignoreLayer)){
                    Debug.Log("Raycast hit: " + hit.collider.gameObject.name + " distance:" + Vector3.Distance(ray.origin,hit.point));
                }
            }
        }
    }
}


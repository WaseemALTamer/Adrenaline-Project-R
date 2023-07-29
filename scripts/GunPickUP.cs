using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunPickUP : MonoBehaviour
{
    public GameObject Player;

    private float distance = 0f;
    private Rigidbody rb;
    private FollowObject attachment;
    private bool callback = true;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        attachment = GetComponent<FollowObject>();
    }
    void Once(){
        if (callback == true){
        transform.position = Player.transform.position + new Vector3(attachment.X_Position,attachment.Y_Position,attachment.Z_Position);
        callback = false;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F)){
            var pos = Player.transform.position;
            Vector3 objectPosition = transform.position;
            float distance = Mathf.Sqrt(Mathf.Pow(pos.x - objectPosition.x, 2) +
                            Mathf.Pow(pos.y - objectPosition.y, 2) +
                            Mathf.Pow(pos.z - objectPosition.z, 2));

            if (distance <= 2){
                attachment.Track = true;
                Once();
            }
        }

    if (Input.GetKeyDown(KeyCode.G) && callback == false){
        attachment.Track = false;
        callback = true;
        rb.AddForce(new Vector3(1,1,0),ForceMode.Impulse);
        }
    }
}

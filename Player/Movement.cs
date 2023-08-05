using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Movement : NetworkBehaviour
{
    public GameObject Object;
    public KeyCode forward;
    public KeyCode backward;
    public KeyCode right;
    public KeyCode left;
    public float speed;
    public float JumbForce;
    public float rotationSpeed = 3f;
    public bool JumbAvaliblity; // read only




    private Rigidbody Rigidbody;
    private float rotationX = 0f;
    private float z;
    private float x;
    private bool JumbState;

    //server pridcation
    private Vector3 ServerPostion;
    private int ServerCurrentTick = 1;
    private bool Currently_Correcting;


    private Vector3[] ClientPostionArray;
    private Vector3[] ServerPostionArray;
    private float Timer;
    private int Current_Tick = 1;
    private int Tick_Rate = 128;
    private float Timer_between_Tick;
    //ends here
    

    Vector3 move_forward(float axis_local, float angle, bool GetLocalAxis)
    {
        float yRotation = 0;
        if (GetLocalAxis == true)
        {
            yRotation = Rigidbody.transform.eulerAngles.y;
        }
        return new Vector3(axis_local * Mathf.Sin((yRotation + angle) * Mathf.Deg2Rad), 0, axis_local * Mathf.Cos((yRotation + angle) * Mathf.Deg2Rad));
    }


    

    void Start()
    {
        //Tick implmentation
        Timer_between_Tick = 1f / Tick_Rate;
        ClientPostionArray = new Vector3[1024];
        ServerPostionArray = new Vector3[1024];
        //
        if (forward == KeyCode.None || backward == KeyCode.None || right == KeyCode.None || left == KeyCode.None)
        {
            forward = KeyCode.W;
            backward = KeyCode.S;
            right = KeyCode.D;
            left = KeyCode.A;
        }
        z = 0;
        x = 0;
        Rigidbody = Object.GetComponent<Rigidbody>();
        Rigidbody.freezeRotation = true;
        Currently_Correcting = false;
        JumbState = false;
    }

    void Update()
    {
        if (!IsOwner) return;
        //Tick implmentation
        Timer += Time.deltaTime;
        while (Timer >= Timer_between_Tick){
            Timer -= Timer_between_Tick;
            Current_Tick ++;
            HandleTick();
        }
        if (Current_Tick >= 512) Current_Tick = 0;
        if (ServerCurrentTick == 512) ServerCurrentTick = 0;
        //
        
        rotationX += Input.GetAxis("Mouse X") * rotationSpeed;
        Rigidbody.transform.localRotation = Quaternion.Euler(0, rotationX, 0f);
        if (JumbAvaliblity == true && Input.GetKey(KeyCode.Space) && JumbState == false){
            JumbState = true;
        }
        if (IsOwnedByServer) print(true);else{
            print(false);
        }
    }

    private void HandleTick(){
        //print($"Client Postion:{ClientPostionArray[ServerCurrentTick]} ==== Server Postoin:{ServerPostionArray[ServerCurrentTick]}____{Current_Tick}__{ServerCurrentTick}");
        if (Vector3.Distance(ClientPostionArray[ServerCurrentTick],ServerPostionArray[ServerCurrentTick]) >= 0.01 || Currently_Correcting == true){
            print($"Corrected by:{ClientPostionArray[ServerCurrentTick] - ServerPostionArray[ServerCurrentTick]}");
            HandleCorrection();
        }
        
        z = 0; // Reset z and x axes each Tick
        x = 0;

        if (Input.GetKey(forward)) z += 1;
        if (Input.GetKey(backward)) z += -1;
        if (Input.GetKey(right)) x += 1;
        if (Input.GetKey(left)) x += -1;

        Vector3 desiredForce = (move_forward(z, 0, true) + move_forward(x, 90, true)) * speed;
        if (Vector3.Distance(Rigidbody.velocity ,new Vector3(0,0,0))> 5){
            desiredForce = new Vector3(0, 0, 0);
        }
        Rigidbody.AddForce(desiredForce, ForceMode.Acceleration);
        ClientPostionArray[Current_Tick] = Rigidbody.transform.position;
        if (IsOwnedByServer == true){
            MovementServerRpc(new Vector3(0,0,0),0);
            if (JumbState == true){
                Rigidbody.AddForce(new Vector3(0,JumbForce,0),ForceMode.Impulse);
                JumbState = false;
            }
            return;
        }
        if (JumbState == true){
            MovementServerRpc(desiredForce,JumbForce);
            JumbState = false;
        }else{
            MovementServerRpc(desiredForce,0);
        }
        JumbState = false;
    }

    private bool HandleCorrection()
    {
        if (Currently_Correcting == true){
            print($"Corrected by:{ClientPostionArray[ServerCurrentTick] - ServerPostionArray[ServerCurrentTick]}");
            if (ServerCurrentTick != Current_Tick){
                MovementServerRpc(new Vector3(0,0,0),0);
                Currently_Correcting = true; 
                return false;
                }else{
                Currently_Correcting = false;
            }
        }
        Vector3 smoother = Vector3.Lerp(Rigidbody.transform.position,ServerPostion,0.075f);
        Rigidbody.transform.position = smoother;
        return true;
    }

    [ServerRpc]
    private void MovementServerRpc(Vector3 Force, float JumbForce)
    {
        if (ServerCurrentTick >= 512) ServerCurrentTick = 0;
        ServerCurrentTick ++;
        if (Vector3.Distance(Rigidbody.velocity ,new Vector3(0,0,0))> 5){
            Force = new Vector3(0, 0, 0);
        }
        Rigidbody.AddForce(new Vector3(0, JumbForce, 0),ForceMode.Impulse);
        Rigidbody.AddForce(Force, ForceMode.Acceleration);
        ServerPostion = Rigidbody.transform.position;
        positionClientRpc(OwnerClientId, ServerPostion, ServerCurrentTick);
        EveryClientRpc(ServerPostion, Rigidbody.velocity); 
    }

    [ServerRpc]
    private void JumbServerRpc(Vector3 JumbForce){
        Rigidbody.AddForce(JumbForce,ForceMode.Impulse);
    }

    [ClientRpc]
    private void MessageClientRpc(int Tick){
        print($"{Current_Tick} => {Tick}");
    }
    
    [ClientRpc]
    private void positionClientRpc(ulong targetClientId, Vector3 Pos, int Tick){
        if (targetClientId != OwnerClientId) return;
        ServerPostion = Pos;
        ServerCurrentTick = Tick;
        ServerPostionArray[ServerCurrentTick] = ServerPostion;
    }

    [ClientRpc]
    private void EveryClientRpc(Vector3 Pos , Vector3 speed){
        Vector3 SmoothedPosition = Vector3.Lerp(Rigidbody.transform.position,Pos,0.075f);
        Rigidbody.transform.position = SmoothedPosition;
        Rigidbody.velocity = speed;
    }
}
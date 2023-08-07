using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Movement : NetworkBehaviour
{
    //public GameObject
    public GameObject Object;
    public KeyCode forward;
    public KeyCode backward;
    public KeyCode right;
    public KeyCode left;
    public float Acceleration;
    public float rotationSpeed;
    public float TerminalVelocity;
    public Vector3 JumbForce;





    //Layers
    public LayerMask GroundLayer;
    public LayerMask WallLayer;
    



    //transform.unityGraph
    private Rigidbody Rigidbody;
    private float rotationX = 0f;
    private float z;
    private float x;


    //Dash
    public bool DashAvalibality;
    public KeyCode DashButton;
    public Vector3 DashForce;
    private bool DashState;
    public float DashDragTime;
    public float DashCoolDown;
    public Vector3 CalDashForce;
    private float DragOrginal;
    private float DashTimerDrag;
    private float DashTimerCooldown;
    public float TerminalVelocityDash;

    //LongJumb
    public bool LongJumbAvalibality;
    public KeyCode LongJumbButton;
    public Vector3 LongJumbForce;
    private bool LongJumbState;
    public float LongJumbCoolDown;
    private Vector3 CalLongJumbForce;
    private float LongJumbTimer;
    
    
    
    //grounded and Jumb and (double Jumb)! Progress
    private bool JumbState;
    private bool Grounded;
    private float drag;
    private float AirDrag;
    private RaycastHit hitGround;
    private float JumbCoolDownTime;
    private bool Spacehold;



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
        if (GetLocalAxis == true){
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
        drag = Rigidbody.drag;
        AirDrag = 0.2f;
        DragOrginal = Rigidbody.drag;
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
        //Jumb
        Grounded = Physics.Raycast(Rigidbody.transform.position, Vector3.down, out hitGround, 1.01f, GroundLayer);
        if (Input.GetKey(KeyCode.Space) && JumbState == false && Grounded == true && JumbCoolDownTime < Time.fixedTime && Spacehold == false){
            JumbState = true;
            Spacehold = true;
            JumbCoolDownTime = Time.fixedTime + 0.1f;
        }if (Input.GetKeyUp(KeyCode.Space)){
            Spacehold = false;
        }



        if (Grounded == true){
            Rigidbody.drag = drag;
        }
        //OnLongJumb
        if (LongJumbAvalibality == true){
            if (Input.GetKeyDown(LongJumbButton) && LongJumbState == false && Time.fixedTime >= LongJumbTimer){
                LongJumbState = true;
                LongJumbTimer = Time.fixedTime + LongJumbCoolDown;
            }
        }
        
        //OnDash
        if (DashAvalibality == true){
            if (Input.GetKeyDown(DashButton) && DashState== false && Time.fixedTime >= DashTimerCooldown){
                DashState = true;
                drag = 0.2f;
                DashTimerDrag = Time.fixedTime + DashDragTime;
                DashTimerCooldown = Time.fixedTime + DashCoolDown;
            }if(Time.fixedTime >= DashTimerDrag){
                drag = DragOrginal;
            }
        }
        //print(Vector3.Distance(Rigidbody.velocity ,new Vector3(0,0,0)));
    }

    private void HandleTick(){
        //print($"Client Postion:{ClientPostionArray[ServerCurrentTick]} ==== Server Postoin:{ServerPostionArray[ServerCurrentTick]}____{Current_Tick}__{ServerCurrentTick}");
        if (Vector3.Distance(ClientPostionArray[ServerCurrentTick],ServerPostionArray[ServerCurrentTick]) >= 0.01 || Currently_Correcting == true){
            print($"Corrected by:{ClientPostionArray[ServerCurrentTick] - ServerPostionArray[ServerCurrentTick]}");
            HandleCorrection();
        }
        RotationServerRpc(Rigidbody.transform.eulerAngles);
        
        z = 0; // Reset z and x axes each Tick
        x = 0;

        if (Input.GetKey(forward)) z += 1;
        if (Input.GetKey(backward)) z += -1;
        if (Input.GetKey(right)) x += 1;
        if (Input.GetKey(left)) x += -1;

        Vector3 desiredForce = (move_forward(z, 0, true) + move_forward(x, 90, true)) * Acceleration;
        if (Vector3.Distance(Rigidbody.velocity ,new Vector3(0,0,0))> TerminalVelocity){
            desiredForce = new Vector3(0, 0, 0);
        }
        if (Grounded == false){
           desiredForce *= 0.2f;
           Rigidbody.drag = AirDrag;
        }
        if (IsOwnedByServer == true){
            Rigidbody.AddForce(desiredForce, ForceMode.Acceleration);
            if (JumbState == true){
                Rigidbody.velocity = new Vector3 (Rigidbody.velocity.x,0,Rigidbody.velocity.z);
                Rigidbody.AddForce(JumbForce,ForceMode.Impulse);
            }
            if (DashState == true){
                CalDashForce = move_forward(DashForce.z,0,true);
                if (x == +1)CalDashForce = move_forward(DashForce.z,90,true);
                if (x == -1)CalDashForce = move_forward(DashForce.z,90,true) * -1;
                if (z == -1)CalDashForce =  move_forward(DashForce.z,0,true) *-1;
                if (z == +1)CalDashForce = move_forward(DashForce.z,0,true);
                Rigidbody.velocity = new Vector3(0,Rigidbody.velocity.y,0);
                Rigidbody.AddForce(CalDashForce,ForceMode.Impulse);
            }
            if (LongJumbState == true){
                Rigidbody.velocity = new Vector3(0,0,0);
                CalLongJumbForce = move_forward(LongJumbForce.z,0, true) + new Vector3(0,LongJumbForce.y,0);
                Rigidbody.AddForce(CalLongJumbForce,ForceMode.Impulse);
            }
            Vector3 Normal = new Vector3(0,0,0);
            MovementServerRpc(Normal,Normal,false,Normal,false,Normal,false,Rigidbody.drag);
            JumbState = false;
            DashState = false;
            LongJumbState = false;
            ServerPostion = Rigidbody.transform.position;
            Current_Tick = ServerCurrentTick;
            ClientPostionArray[Current_Tick] = Rigidbody.transform.position;
            return;
        }else{
            if (DashState == true){
                CalDashForce = move_forward(DashForce.z,0,true);
                if (x == +1)CalDashForce = move_forward(DashForce.z,90,true);
                if (x == -1)CalDashForce = move_forward(DashForce.z,90,true) * -1;
                if (z == -1)CalDashForce =  move_forward(DashForce.z,0,true) *-1;
                if (z == +1)CalDashForce = move_forward(DashForce.z,0,true);
            }
            if (LongJumbState == true){
                CalLongJumbForce = move_forward(LongJumbForce.z,0, true) + new Vector3(0,LongJumbForce.y,0);
            }

            MovementServerRpc(desiredForce,JumbForce,JumbState,CalDashForce,DashState,CalLongJumbForce,LongJumbState,Rigidbody.drag);
            ApplyClinetForces(desiredForce,JumbForce,JumbState,CalDashForce,DashState,CalLongJumbForce,LongJumbState,Rigidbody.drag);
            JumbState = false;
            DashState = false;
            LongJumbState = false;
        }
    }

    private bool HandleCorrection()
    {
        print($"Corrected by:{Rigidbody.transform.position - ServerPostion}");
        if (Currently_Correcting == true){
            if (ServerCurrentTick != Current_Tick){
                Vector3 Normal = new Vector3(0,0,0);
                MovementServerRpc(Normal,Normal,false,Normal,false,Normal,false,Rigidbody.drag);
                Currently_Correcting = true; 
                return false;
                }else{
                Currently_Correcting = false;
            }
        }
        Vector3 smoother = Vector3.Lerp(Rigidbody.transform.position,ServerPostion,0.125f);
        Rigidbody.transform.position = smoother;
        return true;
    }

    private void ApplyClinetForces(Vector3 Force, Vector3 JumbForce, bool JumbState, Vector3 DashForce,bool DashState,Vector3 LongJumbForce,bool LongJumbState,float drag){
        Rigidbody.drag = drag;
        Rigidbody.AddForce(Force, ForceMode.Acceleration);
        if (JumbState == true){
            Rigidbody.velocity = new Vector3 (Rigidbody.velocity.x,0,Rigidbody.velocity.z);
            Rigidbody.AddForce(JumbForce,ForceMode.Impulse);
        }
        if (DashState == true){
            Rigidbody.velocity = new Vector3(0,Rigidbody.velocity.y,0);
            Rigidbody.AddForce(DashForce,ForceMode.Impulse);
        }
        if (LongJumbState == true){
            Rigidbody.velocity = new Vector3(0,0,0);
            Rigidbody.AddForce(LongJumbForce,ForceMode.Impulse);
        }
        ServerPostion = Rigidbody.transform.position;
        ClientPostionArray[Current_Tick] = Rigidbody.transform.position;
    }


    [ServerRpc]
    private void MovementServerRpc(Vector3 Force, Vector3 JumbForce, bool JumbState, Vector3 DashForce,bool DashState,Vector3 LongJumbForce,bool LongJumbState,float drag){
        Rigidbody.drag = drag;
        Rigidbody.AddForce(Force, ForceMode.Acceleration);
        if (JumbState == true){
            Rigidbody.velocity = new Vector3 (Rigidbody.velocity.x,0,Rigidbody.velocity.z);
            Rigidbody.AddForce(JumbForce,ForceMode.Impulse);
        }
        if (DashState == true){
            Rigidbody.velocity = new Vector3(0,Rigidbody.velocity.y,0);
            Rigidbody.AddForce(DashForce,ForceMode.Impulse);
        }
        if (LongJumbState == true){
            Rigidbody.velocity = new Vector3(0,0,0);
            Rigidbody.AddForce(LongJumbForce,ForceMode.Impulse);
        }
        if (ServerCurrentTick >= 512) ServerCurrentTick = 0;
        ServerPostion = Rigidbody.transform.position;
        ServerCurrentTick ++;
        positionClientRpc(OwnerClientId, ServerPostion, ServerCurrentTick);
        EveryClientRpc(ServerPostion, Rigidbody.velocity , Rigidbody.transform.eulerAngles); 
    }

    [ServerRpc]
    private void RotationServerRpc(Vector3 Rotaion){
        Rigidbody.transform.eulerAngles = Rotaion;
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
    private void EveryClientRpc(Vector3 Pos , Vector3 speed , Vector3 angle){
        Vector3 SmoothedPosition = Vector3.Lerp(Rigidbody.transform.position,Pos,0.075f);
        Rigidbody.transform.position = SmoothedPosition;
        Rigidbody.velocity = speed;
        Rigidbody.transform.eulerAngles = angle;
    }
}
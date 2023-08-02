using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InVisibility : MonoBehaviour
{
    // Reference to the Renderer component on the GameObject
    public bool Available;
    private Shader originalShader;
    public Shader graphicShader;
    public KeyCode InvisibilityKey;
    public float TimeAfter;
    public float DurationTime;
    private Renderer renderer;
    private float timer;
    private bool state;
    private MeshRenderer MR;
    



    public void EnableGraphicShader(){
        renderer.material.shader = graphicShader;
        MR.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        state = true;
    }
    public void DisableGraphicShader(){
        renderer.material.shader = originalShader;
        MR.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
        state = false;
    }
    
    void Start(){
        renderer = GetComponent<Renderer>();
        MR = GetComponent<MeshRenderer>();
        originalShader = renderer.material.shader;
        timer = Time.fixedTime;
        state = false;
    }

    void Update(){
        if (Available == true){
            if (Input.GetKeyDown(InvisibilityKey) && state == true && timer > Time.fixedTime){
                DisableGraphicShader();
                timer = Time.fixedTime + TimeAfter;;
            }
            if (Input.GetKeyDown(InvisibilityKey) && state == false && timer <= Time.fixedTime){
                EnableGraphicShader();
                timer = Time.fixedTime + DurationTime;
            }
            if (timer <= Time.fixedTime && state == true){
                DisableGraphicShader();
                timer = Time.fixedTime + TimeAfter;
            }

        }
    }
}
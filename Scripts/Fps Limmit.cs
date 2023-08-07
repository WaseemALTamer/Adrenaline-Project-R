using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FpsLimmit : MonoBehaviour
{
    void Start()
    {
        Application.targetFrameRate = 240;
    }
}

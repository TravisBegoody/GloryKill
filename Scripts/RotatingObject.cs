using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingObject : MonoBehaviour
{
    void Start()
    {
        
    }
    void FixedUpdate()
    {
        this.transform.Rotate(0f, 0f, 90.0f * Time.deltaTime, Space.Self);
    }
}

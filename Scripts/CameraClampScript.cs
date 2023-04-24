using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraClampScript : MonoBehaviour
{
    private Camera mainCamera;
    public bool XAxisClamp,YAxisClamp;//Will allow the camera to move in that axis or neither at all
    /// <summary>
    /// Makes a script that will clamp the camera to an area when the player walks into the 
    /// area and stops when they leave and should also keep the camera out of this area til the player walks in
    /// </summary>
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Clamp Here");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "player")
        {

        }
    }
}

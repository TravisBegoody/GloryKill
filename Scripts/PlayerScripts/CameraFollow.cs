using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public static CameraFollow mainCam;
    private Camera playerCamera;

    public float smoothTimeY;
    public float smoothTimeX;

    public float vertEdge;// How close the Crosshairs are pushed into the middle from the top and bottom edges
    public float horzEdge;// How close the Crosshairs are pushed into the middle from the left and right edges
    private float vertExtent;
    private float horzExtent;

    private float VelocityXLimit; // How far the Velocity of the character can before stopping
    private float VelocityYLimit; // How far the Velocity of the character can before stopping

    public GameObject player;
    public GameObject crosshair;
    public GameObject lastPosition;

    private Rigidbody2D rb;

    public float predAhead;

    public float zoomFactor = 1.5f;
    public float followTimeDelta = 0.8f;

    void Start()
    {
        mainCam = this;
        player = PlayerController.pGameObject;
        rb = player.GetComponent<Rigidbody2D>();

        playerCamera = this.gameObject.GetComponent<Camera>();

        vertExtent = playerCamera.orthographicSize;
        horzExtent = vertExtent * Screen.width / Screen.height;
    }

    void FixedUpdate()
    {
        if(player != null && PlayerController.script.canCamMove)
        {
            //Debug.Log("Camera can't find the Player GameObject"); //Quick removal to check if something runs wrong
            FixedCameraFollowSmooth(playerCamera, player.transform, crosshair.transform);
        }
        else
        {
            //Vector2 target_Offset = transform.position - lastPosition.transform.position;
            //Debug.Log(target_Offset);
            //Debug.Log(Vector2.Lerp(transform.position, target_Offset, 0.1f));


            this.transform.position = Vector2.Lerp(transform.position, lastPosition.transform.position, 0.1f);
            this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, -10f);//Sets it to have camera maintain a z of -10f
        }
    }
    // Follow Two Transforms with a Fixed-Orientation Camera
    public void FixedCameraFollowSmooth(Camera cam, Transform t1, Transform t2) //t1 player, t2 camera
    {
        // How many units should we keep from the players
        

        // Midpoint we're after
        Vector3 midpoint = ( (t1.position * 3f ) + t2.position ) / 4f; // keeps the midpoint closer to the player ((player.pos * 3) + camera.pos) / 4 = 3/4 extension
                                                                                                           // 1/4 of behind player still visible
        //Debug.Log("t1 "+t1.position);
        //Debug.Log("t2 " + t2.position);
        //Debug.Log("midpoint "+midpoint);
        //midpoint = addVelocities(midpoint);

        // Distance between objects
        float distance = (t1.position - t2.position).magnitude;

        // Move camera a certain distance
        Vector3 cameraDestination = midpoint - cam.transform.forward * distance * zoomFactor;

            cam.transform.position = cameraDestination;
        cam.transform.position = new Vector3(cam.transform.position.x, cam.transform.position.y, -10f);
    }
    private float CalculatePlayerVelocityX()
    {
        return rb.velocity.x * 0.1f;
    }
    private float CalculatePlayerVelocityY()
    {
        return rb.velocity.y * 0.05f;
    }
    private Vector3 addVelocities(Vector3 oldMid)
    {
        return new Vector3(oldMid.x + CalculatePlayerVelocityX(), oldMid.y + CalculatePlayerVelocityY(), oldMid.z);
    }
    public void SetCamToLastPosition()//If the Player is dead then it sets it to be where they last where
    {
        //Debug.Log(this.player.transform.position); //Checks where the camera will be set to in the world space

        lastPosition.transform.position = this.player.transform.position;
    }
    public float GetVertExtentMin()
    {
        return this.transform.position.y - vertExtent + vertEdge;
    }
    public float GetVertExtentMax()
    {
        return this.transform.position.y + vertExtent - vertEdge;
    }
    public float GetHorzExtentMin()
    {
        return this.transform.position.x - horzExtent + horzEdge;
    }
    public float GetHorzExtentMax()
    {
        return this.transform.position.x + horzExtent - horzEdge;
    }
}

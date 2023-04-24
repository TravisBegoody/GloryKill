using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    public int id;
    //Waypoint B should always be the closed option and a be the opened
    public GameObject waypointA, waypointB;
    public GameObject doorObject;

    public float speed;
    [SerializeField]protected bool isClosed;//Closed should be at waypointB
    protected bool isMoving;
    void Start()
    {
        GameEvents.current.onDoorTrigger += DoorTriggered;
        isClosed = true;
    }
    void FixedUpdate()
    {
        doorUpdate();
    }
    public void doorUpdate()
    {
        if(isMoving)
        {
            Vector2 currWaypoint, platPos;

            if(isClosed) { currWaypoint = waypointA.transform.position; }
            else { currWaypoint = waypointB.transform.position; }

            platPos = Vector2.MoveTowards(doorObject.transform.position, currWaypoint, speed * Time.fixedDeltaTime);
            doorObject.transform.position = platPos;
            if(Vector2.Distance(doorObject.transform.position, currWaypoint) < 0.01f)
                isMoving = false;
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
            GameEvents.current.doorTrigger(id);
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.tag == "Player")
            GameEvents.current.doorTrigger(id);
    }
    public void DoorTriggered(int id)
    {
        if(id == this.id)
        {
            isClosed = !isClosed;
            isMoving = true;
        }
    }
}

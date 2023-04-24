using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The Script should take the WaypointTransport Script and use 
/// it to help the platform move between points 
/// </summary>
public class WaypointPlatformScript : WaypointTransportScript
{

    public bool canCrush;//canCrush when its moving to waypointA will crush the player when within 3f of waypointA


    // Update is called once per frame
    //void FixedUpdate()
    //{
        /*
        Vector2 platPos;
        if(movingToPoint)
        {
            Vector2 currWaypoint;
            if(moveToWaypointA)
                currWaypoint = waypointA.transform.position;
            else
                currWaypoint = waypointB.transform.position;

            platPos = Vector2.MoveTowards(this.transform.position, currWaypoint, speed * Time.fixedDeltaTime);
            this.transform.position = platPos;

            if(Vector2.Distance(this.transform.position, currWaypoint) < 0.01f)
                movingToPoint = false;
            //Checks Distances
            //Debug.Log("Distance to Waypoint: " + Vector2.Distance(this.transform.position, currWaypoint));
        }
        */
    //}

    public void startMoving()
    {
        toNextPoint = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
            other.transform.parent = this.transform;
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.tag == "Player")
            other.transform.parent = null;
    }
}

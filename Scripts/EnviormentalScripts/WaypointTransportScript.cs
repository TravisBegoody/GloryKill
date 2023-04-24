using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The Script will move between as many points set by the script
/// </summary>
public class WaypointTransportScript : MonoBehaviour
{
    //Should be used on enemies that shoot while moving and traps that move around
    //Enemy Patrols back and forth and stops and the ends of the patrol route then loops around
    
    public Transform[] points;
    int currPoint;
    //Always Moving Keeps objects that want to keep moving never be set to false
    public bool toNextPoint, alwaysMoving;

    bool canMove;//have a delay on after they see the player to resume walking
    ///if the player is higher than them have a delay then they jump to shoot the player same as the shot gun enemy
    ///alert when the player comes within a certain radius of the enemy
    public float moveSpeed;
    void Start()
    {
        currPoint = 0;
    }
    private void FixedUpdate()
    {
        if(toNextPoint || alwaysMoving)
        {
            MoveToPoint(points[currPoint]);
        }
    }
    private void MoveToPoint(Transform currentPoint)
    {
        this.transform.position =  Vector2.MoveTowards(this.transform.position, currentPoint.position, moveSpeed * Time.fixedDeltaTime);
        if(Vector3.Distance(this.transform.position, currentPoint.position) <= 0.1f)
        {
            toNextPoint = false;
            currPoint++;
            if(currPoint == points.Length)
                currPoint = 0;
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
            other.transform.parent = this.transform;
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Player")
            other.transform.parent = null;
    }
}

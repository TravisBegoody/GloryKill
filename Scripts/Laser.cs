using UnityEngine;

public class Laser : MonoBehaviour
{

    private LineRenderer laserLineRenderer;

    [SerializeField] private LayerMask collisionLayerMask;
    [SerializeField] private LaserType type;
    
    //Used to determine if it is sensing for anything or if it is damaging them instead
    public enum LaserType
    {
        Sensor, //used to trigger something else in relation when the laser is tripped by player
        Damage, //damages the player when they are inside of the lasers collision
    }
    void Start()
    {
        laserLineRenderer = GetComponent<LineRenderer>(); //Gets the line renderer of the gameobject
    }
    void FixedUpdate()
    {
        CastLaser();
    }
    /// <summary>
    /// When run casts a raycast in the direction the laser is facing and when it hits objects it checks if the first is the player and if it is a platform first then it looks as if it is hitting that first.
    /// </summary>
    void CastLaser()
    {
        laserLineRenderer.SetPosition(0, transform.position);

        var ray = new Ray(this.transform.position, transform.forward);

        //Raycasts a line in the direction that the laser is facing 5000 units out and gets all layers in the collision mask in an array
        RaycastHit2D[] hit = Physics2D.RaycastAll(transform.position, transform.forward, 500f, collisionLayerMask);

        //if the laser touched nothing then nothing should happen
        LaserCollidesNothing();

        //Runs for every object hit
        for (int i = 0; i < hit.Length; i++)
        {
            if (PlayerController.script != null && hit[i].collider.gameObject == PlayerController.script.gameObject)
            {//If it hits the player
                LaserHitPlayer(hit);
                if (hit.Length == 1)
                    LaserCollidesNothing();
            }
            else//It hits a platform
            {

                //Runs when the laser collides with something that isn't the player and then they would set the impact spot to the position of where the laser last hit
                laserLineRenderer.SetPosition(1, hit[i].point + AdjustLaserDepth(hit[i].point));
                i = hit.Length;
            }
        }
    }
    /// <summary>
    /// When the laser collides with nothing else it will then set itself 500f at one end point away from where it is facing
    /// </summary>
    private void LaserCollidesNothing()
    {
        Vector2 endLaser = (Vector2)this.transform.position + (Vector2)transform.forward * 500f;
        laserLineRenderer.SetPosition(1, endLaser);//Sets the second end point to where the end of the laser should go
    } 
    //Adds a little more to the direction the laser is pointing in to push the laser deeper into an object
    Vector2 AdjustLaserDepth(Vector2 hitPoint)
    {
        Vector2 temp = (Vector2)( this.transform.position ) - hitPoint; 
        float ftempRot = Mathf.Atan2(temp.y, temp.x) * Mathf.Rad2Deg + 180f;
        float newX = Mathf.Cos(( ftempRot ) * Mathf.Deg2Rad);
        float newY = Mathf.Sin(( ftempRot ) * Mathf.Deg2Rad);
        return new Vector2(newX, newY);
    }
    /// <summary>Runs when the laser hits the player and does something based on what type of laser is currently hitting him.</summary>
    private void LaserHitPlayer(RaycastHit2D[] hit)
    {
        switch(type)
        {
            case LaserType.Damage:
                PlayerController.script.Damage(10f);
                break;
        }
    }
}
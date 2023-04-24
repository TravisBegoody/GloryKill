using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointAndShoot : MonoBehaviour
{
    //Make this work for enemies
    public GameObject bulletStart;//Where the bullet starts at
    public GameObject gunHolder; //part of the turret that turns
    public GameObject bulletHolder;

    public GameObject targetObject;//The gameobject that is being targeted
    private Vector3 target;//The position of the gameobject
    

    public float cooldownTime;//Time inbetween shots
    private float currTime;//Time that has past since the last shot

    public GameObject bulletPrefab;//The Gameobject that is being shot.
    public float bulletSpeed;//How fast the object will travel (35f should be default among bullets)

    private bool canShoot;//is the Gun able to shoot at the player or not

    public float range;//How far the bullet will shoot
    public LayerMask whatIsCollide;//What the raycast wil see as a wall/player

    public int bulletAmount;//How many Bullets are being shot by the gun
    public float angle;//Angle difference between all bullets

    void Start()
    {
        if(this.gameObject.tag == "Enemy")
        {
            targetObject = PlayerController.pGameObject;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(targetObject != null && Vector2.Distance(targetObject.transform.position, gunHolder.transform.position) <= range)// a quick fix to try and reduce lag
            tryShootingPlayer();
    }
    void tryShootingPlayer()
    {
        target = targetObject.transform.position;
        Vector3 GunHolder = gunHolder.transform.position;
        RaycastHit2D hit = Physics2D.Raycast(GunHolder,target - GunHolder, range, whatIsCollide);

        if(Vector2.Distance(GunHolder, target) <= range) //Draws the Ray in the Editor to see the ray and how it sees the player
            Debug.DrawRay(GunHolder, target - GunHolder);
        //verfies that the hit raycast isnt null and the collider is a player
        if(hit.collider != null && hit.collider.tag == "Player")
        {
            Vector2 difference = ( target - GunHolder ) * this.transform.localScale.x; // Determines the x and y of how far away the bullet it ex. (-1, 1) = left and up;

            // Gets the degrees of the bullet and helps aim the bullet direction ex: 180 = directly left of bullet
            float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;

            //Rotates the gun in the direction of the player
            gunHolder.transform.rotation = Quaternion.Euler(0f, 0f, rotationZ);

            //Shoots when the cooldown is reached
            if(currTime >= cooldownTime)
            {
                #region Sum
                //Thank you Namey5(Unity Answers) for helping on this part of code

                //Let's say we have an angle 'a', which we get from 'a = atan2 (y, x)'
                //'atan' is the inverse of 'tan', therefore 'atan (tan (a)) = a'
                //We also know that 'tan (a) = sin (a) / cos (a)'
                //Therefore 'atan (sin (a) / cos (a)) = a'
                //You'll notice that 'Vector2 (cos (a), sin (a))' is a point on the circumference of a unit circle rotated 'a' degrees
                //This is actually the basis of how 'atan2' is calculated in the first place, i.e. 'atan2 (y, x) ~ atan (y / x)' (not strictly correct, but close enough for this example)
                //Therefore, for a point/vector at 'theta' degrees; x = cos (theta), y = sin (theta)
                #endregion
                //Half of total angle
                float halfTotAng;

                if(bulletAmount % 2 == 1) //Comes true if the bulletAmount is odd
                    halfTotAng = ( angle * ( bulletAmount - 1 ) ) / 2;// will make bulletamounts that are 1 always at 0
                else 
                    halfTotAng = ( angle * bulletAmount / 2) - (angle / 2); //Math shoots out the even amount of bullets around the player pretty much

                for(float i = -halfTotAng; i <= halfTotAng; i += angle)//adds a bullet for each 
                {
                    float currentRotate = rotationZ + i;

                    float newX = Mathf.Cos((currentRotate) * Mathf.Deg2Rad);
                    float newY = Mathf.Sin((currentRotate) * Mathf.Deg2Rad);

                    //Debug.Log("Added Angle: " + newX + ", " + newY);

                    Vector2 direction = new Vector2(newX, newY);
                    //Debug.Log("Direction: " + direction);


                    fireBullet(direction, currentRotate); //Instantiates the bullets
                }
                

                currTime = 0f;
            }

            currTime += Time.deltaTime;
        }
    }
    void fireBullet(Vector2 direction, float rotationZ)
    {
        GameObject b = Instantiate(bulletPrefab) as GameObject;

        b.transform.position = bulletStart.transform.position;
        b.transform.rotation = Quaternion.Euler(0f, 0f, rotationZ);
        b.GetComponent<Rigidbody2D>().velocity = direction * bulletSpeed * this.transform.localScale.x;
        if (bulletHolder != null)
        {
            b.transform.parent = bulletHolder.transform;
        }
    }
}

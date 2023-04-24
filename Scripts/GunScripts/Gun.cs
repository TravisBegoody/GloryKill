using System.Collections;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] private string gunName;
    //Bullet Stats
    [SerializeField] protected float gunDamage; // how much damage each gun will deal
    [SerializeField] private int bulletShellSize;// The amount of bullets shot in each fire
    [SerializeField] private float bulletCooldown;//Time between each shot
    [SerializeField] private float bulletSpeedModifier;//How fast the bullets should travel (35f should be default among bullets)
    [SerializeField] private float bulletMaxAngle;//angle between each bullet
    [SerializeField] private float bulletAngle;
    private float lastBulletRot = 0;//Used to try and get bullets to fire in a more predicable line
    private bool lastBulletDown;//Goes down when the bullet reaches the top of the angle
    //Magazine Stats
    [SerializeField] private int magazineSize;//How many bullets a gun holds at base
    [SerializeField] private float reloadSpeed;//Time to reload

    //Randomizer Stats
    [SerializeField] private bool randAngles;//Angles are randomized to be inbetween the angle above
    [SerializeField] private float randSpeed;//Bullets will be shot at a randomized speed EX: (BASE * BulletSpeed) -+ randSpeed;

    public Gun()
    {
        //Debug.Log("Created an Empty Gun");
    }
    public Gun(float damage, float cooldown, int GunMagazineSize, float GunReloadSpeed)//basic gun attributes
    {
        gunDamage = damage;
        bulletShellSize = 1;
        bulletCooldown = cooldown;
        bulletSpeedModifier = 35f;
        bulletMaxAngle = 10;//Causes errors if its 0

        magazineSize = GunMagazineSize;
        reloadSpeed = GunReloadSpeed;

        randAngles = false;
        randSpeed = 0f;
    }
    public Gun(float damage,int shellSize, float cooldown,float bulletSpeed,float angle, int GunMagazineSize, float GunReloadSpeed,bool randomAngles,float randomSpeed)//Full gun attributes
    {
        gunDamage = damage;
        bulletShellSize = shellSize;
        bulletCooldown = cooldown;
        bulletSpeedModifier = bulletSpeed;
        bulletMaxAngle = angle;//Causes errors if its 0

        magazineSize = GunMagazineSize;
        reloadSpeed = GunReloadSpeed;

        randAngles = randomAngles;
        randSpeed = randomSpeed;
    }

    /// <summary>
    /// Returns an array of angles that are used to shoot
    /// </summary>
    /// <param name="currentRotation">The Current Rotation that the gun should be at</param>
    /// <param name="bulletShot">The amount of bullets in each shot</param>
    /// <param name="angle">The Angle that the bullets should differitiate in</param>
    /// <returns></returns>
    public object[,] CalculateBullets(float currentRotation)
    {
        //Saves Angles and directions
        object[,] bulletCalcs = new object[bulletShellSize, 2];

        //Should calculate to Half of total angle
        float halfTotAng;

        //Comes true if the bulletAmount is odd
        if(bulletShellSize % 2 == 1)
            // will make bulletamounts that are 1 always at 0
            halfTotAng = ( bulletMaxAngle * ( bulletShellSize - 1 ) ) / 2;
        else
            //Math shoots out the even amount of bullets around the player pretty much
            halfTotAng = ( bulletMaxAngle * bulletShellSize / 2 ) - ( bulletMaxAngle / 2 );
        //Location of current array location
        int arrLoc = -1;
        float currAngle = -halfTotAng;
        for(float i = 0; i < bulletShellSize; i++)
        {
            float calcdRotation;

            if (-lastBulletRot >= bulletMaxAngle || bulletMaxAngle <= lastBulletRot)
            {
                lastBulletRot = Mathf.Clamp(lastBulletRot, -bulletMaxAngle, bulletMaxAngle);
                lastBulletDown = !lastBulletDown;
            }

            if (lastBulletDown)
                lastBulletRot += bulletAngle;
            else
                lastBulletRot -= bulletAngle;



            if (randAngles)
                calcdRotation = currentRotation + lastBulletRot; 
            else
                calcdRotation = currentRotation + currAngle;
            currAngle += bulletMaxAngle;
            arrLoc++;

            //adds the angles to the angDir
            bulletCalcs[arrLoc, 0] = calcdRotation;
            float newX = Mathf.Cos(( calcdRotation ) * Mathf.Deg2Rad);
            float newY = Mathf.Sin(( calcdRotation ) * Mathf.Deg2Rad);

            //Debug.Log("Added Angle: " + newX + ", " + newY);

            bulletCalcs[arrLoc, 1] = new Vector2(newX, newY);
            //Debug.Log("Direction: " + direction);
        }
        return bulletCalcs;
    }
    /// <summary>
    /// For when the bullets aren't associated with a gun script and need their angles,
    /// amount of bullets to be fired
    /// </summary>
    /// <param name="currentRotation">What direction the bullet should be facing</param>
    /// <param name="angle">The Difference in angles of the bulletAmt</param>
    /// <param name="bulletAmt">How many bullets are being shot at once</param>
    /// <returns>Returns array of all bullets and what direction they are facing</returns>
    public object[,] CalculateBullets(float currentRotation, float angle, int bulletAmt)
        
    {
        //Saves Angles and directions
        object[,] bulletCalcs = new object[bulletAmt, 2];

        //Should calculate to Half of total angle
        float halfTotAng;

        //Comes true if the bulletAmount is odd
        if(bulletAmt % 2 == 1)
            // will make bulletamounts that are 1 always at 0
            halfTotAng = ( angle * ( bulletAmt - 1 ) ) / 2;
        else
            //Math shoots out the even amount of bullets around the player pretty much
            halfTotAng = ( angle * bulletAmt / 2 ) - ( angle / 2 );
        //Location of current array location
        int arrLoc = -1;
        float currAngle = -halfTotAng;
        for(float i = 0; i < bulletAmt; i++)
        {
            float calcdRotation;

            if(randAngles)
                calcdRotation = currentRotation + lastBulletRot;
            else
                calcdRotation = currentRotation + currAngle;
            currAngle += angle;
            arrLoc++;

            //adds the angles to the angDir
            bulletCalcs[arrLoc, 0] = calcdRotation;
            float newX = Mathf.Cos(( calcdRotation ) * Mathf.Deg2Rad);
            float newY = Mathf.Sin(( calcdRotation ) * Mathf.Deg2Rad);

            //Debug.Log("Added Angle: " + newX + ", " + newY);

            bulletCalcs[arrLoc, 1] = new Vector2(newX, newY);
            //Debug.Log("Direction: " + direction);
        }
        return bulletCalcs;
    }
    //Add a method to calculate random angles
    //add a method to calculate random speed
    #region Get each stats Methods
    /// <summary>
    /// Returns the damage of the gun
    /// </summary>
    public string GetName()
    {
        return gunName;
    }
    public void SetDamage(float damage)
    {
        this.gunDamage = damage;
    }
    /// <summary>
    /// Returns the damage of the gun
    /// </summary>
    public float GetDamage()
    {
        return gunDamage;
    }
    /// <summary>
    /// Returns the amount of bullets in each shell of the gun
    /// </summary>
    public int GetBulletShellSize()
    {
        return bulletShellSize;
    }
    /// <summary>
    /// Returns the cooldown between shots of the gun
    /// </summary>
    public float GetCooldown()
    {
        return bulletCooldown;
    }
    /// <summary>
    /// Returns the speed of the bullet of the gun
    /// </summary>
    public float GetSpeed()
    {
        return bulletSpeedModifier;
    }
    /// <summary>
    /// Returns the angle of the gun
    /// </summary>
    public float GetAngle()
    {
        return bulletMaxAngle;
    }
    /// <summary>
    /// Returns the magazine size of the gun
    /// </summary>
    public int GetMagazine()
    {
        return magazineSize;
    }
    /// <summary>
    /// Returns the time needed to reload the gun and set it back to a full magazine
    /// </summary>
    public float GetReloadSpeed()
    {
        return reloadSpeed;
    }
    /// <summary>
    /// Returns if the gun has randomized angles that will be inbetween the positive and negative of the angle of the gun
    /// </summary>
    public bool isRandomAngles()
    {
        return randAngles;
    }
    /// <summary>
    /// Returns the speed at which bullets will be shot at EX: (BASE * BulletSpeed) -+ randSpeed;
    /// </summary>
    public float GetSpeedRandomizer()
    {
        return randSpeed;
    }
    /// <summary>
    /// Resets the angle of the gun
    /// </summary>
    /// <returns></returns>
    public void AngleReset()
    {
        lastBulletRot = 0;
        lastBulletDown = false;
    }
    #endregion
    
}

using System.Collections;
using UnityEngine;

[System.Serializable]
public class PlayerController : MonoBehaviour
{
    public static GameObject pGameObject;
    public static PlayerController script;

    private Rigidbody2D rb;

    private Camera mainCamera;

    private Animator playerAnimator;

    [Header("Player Stats")]
    [Min(0)]
    //Character Stats
    public float health;
    public float maxHealth, delayedHealth;//Delayed health is the health that can be recovered when the player gets a glory kill

    public float damageDelay;//How many seconds should be between the health damage and delayed health damage
    private Queue damageQueue = new Queue();//The queue for how much damage the players delayed healthbar will recieve
    public bool inDeathState = false; //When the player is in death state they cant talk to npc but their abilities are enhanced

    public int coins;           //Coins the player has collected to buy things with
    public int upgradesLeft;    //Upgrades left to upgrade
    private bool isInvincible;

    public int bullAmt;         //Bullets left in the current magazine
    public int[,] gunBullets = 
                                //first is bullets left in chamber, second is bullets left total, third is if the gun is usable
    {
        {100,7},
        {300,30},
        {100,5},
        {100,6 }
    };
    [Space(10)]
    //Multiplier Stats
    public float damageMulti;   //Should increase the damage of each bullet
    public float healthMulti;   //Current Calls are in Damage() and CharacterGloryKills()
    public float speedMulti;
    public float energyMulti;
    public float reloadSpeedMulti;//Should increase the speed at which the character reloads/ reduces reload time

    [Header("Movement")]
    //Movement Variables
    public bool canMove;        //Allows the player to move
    public bool canDoAnything;  //Allows the player to make any action
    public bool canCamMove;     //Allows the camera to move

    private float playerSpeed;
    private float horizontalInput;

    [Header("Dash Variables")]
    //Dash Variables
    private bool isDashing;
    private bool canDash;
    public float dashSpeed;

    public float dashDelay;
    private bool dashCooldownComplete;

    public float dashTime;
    private float currDashTime;
    private int dashDirection;

    [Header("Jump")]
    //Jump Variables
    public LayerMask whatIsGround;
    public bool isGrounded;
    private bool isJumping;
    private bool stoppedJumping;//wont reactivate until ground is touched again
    private float jumpLength;           //How long the player can hold the jump button to impact their jump ability difference.
    private float currJumpTime;

    private float jumpHeight;
    private float yVelocityLimit;        //How fast the character is allowed to fall

    [Header("Gun Variables")]
    //Gun Variables
    public string currGun;
    public GameObject playerGun;
    public Gun playerGunStats;
    private int gunSelected;

    //Point and Shoot Variables
    public GameObject crosshairs;
    public GameObject gunHolder;   //Holds All bullets in the game to avoid clutter for testing
    public GameObject allBulletsHolder;

    private Vector3 target;
    public Transform bulletStart;

    private bool isShooting;
    private float shotCurrTime;

    private bool isReloading;
    private bool reloadInput;
    [Header("Shield and Energy")]
    //Defense Variables
    public GameObject shield;

    private bool isDefending;
    private bool isDefenseUp;

    //Energy Variables
    private bool genEnergy, energyCorRunning;

    public int energy, maxEnergy;



    ///ThePlan
    ///
    /// get timed doors to close when time is reached
    /// Change guns to not destroy each swap
    /// add reload time bar to reload
    /// change shield to be based on damage taken
    /// add action for kill on glory kill
    /// 
    ///
    /// Finish up current Dialogue starter
    /// ALL ENEMIES SHOULD have a skull
    /// 
    /// make a top bonker that when the player touches the ceiling in jump stop his ascension and begin descending
    /// When the player stops jumping they should stop going up mid air.
    /// add a double jump
    /// make camera areas that make the camera adjust size to expand and shrink in the level  and go back to normal when leaving the area
    /// 
    /// Make an enemy that patrols between two points and stops to shoot at player when near
    /// make an enemy that self destructs unless killed by the player to discourage some parts of glory kills
    /// Make an enemy that goes for close engagements that encourages player glory kills by killing it to restore health
    /// 
    /// make an enemy that has a burst attack to get the player to dodge more
    ///Make a enemy that walks like a humanoid that has hearing(optional) and has a retreat function,reload function(optional),
    ///     move forward, and patrol
    ///make 2 enemies (one move back and forth another move another move in the air)
    ///
    ///                                                          Add hp bar that has health lost shown and can be regained when the player performs a glory kill
    ///Use GameEvents.cs use a timer to make world time events.
    ///make grenade launcher have an explosion with area of effect 
    ///add trapdoors to wind zones that open them up when a player pushes a button and closes some doors at
    ///     the end of the vent woosh depending on the circumstances
    /// 
    /// Glory Kill Ideas
    ///     Dash into enemy- Charges and shoulder bashes the enemy
    ///     From above- Stomp with foot
    ///     walking upto-Ripping skull
    /// 
    /// 
    /// 
    /// when the player takes a hit have a delayed health bar that will deplete after a set time in which the player will
    /// When the player dies use the delayed health as a reserve to replace the current health and it will deleplete at a rate of 5 health per second and take on enemy damage
    /// 
    ///create a main menu
    ///add dialogue menu
    ///make a upgrade menu
    ///
    ///create sounds
    ///
    ///Create a tech demo
    ///
    ///make part of the final level
    ///add lightweight render pipline lights
    ///Create a dark area in the map

    private void Awake()
    {
        pGameObject = this.gameObject;
        script = this;
    }
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();

        playerAnimator = this.GetComponent<Animator>();

        delayedHealth = health;



        Cursor.visible = false;

        mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();

        ///These are Player variables that affect their movment and all
        playerSpeed = 18f;//16
        jumpHeight = 32f;
        yVelocityLimit = -20f;
        jumpLength = 0.3f;


        bulletStart = playerGun.transform.Find("BulletStart");
        playerGunStats = playerGun.GetComponent<Gun>();

        currGun = playerGunStats.GetName();

        canMove = true;
        canDoAnything = true;
        canCamMove = true;

        dashCooldownComplete = true;
    }

    // Update is called once per frame and gets the inputs of each button if it is pressed
    void Update()
    {
        PlayerInputs();
        //Declares where the mouse is in the world and saves it to target
        target = mainCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, transform.position.z));

        //Sets the Crosshair to the mouse position and clamps it to the camera edges
        Vector2 crosshairClamp = new Vector2
            (Mathf.Clamp(target.x,CameraFollow.mainCam.GetHorzExtentMin(), CameraFollow.mainCam.GetHorzExtentMax()),
            Mathf.Clamp(target.y, CameraFollow.mainCam.GetVertExtentMin(), CameraFollow.mainCam.GetVertExtentMax()));



        crosshairs.transform.position = crosshairClamp;

    }
    //All the Inputs that the players can press for now
    private void PlayerInputs()
    {
        if(Input.GetAxisRaw("Jump") != 0)
            isJumping = true;//Means is attempting to jump
        else
        {
            isJumping = false;// Isnt attempting to jump
            stoppedJumping = true;//has stopped trying to jump
        }
        if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            dashDirection = (int)horizontalInput;
            isDashing = true;
        }

        horizontalInput = Input.GetAxisRaw("Horizontal");
        playerAnimator.SetBool("Idle", horizontalInput == 0f);
        playerAnimator.SetFloat("PlayerDirection", horizontalInput);
        if(Input.GetAxisRaw("Shoot") != 0)
            isShooting = true;
        else
            isShooting = false;

        if(Input.GetAxisRaw("Reload") != 0)
            reloadInput = true;
        else
            reloadInput = false;

        if(Input.GetMouseButtonDown(1))
            isDefending = true;
        else if(Input.GetMouseButtonUp(1))
            isDefending = false;
        if(!isReloading)
        {
            if(Input.GetKeyDown(KeyCode.Alpha1))
            {//Clean this shit up V
                GunInput(0);
            }
            else if(Input.GetKeyDown(KeyCode.Alpha2))
            {//Clean this shit up V
                GunInput(1);
            }
            else if(Input.GetKeyDown(KeyCode.Alpha3))
            {//Clean this shit up V
                GunInput(2);
            }
            else if(Input.GetKeyDown(KeyCode.Alpha4))
            {//Clean this shit up V
                GunInput(3);
            }
        }
    }
    void GunInput(int input)
    {
        gunBullets[gunSelected, 1] = bullAmt;
        gunSelected = input;
        GunSwap();
    }
    private void FixedUpdate()
    {
        if(canDoAnything)
        {
             if(canMove)
            {
                if(isDashing)
                {
                    if(dashDirection == 0)
                        dashDirection = (int)this.transform.localScale.x;
                    currDashTime += Time.fixedDeltaTime;
                    if(currDashTime >= dashTime)
                    {
                        currDashTime = 0f;
                        isDashing = false;
                    }
                    else if(canDash && dashCooldownComplete)
                    {
                        StartCoroutine(DashCooldown());
                        canDash = false;
                    }
                    rb.velocity = new Vector2(dashDirection * dashSpeed, 0f);
                }
                else
                {
                    Jump();
                    Movement();
                }
            }
            DefenseUp();
            PointAndShoot();
            Energy();
        }
    }
    #region All Tasks Used Each FixedUpdate
    //Moves left and right with the playersed each Update
    void Movement()
    {
        rb.velocity = new Vector2((horizontalInput * playerSpeed) * speedMulti, rb.velocity.y);

        ///This part of the code makes it to where the character turns left or right
        if(horizontalInput > 0f)
           transform.localScale = new Vector2(1f, 1f);
        else if(horizontalInput < 0f)
            transform.localScale = new Vector2(-1f, 1f);
    }
    //The Code that runs the jumping and falling velocities
    void Jump()
    {
        if(isGrounded)
        {
            canDash = true;
            stoppedJumping = false;
            currJumpTime = 0f;
        }


        if(isJumping && ( isGrounded || ( currJumpTime <= jumpLength && !stoppedJumping ) )) // Means if is attempting to jump and (is grounded or still holding down jump and hasnt let go)
        {
            if(currJumpTime == 0f)//Just started jumping off the floor or touched the ground again
            {
                isGrounded = false;
                playerAnimator.SetTrigger("JumpTrigger");
            }
            currJumpTime += Time.deltaTime;
            rb.velocity = new Vector2(rb.velocity.x, jumpHeight);
        }
        if(rb.velocity.y < yVelocityLimit)//The Limit on how fast the character can fall
            rb.velocity = new Vector2(rb.velocity.x, yVelocityLimit);
    }
    //Puts up the shield of the character or what ever defense option they have
    void DefenseUp()
    {
        if(isDefending && energy >= 20)
        {
            isDefenseUp = true;
            shield.SetActive(true);
        }
        else if(!isDefending || energy <= 0)
        {
            shield.SetActive(false);
            isDefenseUp = false;
        }
        if(isDefenseUp)
        {
            removeEnergy(1);
        }
    }
    //Turns the Gun in the direction of the crosshair and calculates all of the bullets used
    void PointAndShoot()
    {
        //finds the difference between the target and the gun holder
        //and making it only the difference between them and not the entire world and is multiplied by local scale to correct it if its flipped
        Vector2 difference = ( target - gunHolder.transform.position ) * this.transform.localScale.x;

        //Calculates the rotation of the gun and converts to degrees
        float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;

        //Rotates the gun in the direction the crosshair is
        gunHolder.transform.rotation = Quaternion.Euler(0f, 0f, rotationZ);
        //Keeps Gun from flipping upside down
        if(gunHolder.transform.rotation.z >= 0.7f || gunHolder.transform.rotation.z <= -0.7f)
            playerGun.transform.localScale = new Vector2(playerGun.transform.localScale.x, -this.transform.localScale.y);
        else
            playerGun.transform.localScale = new Vector2(playerGun.transform.localScale.x, this.transform.localScale.y);

        //Should only shoot if the person is trying to shoot,cooldown over, and isnt reloading or defending
        if(isShooting && shotCurrTime >= playerGunStats.GetCooldown() && !isDefenseUp && !isReloading && bullAmt > 0)
        {
            object[,] temp = playerGunStats.CalculateBullets(rotationZ);

            for(int i = 0; i < temp.Length / 2; i++)
            {
                fireBullet((float)temp[i, 0], (Vector2)temp[i, 1]);
            }
            bullAmt--;
            shotCurrTime = 0f;
        }
        if(!isReloading && ( bullAmt <= 0 || reloadInput ) && gunBullets[gunSelected,0] > 0)//makes sure the character isnt already reloading, is at 0 bullets or hit the reload button and has bullets left
            StartCoroutine("Reload");
        shotCurrTime += Time.deltaTime;
    }
    //Creates Energy each Update
    void Energy()
    {
        if(genEnergy)
            energy++;
        if(energy > 100) energy = 100;
        if(energy < 0) energy = 0;
    }
    #endregion
    ///All Methods Below are related to fixed update but are not the actual commands themselves
    ///Refer to All Tasks Used Each FixedUpdate for the Actual Methods themselves
    #region Energy Related Methods
    public void removeEnergy(int energyAmount)
    {
        energy -= energyAmount;
        if(energyCorRunning)
        {
            StopCoroutine("EnergyPause");//resets the coroutine if it is running
        }
        StartCoroutine("EnergyPause");
    }
    IEnumerator EnergyPause()
    {
        genEnergy = false;
        energyCorRunning = true;
        yield return new WaitForSeconds(1f);
        genEnergy = true;
        energyCorRunning = false;
    }
    #endregion
    #region Defense Related Methods

    //Maybe repurpose as a different thing
    IEnumerator RunDefense()
    {
        shield.SetActive(true);
        isDefenseUp = true;
        yield return new WaitForSeconds(3.0f);
        shield.SetActive(false);
        isDefenseUp = false;
    }
    #endregion
    #region Gun Related Methods
    void fireBullet(float bulletAngle, Vector2 direction)
    {
        GameObject b = Instantiate(BulletCollection[gunSelected], bulletStart.position, Quaternion.Euler(0f, 0f, bulletAngle)) as GameObject;

        //Will add the speed randomizer if the bullet randomizer speed isnt 0
        if(playerGunStats.GetSpeedRandomizer() == 0)
            b.GetComponent<Rigidbody2D>().velocity = direction * playerGunStats.GetSpeed() * this.transform.localScale.x;
        else
            b.GetComponent<Rigidbody2D>().velocity = direction * ( playerGunStats.GetSpeed() + Random.Range(-playerGunStats.GetSpeedRandomizer(), playerGunStats.GetSpeedRandomizer()) ) * this.transform.localScale.x;

        BulletScript bScript = b.GetComponent<BulletScript>();
        if(bScript == null)
        {
            Debug.Log("ran GrenadeShot");
            GrenadeBulletScript bGScript = b.GetComponent<GrenadeBulletScript>();
            bGScript.isPlayerBullet = true;
            bGScript.SetDamage(playerGunStats.GetDamage());
        }
        else
        {
            bScript.isPlayerBullet = true;
            bScript.SetDamage(playerGunStats.GetDamage());
        }
        b.transform.parent = allBulletsHolder.transform;
        b.transform.localScale = new Vector2(this.transform.localScale.x * b.transform.localScale.x, b.transform.localScale.y);
    }
    public void Damage(float damageTaken)
    {
        if(!isInvincible)
        {
            if(health > 0)
            {
                //The Player has taken damage
                
                damageQueue.Enqueue(damageTaken);
                StartCoroutine("PlayersDelayedHealth");
                
                health -= damageTaken;
            }
            else//The player is in death state where they can save themselves by killing for a revive
            {
                inDeathState = true;//should have a speed boost when in death state
                speedMulti = 1.5f;
                delayedHealth -= damageTaken;
            }
        }

        if(health <= 0 && delayedHealth <= 0)
        {
            CharacterDies();
        }
    }
    public void Heal(float hpHealed)
    {
        //Maximum amount the heal can provide(doesn't go over max health
        health = Mathf.Clamp(hpHealed + health, 0f, maxHealth);
        delayedHealth = Mathf.Clamp(hpHealed + delayedHealth, 0f, maxHealth);
    }
    public void CharacterDies()//Set up for the glory kill mechanic to when they die make them transparent until they glory kill something
    {
        this.crosshairs.SetActive(false);
        CameraFollow.mainCam.SetCamToLastPosition();
        canCamMove = false;
        Cursor.visible = true;


        GameEvents.current.Death();

    }
    //Should be able to swap between guns
    void GunSwap()
    {
        Destroy(playerGun);

        if(GunCollection.Length <= gunSelected)
            Debug.Log("Failed to get a gun from the collection");
        else
            //Using the form like this since its creating an extra when you dont do it this way and tries destroying the extra instead of this one.
            playerGun = Instantiate(GunCollection[gunSelected], gunHolder.transform);
        bulletStart = playerGun.transform.Find("BulletStart");
        playerGunStats = playerGun.GetComponent<Gun>();
        currGun = playerGunStats.GetName();

        bullAmt = gunBullets[gunSelected, 1];//Should switch back to the bullets the player had originally
    }

    [SerializeField]
    private GameObject[] GunCollection;//contains the guns
    [SerializeField]
    private GameObject[] BulletCollection;//Contains the bullets used for the guns

    public void PlayerGloryKilled()
    {
        Debug.Log("Player Glory Killed!");
        health = delayedHealth;
        inDeathState = false;
        speedMulti = 1f;//Speed goes back to normal speed;
        damageQueue.Clear();//Check gamemaking bookmark folder for current problem solution
        StopCoroutine("PlayersDelayedHealth");
    }
    IEnumerator PlayersDelayedHealth()
    {
        yield return new WaitForSeconds(damageDelay);
        delayedHealth -= (float)damageQueue.Dequeue();
    }

    //Makes the Character Invincible for the wait for seconds
    IEnumerator PlayerIsInvincible()
    {
        Debug.Log("Player is Invincible");
        isInvincible = true;
        yield return new WaitForSeconds(1.25f);
        Debug.Log("Player isn't Invincible");
        isInvincible = false;
    }
    //Reloads the gun after either running out of bullets or pressing reload
    IEnumerator Reload()
    {
        isReloading = true;//Keeps from changing weapons

        playerGunStats.AngleReset();//Resets the angle when reloading
        yield return new WaitForSeconds(playerGunStats.GetReloadSpeed());
        if(gunSelected != 0)
        {
            int tempBullets = bullAmt;//Should contain bullets in magazine before hand to ensure proper reload amount
            bullAmt = playerGunStats.GetMagazine();//Makes the amount in the magazine used as much as it should be filled.

            gunBullets[gunSelected, 0] -= ( playerGunStats.GetMagazine() - tempBullets );//This section should allow the gun to derive its bullets from the full amount it has and if there isnt enough bullets it should give the amount left
            if(gunBullets[gunSelected, 0] < 0)//If the players bullets not in chamber are less than 0
            {
                bullAmt += gunBullets[gunSelected, 0];
                gunBullets[gunSelected, 0] = 0;
            }
        } else
        {
            bullAmt = playerGunStats.GetMagazine();
        }
        isReloading = false;
    }
    #endregion

    public void getCollectableType(CollectableType type,float typeAmount) 
    {
        switch(type)
        {
            case CollectableType.Ammo:
                for(int i = 0; i < gunBullets.Length / 2; i++)
                    gunBullets[i, 0] += Mathf.RoundToInt(typeAmount);
                break;
            case CollectableType.Coin:
                coins += (int)typeAmount;
                break;
            case CollectableType.Health:
                Heal(typeAmount);
                break;
            case CollectableType.Upgrade:
                upgradesLeft += (int)typeAmount;
                break;
        }
    }
    IEnumerator DashCooldown()
    {
        dashCooldownComplete = false;
        yield return new WaitForSeconds(dashDelay);
        dashCooldownComplete = true;
    }
}

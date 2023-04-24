using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum CollectableType
{
    Ammo,
    Health,
    Upgrade,
    Coin
}
public class Collectable : MonoBehaviour
{
    private Rigidbody2D rb;

    //What type of colletable it should be
    public CollectableType type;
    //how much of the type there should be.
    public int typeAmount;

    public bool hasGravity;
    public bool randomJumpDrop;//Causes the collectable to jump in a direction 
    void Start()
    {
        if(hasGravity)
            rb = this.GetComponent<Rigidbody2D>();
        if(randomJumpDrop)
            rb.velocity = new Vector2(Random.Range(-5f, 5f), Random.Range(0f, 10f));
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(hasGravity)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y - 0.6f);
            if(rb.velocity.y < -22.5f)
                rb.velocity = new Vector2(rb.velocity.x, -22.5f);
        }
    }
    //Should be if the user gets in range to pick up the collectable
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            //Debug.Log("Gave Player Collectable");

            PlayerController.script.getCollectableType(type, typeAmount);
            Destroy(this.gameObject);
        }
    }
}

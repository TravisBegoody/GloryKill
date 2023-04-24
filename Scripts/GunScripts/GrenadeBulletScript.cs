using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeBulletScript : BulletScript
{
    private Rigidbody2D rb;


    [SerializeField] private GameObject bulletShrapnel;
    [SerializeField] private GameObject explosionParticles;

    void Start()
    {

        rb = this.GetComponent<Rigidbody2D>();

        StartCoroutine("explosion");
    }
    private void FixedUpdate()
    {
        rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y - 0.6f);
        if(rb.velocity.y < -22.5f)
            rb.velocity = new Vector2(rb.velocity.x, -22.5f);
    } 
    private void OnTriggerEnter2D(Collider2D other)
    {
      //  if(other.tag == "Platform" || other.tag == "PlayerShield")
    //    {
  //          Instantiate(collisionParticles,this.transform.position, Quaternion.identity,this.transform.parent);
//
       //     Destroy(this.gameObject);
       // }
       // else if(other.tag == "Player" && !isPlayerBullet)//these are enemy bullets
       // {
        //    PlayerController.script.Damage(bulletDamage);
        //    Destroy(this.gameObject);
        //}
        //else
        if(other.tag == "Enemy" && isPlayerBullet)//These are player bullets
        {
            object[,] temp = CalculateBullets(0f, 9f, 40);
            for(int i = 0; i < temp.Length / 2; i++)
            {
                fireBullet((float)temp[i, 0], (Vector2)temp[i, 1]);
            }
            Destroy(this.gameObject);
        }
    }
    void fireBullet(float bulletAngle, Vector2 direction)
    {
        GameObject b = Instantiate(bulletShrapnel, this.transform.position, Quaternion.Euler(0f, 0f, bulletAngle)) as GameObject;
        float speed = 60f; //Should be changed to work with the randomizer eventually

        b.GetComponent<Rigidbody2D>().velocity = direction * speed * this.transform.localScale.x;
        
        BulletScript bScript = b.GetComponent<BulletScript>();
        bScript.isPlayerBullet = true;
        b.transform.parent = this.transform.parent;
        b.transform.localScale = new Vector2(this.transform.localScale.x * 2, b.transform.localScale.y);
    }
    ///should for now shoot out 20 bullets in a circle
    ///SHOULD BE 40 bullets at a 9 degrees
    IEnumerator explosion()
    {
        object[,] temp = CalculateBullets(0f, 9f, 40);// shoots a circle
        yield return new WaitForSeconds(destroyTime);
        Instantiate(explosionParticles, this.transform.position, Quaternion.identity, this.transform.parent);
        Instantiate(collisionParticlesArray[0], this.transform.position, Quaternion.identity, this.transform.parent);
        for(int i = 0; i < temp.Length / 2; i++)
        {
            fireBullet((float)temp[i, 0], (Vector2)temp[i, 1]);
        }
        Destroy(this.gameObject);
    }
}

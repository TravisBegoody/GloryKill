using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : Gun
{
    public float destroyTime;
    public bool isPlayerBullet;//Says if the bullet is player shot or not

    //0 - enviorment
    //1 - player hit
    //2 - enemy hit
    public GameObject[] collisionParticlesArray;
    void Start()
    {
        Destroy(this.gameObject, destroyTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Platform" || other.tag == "PlayerShield")
        {
            if(collisionParticlesArray[0] != null)
                Instantiate(collisionParticlesArray[0],this.transform.position, Quaternion.identity,this.transform.parent);

            Destroy(this.gameObject);
        }
        else if(other.tag == "Player" && !isPlayerBullet)//these are enemy bullets
        {
            PlayerController.script.Damage(gunDamage);
            if (collisionParticlesArray[1] != null)
                Instantiate(collisionParticlesArray[1], this.transform.position, Quaternion.identity, this.transform.parent);

            Destroy(this.gameObject);
        }
        else if(other.tag == "Enemy" && isPlayerBullet)//These are player bullets
        {
            other.GetComponent<EnemyScript>().Damage(gunDamage);
            if (collisionParticlesArray[2] != null)
                Instantiate(collisionParticlesArray[2], this.transform.position, Quaternion.identity, this.transform.parent);
            //Debug.Log("EnemyHit");
            Destroy(this.gameObject);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    int platCurrOn;//How many platforms the player is currently on
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(PlayerController.script.whatIsGround == ( PlayerController.script.whatIsGround| (1 <<other.gameObject.layer)))
        {
            platCurrOn++;
            //Debug.Log("Touched Ground");
            PlayerController.script.isGrounded = true;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if(PlayerController.script.whatIsGround == ( PlayerController.script.whatIsGround | ( 1 << other.gameObject.layer ) ) )
        {
            platCurrOn--;
            //Debug.Log("Exited Ground");
            if(platCurrOn < 1)
                PlayerController.script.isGrounded = false;
        }
    }
}

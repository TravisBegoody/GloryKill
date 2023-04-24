using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyScript : MonoBehaviour
{
    [SerializeField]
    CreatureData enemyData;

    private float health;
    public float currHealth { get { return health; } set { health = value; } }

    private Text debugText;//Should show the health for now but should have categories.
    void Start()
    {
        currHealth = enemyData.Health;
        foreach(Transform tr in this.transform)
        {
            if(tr.tag == "DebugText")
            {
                debugText = tr.GetComponent<Text>();
            }
        }
        if(GameEvents.current.debugEnemyHealth)
            debugText.text = "Health = " + currHealth;
        else
            debugText.gameObject.SetActive(false);
    }
    /// 
    /// Add a thing where you can shoot multiple and change the angle difference in them
    /// Add a bullet type that explodes into multiple bullets

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Damage(float damageTaken)
    {
        currHealth -= damageTaken;
        if(GameEvents.current.debugEnemyHealth)
            debugText.text = "Health = " + currHealth;
        if(currHealth <= 0)
        {
            IsGloryKilled();
            Destroy(this.gameObject);
        } 
    }
    public void IsGloryKilled()//Runs when the enemy has been glory killed
    {
        PlayerController.script.PlayerGloryKilled();
        Destroy(this.gameObject);
    }
}

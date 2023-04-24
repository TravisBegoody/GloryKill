using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    GameObject player;
    PlayerController playerScript;

    [SerializeField]
    RectTransform healthBar;

    [SerializeField]
    RectTransform delayedHealthBar;

    [SerializeField]
    GameObject healthTextHolder;
    Text healthText;

    [SerializeField]
    GameObject energyTextHolder;
    Text energyText;

    [SerializeField]
    GameObject bulletTextHolder;
    Text bulletText;

    [SerializeField]
    GameObject coinTextHolder;
    Text coinText;

    [SerializeField]
    GameObject currGunTextHolder;
    Text CurrGunText;

    [SerializeField]
    GameObject eachGunBulletsTextHolder;
    Text eachGunBulletsText;

    [SerializeField]
    GameObject timeScaleTextHolder;
    Text timeScaleText;

    [SerializeField]
    GameObject slider;
    Slider sliderValue;

    [SerializeField]
    GameObject timerTextHolder;
    Text timerText;

    void Start()
    {
        playerScript = player.GetComponent<PlayerController>();

        healthText = healthTextHolder.GetComponent<Text>();
        energyText = energyTextHolder.GetComponent<Text>();
        bulletText = bulletTextHolder.GetComponent<Text>();
        coinText = coinTextHolder.GetComponent<Text>();

        CurrGunText = currGunTextHolder.GetComponent<Text>();
        eachGunBulletsText = eachGunBulletsTextHolder.GetComponent<Text>();
        timeScaleText = timeScaleTextHolder.GetComponent<Text>();

        sliderValue = slider.GetComponent<Slider>();

        timerText = timerTextHolder.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        healthBar.transform.localScale = new Vector2(playerScript.health / playerScript.maxHealth, 1f);
        delayedHealthBar.transform.localScale = new Vector2(playerScript.delayedHealth / playerScript.maxHealth, 1f);


        healthText.text = "Health " + playerScript.health + " / "+ playerScript.maxHealth;
        energyText.text = "Energy " + playerScript.energy + " / " + playerScript.maxEnergy;
        bulletText.text = "Bullets " + playerScript.bullAmt + " / " + playerScript.playerGunStats.GetMagazine();
        coinText.text = "Coins " + playerScript.coins;

        CurrGunText.text = "Current Gun " + playerScript.currGun;
        eachGunBulletsText.text = "Pistol Rifle Shotgun Grenade \n" +// This looks so bad find a way to make it look better as code
                                  playerScript.gunBullets[0, 0] + " " + playerScript.gunBullets[1, 0] + " " + playerScript.gunBullets[2, 0] + " " + playerScript.gunBullets[3, 0] + " \n" +
                                  playerScript.gunBullets[0, 1] + " " + playerScript.gunBullets[1, 1] + " " + playerScript.gunBullets[2, 1] + " " + playerScript.gunBullets[3, 1] ;
        timeScaleText.text = "Current Time Scale " + sliderValue.value;
        Time.timeScale = sliderValue.value;

        float timer = GameEvents.current.timer;
        string minutes, seconds, milliseconds;
        if((timer / 60f % 60) < 10)
            minutes =  "0" + ((int)(timer / 60f % 60 ) ).ToString();
        else
            minutes =  ((int)(timer / 60f % 60 )).ToString();

        if(timer % 60 < 10)
            seconds = "0" + ((int)( timer % 60)).ToString();
        else
            seconds = ((int)( timer % 60)).ToString();

        milliseconds = ((int)( timer * 100f % 100)).ToString();

        timerText.text = "Time " + minutes + ":" + seconds + "." + milliseconds;//Change to timer with seconds minutes and hours
    }
}
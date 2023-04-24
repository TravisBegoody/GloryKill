using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;

public class GameEvents : MonoBehaviour
{
    public static GameEvents current;

    public float timer;

    public bool debugEnemyHealth;//Shows the enemy health when enabled
    public bool debugUI;//Shows the player ui when enabled

    private void Awake()
    {
        current = this;
    }
    public void Update()
    {
        timer += Time.deltaTime;
    }

    public event Action<int> onButtonTriggerPress;
    //Activates the id that is connected to this id and triggers it
    public void ButtonTriggerPress(int id)
    {
        onButtonTriggerPress(id);
    }
    public event Action<int> onDoorTrigger;
    public void doorTrigger(int id)
    {
        onDoorTrigger(id);
    }
    public void Death()
    {
        StartCoroutine(DelayedLoad());
    }
    IEnumerator DelayedLoad()
    {
        yield return new WaitForSeconds(5f);

        SceneManager.LoadScene("TestScene");
    }
}

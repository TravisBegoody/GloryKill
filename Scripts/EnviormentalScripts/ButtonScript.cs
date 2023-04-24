using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScript : MonoBehaviour
{
    /// <summary>
    /// The Button should have a function when pushed whether it be giving a tutorial pop up
    /// a platform interaction or door opening it should be doing something.
    /// </summary>
    enum ButtonType
    {
        Spawner,//Spawns something in.
        Elevator,//Moves a platform between two different points
        Door,//Should Open a door
        Merchant,// Start a conversation with a merchant.
    }

    public int id;

    [SerializeField] private ButtonType type;
    [SerializeField] private GameObject controlledObject;

    bool canPushButton;

    void Start()
    {
        GameEvents.current.onButtonTriggerPress += OnButtonPress;
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.E) && canPushButton)
        {
            GameEvents.current.ButtonTriggerPress(id);
        }
    }
    /// <summary>
    /// GameEvents Calls this id and checks if its a spawner or elevator.
    /// </summary>
    /// <param name="id">The ID of the current object</param>
    private void OnButtonPress(int id)
    {
        if(id == this.id)
        switch(this.type)
        {
            case ButtonType.Spawner:
                SpawnOnButtonPush();
                break;
            case ButtonType.Elevator:
                MoveElevatorOnPush();
                break;
            default:
                Debug.Log("Hold up" + this.gameObject.ToString() + " had an oopsie");
                break;
        }
    }
    private void OnDestroy()
    {
        GameEvents.current.onButtonTriggerPress -= OnButtonPress;
    }
    void SpawnOnButtonPush()
    {
        Transform childTransform = this.transform;//If it spawns on the button unless assigned then we know that there is an error
        foreach(Transform tr in this.transform) {
            if(tr.tag == "ButtonSpawn")
                childTransform = tr;
        }
        Instantiate(controlledObject, childTransform.position, Quaternion.identity);
    }
    void MoveElevatorOnPush()
    {
        WaypointPlatformScript platformScript = controlledObject.GetComponent<WaypointPlatformScript>();
        platformScript.startMoving();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            canPushButton = true;
        }   
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            canPushButton = false;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedDoor : DoorScript
{
    public float timeToBreak;//What time will the door stop functioning and be broken
    private bool ranBreakSequence; // Runs when the timeToBreak has just been reached and the door is now broken

    public GameObject SparksParticles;//The particles that are instansiated when the door breaks

    /// <summary>
    /// This Door breaks down after a certain time in the Game Events Script and wont open again after
    /// </summary>
    void FixedUpdate()
    {
        if(GameEvents.current.timer <= timeToBreak || isClosed)
            doorUpdate();
        else if(!ranBreakSequence)
        {
            ranBreakSequence = true;
            SparksParticles.SetActive(true);
        }
    }
        ///Should always be added to doors with the DoorScript already
        ///this door shouldn't open after a certain gameEvents.Timer.time
        ///this door will emit particles when broken
}

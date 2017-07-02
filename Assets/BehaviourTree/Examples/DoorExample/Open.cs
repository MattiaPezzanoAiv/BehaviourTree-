using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mattia;

public class Open : Node
{

    public override NodeState Run(Agent agent)
    {
        Door door = FakeWorld.GetNearestObjectOfType<Door>(agent);
        if (door == null)
            return NodeState.Aborted; //there is no door(1)
        if (Vector3.Distance(agent.transform.position, door.transform.position) > 0.5f)
            return NodeState.Aborted; //door is too far away(2)
        if (door.locked)
            return NodeState.Aborted; //door locked(3)
                                      //if (!agent.hasKey)
                                      //    return NodeState.Aborted; //have no keys available(3)

        //at this point i'm sure that: there is a door(1), the door is in range(2) and door is no lcoked(3), i can open door.
        door.GetComponent<Renderer>().material.color = Color.green;
        Debug.Log("Door opened without key");
        return NodeState.Done;
    }
}

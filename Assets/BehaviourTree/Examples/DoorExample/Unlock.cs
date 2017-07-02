using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mattia;

public class Unlock : Node {

    public override NodeState Run(Agent agent)
    {
        Door door = FakeWorld.GetNearestObjectOfType<Door>(agent);
        if (door == null)
            return NodeState.Aborted; //there is no door(1)
        if (Vector3.Distance(agent.transform.position, door.transform.position) > 0.5f)
            return NodeState.Aborted; //door is too far away(2)
        if (!agent.hasKey)
            return NodeState.Aborted; //have no keys available(4)
        if (!door.locked)
            return NodeState.Aborted; //door locked(3)

        door.locked = false;
        Debug.Log("Unlocked door with key");
        return NodeState.Done;
    }
}

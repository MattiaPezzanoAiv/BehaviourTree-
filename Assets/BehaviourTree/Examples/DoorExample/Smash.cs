using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mattia;

public class Smash : Node {

    public override NodeState Run(Agent agent)
    {
        Door door = FakeWorld.GetNearestObjectOfType<Door>(agent);
        if (door == null)
            return NodeState.Aborted; //there is no door(1)
        if (Vector3.Distance(agent.transform.position, door.transform.position) > 0.5f)
            return NodeState.Aborted; //door is too far away(2)


        door.locked = false;
        door.GetComponent<Renderer>().material.color = Color.red;
        Debug.Log("Door smashed");
        GameObject.Destroy(door);
        return NodeState.Done;
    }
}

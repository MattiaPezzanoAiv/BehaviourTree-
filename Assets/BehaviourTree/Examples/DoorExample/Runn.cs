using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mattia;

public class Runn : Node {

    public override NodeState Run(Agent agent)
    {
        Door door = FakeWorld.GetNearestObjectOfType<Door>(agent);
        if (door == null)
            return NodeState.Aborted; //there is no door(1)
        if (Vector3.Distance(agent.transform.position, door.transform.position) <= 0.5f)
            return NodeState.Aborted; //door is too far away(2)
        
        Vector3 dir = (door.transform.position - agent.transform.position).normalized;
        agent.transform.position += dir * 3f * Time.deltaTime;
        return NodeState.Running;
    }

}

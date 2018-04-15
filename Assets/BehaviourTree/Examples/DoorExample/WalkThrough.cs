using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mattia;

public class WalkThrough : Node {

    public override NodeState Run(Agent agent)
    {
        Debug.Log("door walked through");
        return NodeState.Done;
    }
}

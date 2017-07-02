using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mattia;

public class CantPass : Node {


    public override NodeState Run(Agent agent)
    {
        Debug.Log("Cant pass");
        return NodeState.Done;
    }
}

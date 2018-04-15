using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mattia
{
    public class Selector : Node
    {
        public override NodeState Run(Agent agent)
        {
            foreach (Node child in Children)
            {
                NodeState state = child.Run(agent);
                if (state == NodeState.Done || state == NodeState.Running)
                    return state;
            }
            return NodeState.Aborted;
        }
    }
}
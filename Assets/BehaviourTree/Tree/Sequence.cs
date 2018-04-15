using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mattia
{
    public class Sequence : Node
    {

        public override NodeState Run(Agent agent)
        {
            foreach (Node child in Children)
            {
                NodeState state = child.Run(agent);
                if (state == NodeState.Aborted)
                    return NodeState.Aborted;
            }
            return NodeState.Done;
        }
    }
}
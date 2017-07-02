using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mattia
{
    public class Chasing : Node
    {

        public override NodeState Run(Agent agent)
        {
            Collider[] cols = Physics.OverlapSphere(agent.transform.position, 10f);
            if (cols.Length > 0)
            {
                foreach (var c in cols)
                {
                    if (c.gameObject != agent.gameObject)
                    {
                        agent.target = c.gameObject.transform;
                        Debug.Log("Target gained");
                        return NodeState.Done;
                    }
                }
            }
            Debug.Log("Too long distance from target");
            return NodeState.Aborted;
        }

    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mattia
{
    public class Attacking : Node
    {

        private float t;
        private float f;
        private int counter;

        public override NodeState Run(Agent agent)
        {
            if (agent.target == null) return NodeState.Aborted;
            if (Vector3.Distance(agent.target.position, agent.transform.position) > 0.3f)
                return NodeState.Aborted;

            if (t > 0)
                t -= Time.deltaTime;
            if (t <= 0)
            {
                t = f;
                Debug.Log("Attack " + ++counter);
                return NodeState.Done;
            }
            return NodeState.Running;
        }

        public override void Init()
        {
            base.Init();
            f = 0.5f;
            t = f;
        }
    }
}

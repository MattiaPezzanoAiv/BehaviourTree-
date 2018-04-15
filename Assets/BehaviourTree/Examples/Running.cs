using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mattia
{
    public class Running : Node
    {
        private float speed;

        public override NodeState Run(Agent agent)
        {
            if (agent.target == null) return NodeState.Aborted; //no have target

            if (Vector3.Distance(agent.target.position, agent.transform.position) <= 0.3f)
            {
                Debug.Log("Arrived");
                return NodeState.Done;
            }
            else if (Vector3.Distance(agent.target.position, agent.transform.position) > 10f)
            {
                Debug.Log("Too distance");
                return NodeState.Aborted;
            }
            else
            {
                Vector3 direction = (agent.target.position - agent.transform.position).normalized;
                direction.y = 0;
                agent.transform.position += direction * speed * Time.deltaTime;
                Debug.Log("Running");
                return NodeState.Aborted;
            }
        }

        public override void Init()
        {
            speed = 3f;
        }
    }
}
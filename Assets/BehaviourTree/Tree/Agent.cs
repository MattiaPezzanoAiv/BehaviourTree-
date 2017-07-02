using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mattia
{
    public class Agent : MonoBehaviour
    {
        [SerializeField]
        private TextAsset xmlBehaviourTree;

        private Node agent;
        [HideInInspector]
        public Transform target;

        //implement dictionary of object for player customizable reference

        /// <summary>
        /// TEMPORARY VARIABLE USED FOR EXECUTE EXAMPLE SCRIPTS. IN A REAL PROJECT THIS MUST BE OVERRIDED WITH A MORE COMPLEX INVENTORY SYSTEM.
        /// </summary>
        public bool hasKey = false;

        // Use this for initialization
        void Start()
        {
            agent = BehaviourTreeUtils.LoadBehaviourTree(xmlBehaviourTree);
            if(agent == null)
            {
                Debug.LogError("Unable to parse XML file " + xmlBehaviourTree.name);
            }
            
        }

        // Update is called once per frame
        void Update()
        {
            agent.Run(this);
        }
    }
}

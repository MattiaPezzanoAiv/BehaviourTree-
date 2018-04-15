using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;

namespace Mattia
{

    public enum NodeState { Aborted, Running, Done }
    public class Node
    {
        public Node()
        {
            Children = new List<Node>();
            Init();
        }

        public List<Node> Children { get; set; }

        [XmlIgnore]
        public Node parent;

        public void AddChild(Node node)
        {
            Children.Add(node);
        }
        public int GetChildCount()
        {
            if (Children != null)
                return Children.Count;
            return 0;
        }
        /// <summary>
        /// Must be overrided, have the same function of Update() in MonoBehaviour. 
        /// </summary>
        /// <param name="agent"></param>
        /// <returns></returns>
        public virtual NodeState Run(Agent agent)
        {
            return NodeState.Aborted;
        }

        /// <summary>
        /// Called in constructor, default is empty.
        /// </summary>
        public virtual void Init()
        {
        }
    }
}

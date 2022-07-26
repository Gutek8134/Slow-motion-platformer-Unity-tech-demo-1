using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BehaviourTreeNS
{
    public enum NodeState{
        Running,
        Success,
        Failure
    }

    public class Node
    {
        protected NodeState state;

        public Node parent;
        protected List<Node> children = new List<Node>();

        private Dictionary<string, object> data = new Dictionary<string, object>();

        public Node()
        {
            parent = null;
        }

        public Node(List<Node> _children)
        {
            foreach (Node child in _children)
                _Attach(child);
        }

        private void _Attach(Node node)
        {
            node.parent = this;
            children.Add(node);
        }

        public void SetData(string key, object value)
        {
            data[key] = value;
        }

        public object GetData(string key)
        {
            if(data.TryGetValue(key, out object value))
                return value;
            
            if (parent != null)
            {
                return parent.GetData(key);
            }

            return null;
        }

        public bool ClearData(string key)
        {
            if(data.ContainsKey(key))
            {
                data.Remove(key);
                return true;
            }

            if (parent != null)
            {
                return parent.ClearData(key);
            }
            return false;
        }

        ///<summary>Runs the action node has to perform</summary>
        ///<returns>Current state of the node</returns>
        public virtual NodeState Run() => NodeState.Failure;
        public virtual NodeState FixedRun() => NodeState.Failure;
    }
}

using System.Collections.Generic;

namespace BehaviourTreeNS
{
    ///<summary>Runs all nodes until failure</summary>
    public class Sequence : Node {

        public Sequence() : base() {}
        public Sequence(List<Node> children) : base(children) {}

        ///<returns>State of the set of nodes:
        ///Failure if any failed to execute
        ///Running if any is running
        ///Success if there are none of the above</returns>
        public override NodeState Run()
        {
            bool anyChildrenRunning = false;

            foreach(Node child in children)
            {
                switch (child.Run())
                {
                    case NodeState.Failure:
                        state = NodeState.Failure;
                        return NodeState.Failure;
                    
                    case NodeState.Success:
                        continue;

                    case NodeState.Running:
                        anyChildrenRunning = true;
                        continue;
                    
                    default:
                        state = NodeState.Success;
                        return state;
                }
            }

            state = (anyChildrenRunning) ? NodeState.Running : NodeState.Success;
            return state;
        }
    }
}
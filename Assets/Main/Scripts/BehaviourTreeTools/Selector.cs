using System.Collections.Generic;

namespace BehaviourTreeNS
{
    ///<summary>Runs the first successful node</summary>
    public class Selector : Node {

        public Selector() : base() {}
        public Selector(List<Node> children) : base(children) {}

        ///<returns>State of the set of nodes:
        ///Failure if every node failed
        ///Running if any is running
        ///Success if any node succeeded</returns>
        public override NodeState Run()
        {
            foreach(Node child in children)
            {
                switch (child.Run())
                {
                    case NodeState.Failure:
                        continue;
                    
                    case NodeState.Success:
                        state = NodeState.Success;
                        return state;

                    case NodeState.Running:
                        state = NodeState.Running;
                        return state;
                    
                    default:
                        continue;
                }
            }

            state = NodeState.Failure;
            return state;
        }
    }
}
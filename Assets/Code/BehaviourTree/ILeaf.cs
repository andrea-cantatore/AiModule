using System;

namespace code.behaviourtree
{
    public interface ILeaf
    {
        Node.NodeState Process();
        void Reset()
        {
            //Implementation not needed like this cause i'm dumb (or smart, who knows)
        }
    }
    
    public class Condition : ILeaf
    {
        private Func<bool> _condition;
        public Condition(Func<bool> condition)
        {
            _condition = condition;
        }
        public Node.NodeState Process()
        {
            return _condition() ? Node.NodeState.SUCCESS : Node.NodeState.FAILURE;
        }
        public void Reset()
        {
        }
    }
}

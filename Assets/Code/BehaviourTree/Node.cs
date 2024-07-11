using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace code.behaviourtree
{

        #region BasicNode

    public class Node
    {
        public readonly string name;
        public readonly int priority;
        public readonly List<Node> Children = new List<Node>();
        protected int currentChild = 0;
        private Dictionary<string, object> _dataContext = new Dictionary<string, object>();
        public Node parent;

        public Node(string name = "Default Node", int priority = 0)
        {
            this.name = name;
            this.priority = priority;
            parent = null;
        }
        
        public void AddChild(Node child)
        {
            child.parent = this;
            Children.Add(child);
        }
        
        public void SetData(string key, object value)
        {
            _dataContext[key] = value;
        }
        
        public object GetData(string key)
        {
            object value = null;
            if(_dataContext.TryGetValue(key, out value))
            {
                return value;
            }
            
            Node node = this.parent;
            while (node != null)
            {
                if (node._dataContext.TryGetValue(key, out value))
                {
                    return value;
                }
                node = node.parent;
            }
            return null;
        }
        
        public bool ClearData(string key)
        {
            if(_dataContext.ContainsKey(key))
            {
                _dataContext.Remove(key);
                return true;
            }
            
            Node node = this.parent;
            while (node != null)
            {
                if (node._dataContext.ContainsKey(key))
                {
                    node._dataContext.Remove(key);
                    return true;
                }
                node = node.parent;
            }
            return false;
        }

        public virtual NodeState Process()
        {
            return Children[currentChild].Process();
        }
        
        public virtual void Reset()
        {
            currentChild = 0;
            foreach (var child in Children)
            {
                child.Reset();
            }
        }
        
        public enum NodeState
        {
            RUNNING,
            SUCCESS,
            FAILURE
        }
    }
    
        #endregion
        
        #region Decorators
    public class BehaviourTree : Node
    {
        public BehaviourTree(string name) : base(name)
        {
        }
        
        public override NodeState Process()
        {
            while (currentChild < Children.Count)
            {
                var state = Children[currentChild].Process();
                switch (state)
                {
                    case NodeState.RUNNING:
                        return NodeState.RUNNING;
                    case NodeState.SUCCESS:
                        currentChild++;
                        break;
                    case NodeState.FAILURE:
                        currentChild++;
                        return NodeState.FAILURE;
                }
            }
            Reset();
            return NodeState.SUCCESS;
        }
        
        public override void Reset()
        {
            base.Reset();
        }

        struct NodeStruct
        {
            public int index;
            public Node node;
        }
        
        public void PrintTree()
        {
            string tree = "";
            Stack<NodeStruct> stack = new Stack<NodeStruct>();
            stack.Push(new NodeStruct() { index = 0, node = this });
            while (stack.Count > 0)
            {
                NodeStruct node = stack.Pop();
                tree += new string('~', node.index * 2) + node.node.name + "\n";
                for(int i = node.node.Children.Count - 1; i >= 0; i--)
                {
                    stack.Push(new NodeStruct() { index = node.index + 1 , node = node.node.Children[i] });
                }
            }
            Debug.Log(tree);
        }
    }
    
    public class Leaf : Node
    {
        readonly ILeaf _leaf;
        
        public Leaf(string name, ILeaf leaf, int priority = 0) : base(name, priority)
        {
            _leaf = leaf;
        }
        
        public override NodeState Process()
        {
            return _leaf.Process();
        }
        
        public override void Reset()
        {
            _leaf.Reset();
        }
    }
    
    public class Sequence : Node
    {
        public Sequence(string name, int priority = 0) : base(name, priority)
        {
        }
        
        public override NodeState Process()
        {
            if(currentChild >= Children.Count)
            {
                Reset();
                return NodeState.SUCCESS;
            }
            switch (Children[currentChild].Process())
            {
                case NodeState.RUNNING:
                    return NodeState.RUNNING;
                case NodeState.SUCCESS:
                    currentChild++;
                    return Process();
                case NodeState.FAILURE:
                    Reset();
                    return NodeState.FAILURE;
                default:
                    currentChild++;
                    return currentChild == Children.Count ? NodeState.SUCCESS : NodeState.RUNNING;
            }
        }
    }
    
    public class Selector : Node
    {
        public Selector(string name, int priority = 0) : base(name, priority)
        {
        }
        
        public override NodeState Process()
        {
            if(currentChild >= Children.Count)
            {
                Reset();
                return NodeState.FAILURE;
            }
            switch (Children[currentChild].Process())
            {
                case NodeState.RUNNING:
                    return NodeState.RUNNING;
                case NodeState.SUCCESS:
                    Reset();
                    return NodeState.SUCCESS;
                case NodeState.FAILURE:
                    currentChild++;
                    return NodeState.RUNNING;
                default:
                    currentChild++;
                    return NodeState.RUNNING;
            }
        }
    }
    
    public class PrioritySelector : Selector 
    {
        List<Node> sortedChildren;
        List<Node> SortedChildren => sortedChildren ??= SortChildren();
        
        protected virtual List<Node> SortChildren() => Children.OrderByDescending(child => child.priority).ToList();

        public PrioritySelector(string name, int priority = 0) : base(name, priority)
        {
        }
        
        public override void Reset() {
            base.Reset();
            sortedChildren = null;
        }
        
        public override NodeState Process() {
            foreach (var child in SortedChildren) {
                switch (child.Process()) {
                    case NodeState.RUNNING:
                        return NodeState.RUNNING;
                    case NodeState.SUCCESS:
                        Reset();
                        return NodeState.SUCCESS;
                    default:
                        continue;
                }
            }

            Reset();
            return NodeState.FAILURE;
        }
    }
        #endregion
}


using code.behaviourtree;
using UnityEngine;

public class SetTarget : ILeaf
{
    private Transform _target;
    private Node _parentNode;
    
    public SetTarget(Transform target, Node parentNode)
    {
        _target = target;
        _parentNode = parentNode;
    }

    public Node.NodeState Process()
    {
        _parentNode.ClearData("Target");
        _parentNode.SetData("Target", _target);
        return Node.NodeState.SUCCESS;
    }
    
}

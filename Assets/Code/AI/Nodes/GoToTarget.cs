using code.behaviourtree;
using UnityEngine;
using UnityEngine.AI;


public class GoToTarget : ILeaf
{
    private NavMeshAgent _agent;
    private Transform Transform => _agent.transform;
    
    private Transform _target;
    
    private Node _parentNode;
    
    Animator _animator;

    public GoToTarget(NavMeshAgent agent, Node parentNode, Animator animator)
    {
        _parentNode = parentNode;
        _agent = agent;
        _animator = animator;
    }
    
    public Node.NodeState Process()
    {
        _target = _parentNode.GetData("Target") as Transform;
        if(_target == null)
        {
            return Node.NodeState.FAILURE;
        }
        if (Vector3.Distance(Transform.position, _target.position) <= 2.5f)
        {
            _animator.SetBool("Walk", false);
            return Node.NodeState.SUCCESS;
        }
        _animator.SetBool("Walk", true);
        _agent.SetDestination(_target.position);
        return Node.NodeState.RUNNING;
    }
    public void Reset()
    {
    }
}

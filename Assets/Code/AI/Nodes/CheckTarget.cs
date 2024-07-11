using code.behaviourtree;
using UnityEngine.AI;
using UnityEngine;

public class CheckTarget : ILeaf
{
    private NavMeshAgent _agent;
    private Transform Transform => _agent.transform;
    private float _FOVRadius;
    public LayerMask _targetLayer;
    private Node _parentNode;
    
    public CheckTarget(NavMeshAgent agent, float FOVRadius, LayerMask targetLayer, Node parentNode)
    {
        _agent = agent;
        _FOVRadius = FOVRadius;
        _targetLayer = targetLayer;
        _parentNode = parentNode;
    }
    
    public Node.NodeState Process()
    {
        Collider[] colliders = Physics.OverlapSphere(Transform.position, _FOVRadius, _targetLayer);

        if (colliders.Length > 0)
        {
            float minDistance = float.MaxValue;
            Transform closestTarget = null;

            foreach (var collider in colliders)                                    //taking the closest target
            {
                float distance = Vector3.Distance(Transform.position, collider.transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closestTarget = collider.transform;
                }
            }

            _parentNode.ClearData("Target");
            _parentNode.SetData("Target", closestTarget);
            Debug.Log("Closest target found");
            return Node.NodeState.SUCCESS;
        }
        Debug.Log("No target found");
        return Node.NodeState.FAILURE;
    }
    public void Reset()
    {
    }
    
    
}

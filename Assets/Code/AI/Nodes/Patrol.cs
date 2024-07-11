
using code.behaviourtree;
using UnityEngine.AI;
using UnityEngine;

public class Patrol : ILeaf
{
    
    private NavMeshAgent _agent;
    private float _patrolRadius;
    private Animator _animator;
    private bool _isPatrolling = false;
    
    private Transform Transform => _agent.transform;

    public Patrol(NavMeshAgent agent, float patrolRadius, Animator animator)
    {
        _agent = agent;
        _patrolRadius = patrolRadius;
        _animator = animator;
    }

    public Node.NodeState Process()
    {
        if(Vector3.Distance(_agent.transform.position, _agent.destination) <= 1)
        {
            if(_isPatrolling)
            {
                Debug.Log("Patroling success");
                _animator.SetBool("Walk", false);
                return Node.NodeState.SUCCESS;
            }
            _animator.SetBool("Walk", true);
            SetRandomDestination();
            
        }
        return Node.NodeState.RUNNING;
    }
    public void Reset()
    {
        _isPatrolling = false;
    }

    public void SetRandomDestination()
    {
        Debug.Log("Setting random destination");
        
        float randomAngle;
        if (Random.value < 0.75f)                            //tried to make the agent move in the same "direction angle" as the previous movement but not 100% of the time
        {
            randomAngle = Random.Range(-90, 90);
        }
        else
        {
            randomAngle = Random.Range(90, 270);
        }
        
        Vector3 randomDirection = Quaternion.Euler(0, randomAngle, 0) * _agent.transform.forward;
        Vector3 destination = _agent.transform.position + randomDirection * _patrolRadius;
        
        NavMeshHit hit;
        if (NavMesh.Raycast(_agent.transform.position, destination, out hit, NavMesh.AllAreas))         //check destination is reachable, if not find a new one
        {
            randomDirection = Quaternion.Euler(0, 90, 0) * randomDirection;
            destination = _agent.transform.position + randomDirection * _patrolRadius;
        }
        
        if (NavMesh.SamplePosition(destination, out hit, _patrolRadius, 1))
        {
            _agent.SetDestination(hit.position);
        }
        else
        {
            Debug.Log("Failed to find a valid patrol destination.");
        }

        _isPatrolling = true;
    }
}

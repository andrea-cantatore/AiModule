using code.behaviourtree;
using UnityEngine;
using UnityEngine.AI;

public class Cut : ILeaf
{
    private Animator _animator;
    private NavMeshAgent _agent;
    private AIController _controller;
    private bool _isCutting = false;
    private Node _parentNode;
    private int _cuttingCount;

    public Cut(Animator animator, NavMeshAgent agent, Node parentNode)
    {
        _animator = animator;
        _agent = agent;
        _controller = agent.GetComponent<AIController>();
        _parentNode = parentNode;
    }

    public Node.NodeState Process()
    {
        Transform obj = _parentNode.GetData("Target") as Transform;

        Vector3 direction = (obj.position - _controller.transform.position);                        //adjusting rotation
        direction.y = 0;
        direction = direction.normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        _controller.transform.rotation = Quaternion.Slerp(_controller.transform.rotation, lookRotation,
            Time.deltaTime * (_agent.angularSpeed / 20f));

        if (!_isCutting)
        {
            _controller.HungerAdder();
            _animator.SetBool("Attack", true);
            _isCutting = true;
            _cuttingCount++;
            if (_cuttingCount >= _controller.HitForCutting)
            {
                _cuttingCount = 0;
                obj.gameObject.layer = 0;
                obj.GetComponent<Animator>().SetBool("Falling", true);
            }
        }
        if (!_animator.GetBool("Attack"))
        {
            _isCutting = false;
            return Node.NodeState.SUCCESS;
        }
        return Node.NodeState.RUNNING;
    }
}

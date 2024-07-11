using code.behaviourtree;
using UnityEngine;

public class Eat : ILeaf
{
    private Animator _animator;
    private AIController _controller;
    private bool _isEating = false;
    private Node _parentNode;
    
    public Eat(Animator animator, AIController controller, Node parentNode)
    {
        _animator = animator;
        _controller = controller;
        _parentNode = parentNode;
    }
    
    public Node.NodeState Process()
    {
        if (!_isEating)
        {
            _animator.SetBool("Eating", true);
            _isEating = true;
            return Node.NodeState.RUNNING;
        }
        if (_animator.GetBool("Eating"))
        {
            return Node.NodeState.RUNNING;
        }
        
        Transform food = _parentNode.GetData("Target") as Transform;
        food.gameObject.SetActive(false);
        _controller.HadFood();
        _isEating = false;
        return Node.NodeState.SUCCESS;
    }
}

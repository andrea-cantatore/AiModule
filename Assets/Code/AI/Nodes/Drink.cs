using code.behaviourtree;
using UnityEngine;

public class Drink : ILeaf
{
    private Animator _animator;
    private AIController _controller;
    private bool _isDrinking = false;

    public Drink(Animator animator, AIController controller)
    {
        _animator = animator;
        _controller = controller;
    }

    public Node.NodeState Process()
    {
        if (!_isDrinking)
        {
            _animator.SetBool("Drinking", true);
            _isDrinking = true;
            return Node.NodeState.RUNNING;
        }
        if (_animator.GetBool("Drinking"))
        {
            return Node.NodeState.RUNNING;
        }
        _controller.HadDrink();
        _isDrinking = false;
        return Node.NodeState.SUCCESS;
    }
}

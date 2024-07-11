using code.behaviourtree;
using UnityEngine;
using UnityEngine.AI;

public class AIController : MonoBehaviour
{

        #region Variables

    [Header("AI Components")]
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private Animator _animator;
    
    [SerializeField] private Transform _home, _waterSource;
    
    [Header("AI Stats")]
    [SerializeField] private float _patrolRadius = 50f;
    [SerializeField] public float _FOVRadius = 10f;
    [SerializeField] private LayerMask _treeLayer, _foodLayer;
    
    [Header("General Stats")]
    
    [SerializeField] private int _thirstyThreshold = 50;
    [SerializeField] private int _hungryThreshold = 50;
    [SerializeField] private int _thirstToAdd = 100;
    [SerializeField] private int _hungerToAdd = 25;
    [SerializeField] private int _thirstToSubtract = 10;
    [SerializeField] private int _hungerToSubtract = 10;
    [SerializeField] private float _thirstTime = 10f;
    
    [SerializeField] private int _thirst;
    [SerializeField] private int _hunger;
    private int _maxThirst = 100, _maxHunger = 100;
    public int HitForCutting = 3;
    
    private float _timer;
    
    private BehaviourTree _tree;

        #endregion
        
    private void Awake()
    {
            #region BehaviourTree

        _tree = new BehaviourTree("Lumberjack");
        PrioritySelector root = new PrioritySelector("Root");
        Sequence BackToBase = new Sequence("BackToBase", 100);
        Sequence FindWater = new Sequence("FindWater", 75);
        Sequence FindFood = new Sequence("FindFood", 50);
        Sequence FindTree = new Sequence("CutTarget", 25);
        
            #region Conditions
        bool IsNight()
        {
            if (TimeManager.Instance.IsDay())
            {
                BackToBase.Reset();
                return false;
            }
            return true;
        }

        bool IsThirsty()
        {
            if(_thirst >= _thirstyThreshold)
            {
                FindWater.Reset();
                return false;
            }
            return true;
        }
        
        bool IsHungry()
        {
            if(_hunger >= _hungryThreshold)
            {
                FindFood.Reset();
                return false;
            }
            return true;
        }
            #endregion
            
        BackToBase.AddChild(new Leaf("IsNight?", new Condition(IsNight)));
        BackToBase.AddChild(new Leaf("Home Setter", new SetTarget(_home, root)));
        BackToBase.AddChild(new Leaf("GoToBase", new GoToTarget(_agent, root, _animator)));
        
        FindWater.AddChild(new Leaf("IsThirsty?", new Condition(IsThirsty)));
        FindWater.AddChild(new Leaf("Water Source Setter", new SetTarget(_waterSource, root)));
        FindWater.AddChild(new Leaf("GoToWater", new GoToTarget(_agent, root, _animator)));
        FindWater.AddChild(new Leaf("Drink", new Drink(_animator, this)));
        
        FindFood.AddChild(new Leaf("IsHungry?", new Condition(IsHungry)));
        FindFood.AddChild(new Leaf("Check Food", new CheckTarget(_agent, _FOVRadius, _foodLayer, root)));
        FindFood.AddChild(new Leaf("GoToFood", new GoToTarget(_agent, root, _animator)));
        FindFood.AddChild(new Leaf("Eat", new Eat(_animator, this, root)));
        
        FindTree.AddChild(new Leaf("IsAbleToCut", new Condition(() => _hunger >= _hungerToSubtract)));
        FindTree.AddChild(new Leaf("Check Tree", new CheckTarget(_agent, _FOVRadius, _treeLayer, root)));
        FindTree.AddChild(new Leaf("GoToTree", new GoToTarget(_agent, root, _animator)));
        FindTree.AddChild(new Leaf("Cut", new Cut(_animator, _agent, root)));
        
        root.AddChild(BackToBase);
        root.AddChild(FindWater);
        root.AddChild(FindFood);
        root.AddChild(FindTree);
        root.AddChild(new Leaf("Patrol", new Patrol(_agent, _patrolRadius, _animator)));
        
        _tree.AddChild(root);
        
        _tree.PrintTree();
        
            #endregion
    }

    private void Start()
    {
        _thirst = _maxThirst;
        _hunger = _maxHunger;
        UIManager.Instance.UpdateThirstSlider(_thirst);
        UIManager.Instance.UpdateHungerSlider(_hunger);
    }

    private void Update()
    {
        _tree.Process();
        
        _timer += Time.deltaTime;
        if(_timer >= _thirstTime)
        {
            _thirst -= _thirstToSubtract;
            UIManager.Instance.UpdateThirstSlider(_thirst);
            _timer = 0;
        }
        
    }
    
        #region Stats
    public void HadDrink()
    {
        _thirst += _thirstToAdd;
        if(_thirst >= _maxThirst)
        {
            _thirst = _maxThirst;
        }
        UIManager.Instance.UpdateThirstSlider(_thirst);
    }
    
    public void HadFood()
    {
        _hunger += _hungerToAdd;
        if (_hunger >= _maxHunger)
        {
            _hunger = _maxHunger;
        }
        UIManager.Instance.UpdateHungerSlider(_hunger);
    }
    
    public void HungerAdder()
    {
        _hunger -= _hungerToSubtract;
        if (_hunger <= 0)
        {
            _hunger = 0;
        }
        UIManager.Instance.UpdateHungerSlider(_hunger);
    }
    
        #endregion
    
}



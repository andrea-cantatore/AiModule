using System;
using Unity.AI.Navigation;
using UnityEngine;
using Grid = Code.Grid.Grid;
using Random = UnityEngine.Random;

public class ObjectSpawner : MonoBehaviour
{
    public static ObjectSpawner Instance { get; private set; }

    Grid _grid;
    [SerializeField] private int _width;
    [SerializeField] private int _height;
    [SerializeField] private float _cellSize;
    [SerializeField] private GameObject _treePrefab;
    [SerializeField] private GameObject _steakPrefab;
    [SerializeField] private int amount = 10;
    private int _amountCounter = 0;
    [SerializeField] private NavMeshSurface _navMeshSurface;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        _grid = new Grid(_cellSize, _width, _height);
    }

    private void Start()
    {
        SpawnObjects();
    }

    public void SpawnObjects()
    {
        Spawner(_treePrefab);
        Spawner(_steakPrefab);
    }

    private void Spawner(GameObject objToSpawn)
    {
        _amountCounter = 0;
        int maxAttempts = 500;
        int attempts = 0;

        while (_amountCounter < amount && attempts < maxAttempts)
        {
            Vector3 randomPosition =
                new Vector3(Random.Range(0, _width * _cellSize), 0, Random.Range(0, _height * _cellSize));

            if (_grid.SearchValue(randomPosition) == 0)
            {
                _grid.TakeValue(randomPosition, 1);
                ObjectPooler.SharedInstance.objectToPool = objToSpawn;
                GameObject newObject = ObjectPooler.SharedInstance.GetPooledObject();
                if (newObject != null)
                {
                    newObject.transform.position = randomPosition;
                    newObject.SetActive(true);
                    _amountCounter++;
                }
            }
            attempts++;
        }
    }

}

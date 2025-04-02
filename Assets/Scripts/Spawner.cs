using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Cube _prefab;
    [SerializeField] private ColorChanger _colorChanger;

    [SerializeField] private float _height;
    [SerializeField] private MeshCollider _spawnPlaceMesh;

    [SerializeField] private float _delay = 1f;
    [SerializeField] private bool _isSpawning = true;
    private WaitForSecondsRealtime _time;

    private ObjectPool<Cube> _pool;
    private int _poolCapacity = 10;
    private int _poolDefaultCapacity = 10;

    private float _maxLifeTime = 5f;
    private float _minLifeTime = 2f;

    private void Awake()
    {
        _time = new WaitForSecondsRealtime(_delay);
        _pool = new ObjectPool<Cube>(
            createFunc: () => CreateFunc(),
            actionOnGet: (cube) => ActionOnGet(cube),
            actionOnRelease: (cube) => cube.gameObject.SetActive(false),
            actionOnDestroy: (cube) => Destroy(cube.gameObject),
            collectionCheck: true,
            defaultCapacity: _poolDefaultCapacity,
            maxSize: _poolCapacity);
    }

    private void Start()
    {
        StartCoroutine(SpawnCubesCoroutine());
    }

    private Cube CreateFunc()
    {
        Cube cube = Instantiate(_prefab);
        cube.SetColorChanger(_colorChanger);

        return cube;
    }

    private IEnumerator SpawnCubesCoroutine()
    {
        while (_isSpawning)
        {
            GetCube();
            yield return _time;
        }
    }

    private void GetCube()
    {
        _pool.Get();
    }

    private void ActionOnGet(Cube cube)
    {
        cube.ResetCube(GetRandomLifeTime(), GetRandomSpawnPosition());
        cube.gameObject.SetActive(true);
        cube.Died += OnDied;
    }

    private void OnDied(Cube cube)
    {
        _pool.Release(cube);
    }

    private Vector3 GetRandomSpawnPosition()
    {
        Bounds bounds = _spawnPlaceMesh.bounds;

        float randomX = Random.Range(bounds.min.x, bounds.max.x);
        float randomZ = Random.Range(bounds.min.z, bounds.max.z);

        return new Vector3(randomX, _height + bounds.max.y, randomZ);
    }

    private float GetRandomLifeTime()
    {
        return Random.Range(_minLifeTime, _maxLifeTime);
    }
}

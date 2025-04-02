using UnityEngine;
using UnityEngine.Pool;


public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject _prefab;

    [SerializeField] private float _height;
    [SerializeField] private MeshCollider _spawnPlaceMesh;

    [SerializeField] private float _delay = 1f;
    private WaitForSecondsRealtime _time;

    private ObjectPool<GameObject> _pool;
    private int _poolCapacity = 10;
    private int _poolDefaultCapacity = 10;

    private float _maxLifeTime = 5f;
    private float _minLifeTime = 2f;

    private void Awake()
    {
        _time = new WaitForSecondsRealtime(_delay);
        _pool = new ObjectPool<GameObject>(
            createFunc: () => Instantiate(_prefab),
            actionOnGet: (obj) => ActionOnGet(obj),
            actionOnRelease: (obj) => obj.SetActive(false),
            actionOnDestroy: (obj) => Destroy(obj),
            collectionCheck: true,
            defaultCapacity: _poolDefaultCapacity,
            maxSize: _poolCapacity);
    }

    private void Start()
    {
        InvokeRepeating(nameof(GetCube), 0.0f, _delay);
    }

    public void ReturnCube(GameObject cube)
    {
        _pool.Release(cube);
    }

    private void GetCube()
    {
        _pool.Get();
    }

    private void ActionOnGet(GameObject obj)
    {
        obj.transform.position = GetRandomSpawnPosition();
        obj.transform.rotation = Quaternion.Euler(Vector3.zero);
        obj.GetComponentInChildren<Rigidbody>().velocity = Vector3.zero;
        obj.SetActive(true);
        obj.GetComponentInChildren<Cube>().ResetCube(GetRandomLifeTime());
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

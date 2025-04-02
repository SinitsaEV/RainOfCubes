using UnityEngine;

public class Cube : MonoBehaviour
{
    [SerializeField] LayerMask _layerMask;

    private ColorChanger _colorChanger;
    private Spawner _spawner;
    private Renderer _renderer;

    private Color _defaultСolor;
    private float _lifeTime;
    private bool isFirstCollision = true;

    private void Awake()
    {
        _colorChanger = FindObjectOfType<ColorChanger>();
        _renderer = GetComponent<Renderer>();
        _spawner = FindObjectOfType<Spawner>();
        _defaultСolor = _renderer.material.color;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (( _layerMask.value & (1 << collision.gameObject.layer)) != 0 && isFirstCollision)
        {
            isFirstCollision = false;
            _renderer.material.color = _colorChanger.GetRandomColor();
            Invoke(nameof(ReturnCube), _lifeTime);
        }
    }

    public void ResetCube(float lifeTime)
    {
        _renderer.material.color = _defaultСolor;
        isFirstCollision = true;
        _lifeTime = lifeTime;
    }

    private void ReturnCube()
    {
        _spawner.ReturnCube(gameObject);
    }
}

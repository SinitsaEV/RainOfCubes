using System;
using System.Collections;
using UnityEngine;

public class Cube : MonoBehaviour
{
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private Renderer _renderer;

    private ColorChanger _colorChanger;
    private Color _defaultСolor;

    private bool isFirstCollision = true;
    private WaitForSecondsRealtime _lifeTime;

    public Action<Cube> Died;

    private void Awake()
    {
        _defaultСolor = _renderer.material.color;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (( _layerMask.value & (1 << collision.gameObject.layer)) != 0 && isFirstCollision)
        {
            isFirstCollision = false;
            _renderer.material.color = _colorChanger.GetRandomColor();
            StartCoroutine(DeathTimerCoroutine());
        }
    }

    public void ResetCube(float lifeTime, Vector3 position)
    {
        transform.position = position;
        transform.rotation = Quaternion.Euler(Vector3.zero);
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        _renderer.material.color = _defaultСolor;
        isFirstCollision = true;
        SetLifeTime(lifeTime);
    }

    public void SetLifeTime(float lifeTime)
    {
        _lifeTime = new WaitForSecondsRealtime(lifeTime);
    }

    public void SetColorChanger(ColorChanger colorChanger)
    {
        _colorChanger = colorChanger;
    }

    private IEnumerator DeathTimerCoroutine()
    {
        yield return _lifeTime;
        Died?.Invoke(this);
        Died = null;
    }
}

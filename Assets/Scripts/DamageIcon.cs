using System;
using UnityEngine;

public class DamageIcon : MonoBehaviour
{
    [SerializeField] private Sprite[] _damageSprites;
    [SerializeField] private float _lifeTime;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private GameObject _effectPrefab;

    public void Setup(int damage)
    {
        var index = Math.Clamp(damage - 1, 0, _damageSprites.Length - 1);
        _spriteRenderer.sprite = _damageSprites[index];
    }

    private void Start()
    {
        Invoke(nameof(Destruction), _lifeTime);
    }

    private void Destruction()
    {
        Instantiate(_effectPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}

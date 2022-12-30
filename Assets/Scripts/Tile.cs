using UnityEngine;
using Random = UnityEngine.Random;

public class Tile : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Sprite[] _sprites;
    [SerializeField] private float _hoverAmount;
    [SerializeField] private LayerMask _obstacleLayer;

    private void Start()
    {
        var spriteCount = _sprites.Length;
        var randomSpriteIndex = Random.Range(0, spriteCount);
        _spriteRenderer.sprite = _sprites[randomSpriteIndex];
    }

    private void OnMouseEnter()
    {
        transform.localScale += Vector3.one * _hoverAmount;
    }
    
    private void OnMouseExit()
    {
        transform.localScale -= Vector3.one * _hoverAmount;
    }

    public bool IsClear()
    {
        var obstacle = Physics2D.OverlapCircle(transform.position, 0.2f, _obstacleLayer);
        return obstacle == null;
    }
}

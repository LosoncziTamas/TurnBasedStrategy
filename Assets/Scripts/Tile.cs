using UnityEngine;
using Random = UnityEngine.Random;

public class Tile : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Sprite[] _sprites;

    private void Start()
    {
        var spriteCount = _sprites.Length;
        var randomSpriteIndex = Random.Range(0, spriteCount);
        _spriteRenderer.sprite = _sprites[randomSpriteIndex];
    }
}

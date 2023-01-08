using UnityEngine;
using Random = UnityEngine.Random;

public class Tile : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private Sprite[] _sprites;
    [SerializeField] private LayerMask _obstacleLayer;
    [SerializeField] private Color _highlightedColor;
    [SerializeField] private Color _creatableColor;
    [SerializeField] private bool _isWalkable;
    [SerializeField] private bool _isCreatable;
    [SerializeField] private GameMaster _gameMaster;

    private void Start()
    {
        _gameMaster = FindObjectOfType<GameMaster>();
        var spriteCount = _sprites.Length;
        var randomSpriteIndex = Random.Range(0, spriteCount);
        _spriteRenderer.sprite = _sprites[randomSpriteIndex];
    }

    public void SetCreatable()
    {
        _spriteRenderer.color = _creatableColor;
        _isCreatable = true;
    }
    
    public bool IsClear()
    {
        var obstacle = Physics2D.OverlapCircle(transform.position, 0.2f, _obstacleLayer);
        return obstacle == null;
    }

    public void Highlight()
    {
        _spriteRenderer.color = _highlightedColor;
        _isWalkable = true;
    }

    public void ResetToDefault()
    {
        _spriteRenderer.color = Color.white;
        _isWalkable = false;
        _isCreatable = false;
    }

    private void OnMouseDown()
    {
        var unitCanWalkToThisTile = _isWalkable && _gameMaster.HasSelectedUnit;
        if (unitCanWalkToThisTile)
        {
            _gameMaster.SelectedUnit.Move(transform.position);
        }
        else if (_isCreatable)
        {
            var barrackItem = Instantiate(_gameMaster.PurchasedItem, new Vector2(transform.position.x, transform.position.y), Quaternion.identity);
            GameMaster.ResetTiles();
            var unit = barrackItem.GetComponent<Unit>();
            if (unit != null)
            {
                unit.HasMoved = true;
                unit.HasAttacked = true;
            }
        }
    }
}

using System.Collections;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public bool Selected { get; set; }
    public bool HasMoved { get; set; }
    
    [SerializeField] private PlayerType _playerType;
    [SerializeField] private int _tileSpeed;
    [SerializeField] private float _moveSpeed;

    private GameMaster _gameMaster;

    private void OnValidate()
    {
        if (_playerType == PlayerType.None)
        {
            Debug.LogError($"PlayerType is not set on {gameObject.name}.");
        }
    }

    private void Start()
    {
        _gameMaster = FindObjectOfType<GameMaster>();
    }

    private void OnMouseDown()
    {
        if (Selected)
        {
            Unselect();
        }
        else
        {
            var selected = Select(this);
            if (selected)
            {
                GetWalkableTiles();
            }
        }
    }

    private void Unselect()
    {
        Selected = false;
        _gameMaster.SelectedUnit = null;
        GameMaster.ResetTiles();
    }

    private bool Select(Unit toSelect)
    {
        if (_playerType == _gameMaster.PlayerTurn)
        {
            if (_gameMaster.HasSelectedUnit)
            {
                _gameMaster.SelectedUnit.Selected = false;
            }
            Selected = true;
            _gameMaster.SelectedUnit = toSelect;
            GameMaster.ResetTiles();
            return true;
        }
        return false;
    }

    private void GetWalkableTiles()
    {
        if (HasMoved)
        {
            return;
        }
        var allTiles = FindObjectsOfType<Tile>();
        foreach (var tile in allTiles)
        {
            if (HasEnoughSpeedToGetToTile(tile) && tile.IsClear())
            {
                tile.Highlight();
            }
        }
    }

    private bool HasEnoughSpeedToGetToTile(Tile tile)
    {
        Vector2 unitPos = transform.position;
        Vector2 tilePos = tile.transform.position;
        var horizontalDistance = Mathf.Abs(tilePos.x - unitPos.x);
        var verticalDistance = Mathf.Abs(tilePos.y - unitPos.y);
        return horizontalDistance + verticalDistance <= _tileSpeed;
    }

    public void Move(Vector2 tilePos)
    {
        GameMaster.ResetTiles();
        StartCoroutine(StartMovement(tilePos));
    }

    private IEnumerator StartMovement(Vector2 tilePos)
    {
        while (!Mathf.Approximately(transform.position.x, tilePos.x))
        {
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(tilePos.x, transform.position.y),
                _moveSpeed * Time.deltaTime);
            yield return null;
        }
        while (!Mathf.Approximately(transform.position.y, tilePos.y))
        {
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x, tilePos.y),
                _moveSpeed * Time.deltaTime);
            yield return null;
        }
        HasMoved = true;
    }
}

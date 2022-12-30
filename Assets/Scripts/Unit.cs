using UnityEngine;

public class Unit : MonoBehaviour
{
    public bool Selected { get; private set; }
    
    [SerializeField] private int _tileSpeed;
    [SerializeField] private bool _hasMoved;

    private GameMaster _gameMaster;
    
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
            Select(this);
            GetWalkableTiles();
        }
    }

    private void Unselect()
    {
        Selected = false;
        _gameMaster.SelectedUnit = null;
    }

    private void Select(Unit toSelect)
    {
        if (_gameMaster.SelectedUnit != null)
        {
            _gameMaster.SelectedUnit.Selected = false;
        }
        Selected = true;
        _gameMaster.SelectedUnit = toSelect;
    }

    private void GetWalkableTiles()
    {
        if (_hasMoved)
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
}

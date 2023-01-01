using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    public bool Selected { get; set; }
    public bool HasMoved { get; set; }
    public bool HasAttacked { get; set; }
    
    [SerializeField] private PlayerType _playerType;
    [SerializeField] private int _tileSpeed;
    [SerializeField] private int _attackRange;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private GameObject _weaponIcon;

    private GameMaster _gameMaster;
    private List<Unit> _enemiesInRage = new();

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
        ResetWeaponIcons();
        if (Selected)
        {
            Unselect();
        }
        else
        {
            var selected = Select(this);
            if (selected)
            {
                GetEnemies();
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
            if (WithinRange(transform, tile.transform, _tileSpeed) && tile.IsClear())
            {
                tile.Highlight();
            }
        }
    }

    private static bool WithinRange(Transform self, Transform other, int range)
    {
        Vector2 unitPos = self.position;
        Vector2 tilePos = other.position;
        var horizontalDistance = Mathf.Abs(tilePos.x - unitPos.x);
        var verticalDistance = Mathf.Abs(tilePos.y - unitPos.y);
        return horizontalDistance + verticalDistance <= range;
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
        GetEnemies();
    }

    private void GetEnemies()
    {
        _enemiesInRage.Clear();
        var units = FindObjectsOfType<Unit>();
        foreach (var unit in units)
        {
            if (WithinRange(transform, unit.transform, _attackRange))
            {
                var canAttack = !HasAttacked && unit._playerType != _gameMaster.PlayerTurn;
                if (canAttack)
                {
                    _enemiesInRage.Add(unit);
                    _weaponIcon.gameObject.SetActive(true);
                }
            }
        }
    }

    public void ResetWeaponIcons()
    {
        var units = FindObjectsOfType<Unit>();
        foreach (var unit in units)
        {
            unit._weaponIcon.SetActive(false);
        }
    }
}

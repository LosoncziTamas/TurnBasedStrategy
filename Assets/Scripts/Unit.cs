using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    private static readonly int ShakeTriggerId = Animator.StringToHash("Shake");
    
    public bool Selected { get; set; }
    public bool HasMoved { get; set; }
    public bool HasAttacked { get; set; }
    public GameObject WeaponIcon => _weaponIcon; 
    
    [SerializeField] private PlayerType _playerType;
    [SerializeField] private int _tileSpeed;
    [SerializeField] private int _attackRange;
    [SerializeField] private int _health;
    [SerializeField] private int _attackDamage;
    [SerializeField] private int _defenseDamage;
    [SerializeField] private int _armor;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private GameObject _weaponIcon;
    [SerializeField] private DamageIcon _damageIconPrefab;
    [SerializeField] private GameObject _deathEffectPrefab;

    private Camera _mainCamera;
    private GameMaster _gameMaster;
    private Animator _cameraAnimator;
    private readonly List<Unit> _enemiesInRage = new();

    private void OnValidate()
    {
        if (_playerType == PlayerType.None)
        {
            Debug.LogError($"PlayerType is not set on {gameObject.name}.");
        }
    }

    private void Start()
    {
        _mainCamera = Camera.main;
        _cameraAnimator = _mainCamera.GetComponent<Animator>();
        _gameMaster = FindObjectOfType<GameMaster>();
    }

    private bool IsPotentialEnemyInRage()
    {
        return _gameMaster.HasSelectedUnit && _gameMaster.SelectedUnit._enemiesInRage.Count > 0;
    }

    private static bool UnitCanBeAttacked(Unit unit, Unit selectedUnit)
    {
        return unit && selectedUnit._enemiesInRage.Contains(unit) && !selectedUnit.HasAttacked;
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
        if (!IsPotentialEnemyInRage())
        {
            return;
        }
        var col = Physics2D.OverlapCircle(_mainCamera.ScreenToWorldPoint(Input.mousePosition), 0.15f);
        var unit = col.GetComponent<Unit>();
        var selectedUnit = _gameMaster.SelectedUnit;
        if (UnitCanBeAttacked(unit, selectedUnit))
        {
            _gameMaster.SelectedUnit.Attack(unit);
        }
    }

    private void Attack(Unit enemy)
    {
        _cameraAnimator.SetTrigger(ShakeTriggerId);
        HasAttacked = true;
        var enemyDamage = Mathf.Max(_attackDamage - enemy._armor, 0);
        var myDamage = Mathf.Max(enemy._defenseDamage - _armor, 0);
        if (enemyDamage > 0)
        {
            enemy._health -= enemyDamage;
            var damageIcon = Instantiate(_damageIconPrefab, enemy.transform.position, Quaternion.identity);
            damageIcon.Setup(enemyDamage);
        }
        if (myDamage > 0)
        {
            _health -= myDamage;
            var damageIcon = Instantiate(_damageIconPrefab, transform.position, Quaternion.identity);
            damageIcon.Setup(myDamage);
        }
        if (enemy._health <= 0)
        {
            Instantiate(_deathEffectPrefab, enemy.transform.position, Quaternion.identity);
            Destroy(enemy.gameObject);
            GetWalkableTiles();
        }
        if (_health <= 0)
        {
            Instantiate(_deathEffectPrefab, transform.position, Quaternion.identity);
            GameMaster.ResetTiles();
            Destroy(gameObject);
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
        ResetWeaponIcons();
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
                    unit.WeaponIcon.SetActive(true);
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

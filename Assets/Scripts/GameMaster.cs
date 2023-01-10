using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameMaster : MonoBehaviour
{
    public bool HasSelectedUnit => SelectedUnit != null;
    public Unit SelectedUnit { get; set; }
    public PlayerType PlayerTurn { get; private set; } = PlayerType.Red;
    public BarrackItem PurchasedItem { get; set; }

    public int BluePlayerGold
    {
        get => _bluePlayerGold;
        set => _bluePlayerGold = value;
    }

    public int RedPlayerGold
    {
        get => _redPlayerGold;
        set => _redPlayerGold = value;   
    }

    [SerializeField] private GameObject _selectedUnitSquare;
    [SerializeField] private Image _playerIndicator;
    [SerializeField] private Sprite _redPlayerIndicator;
    [SerializeField] private Sprite _bluePlayerIndicator;
    [SerializeField] private int _bluePlayerGold = 100;
    [SerializeField] private int _redPlayerGold = 100;
    [SerializeField] private TextMeshProUGUI _redPlayerGoldText;
    [SerializeField] private TextMeshProUGUI _bluePlayerGoldText;
    [SerializeField] private Barrack _barrack;
    [SerializeField] private Vector2 _statsPanelShift;
    [SerializeField] private Unit _viewedUnit;

    public static void ResetTiles()
    {
        var allTiles = FindObjectsOfType<Tile>();
        foreach (var tile in allTiles)
        {
            tile.ResetToDefault();
        }
    }

    public void ToggleStatsPanel(Unit unit)
    {
        var newUnitSelected = unit != _viewedUnit;
        var panel = StatsPanel.Instance;
        if (newUnitSelected)
        {
            panel.SetVisible(true);
            panel.transform.position = (Vector2)unit.transform.position + _statsPanelShift;
            _viewedUnit = unit;
            UpdateStatsPanel();
        }
        else
        {
            panel.SetVisible(false);
            _viewedUnit = null;
        }
    }

    public void UpdateStatsPanel()
    {
        if (_viewedUnit != null)
        {
            StatsPanel.Instance.UpdateStats(_viewedUnit);
        }
    }

    private void Start()
    {
        GetGoldIncome(PlayerType.Red);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            EndTurn();
        }

        if (HasSelectedUnit)
        {
            _selectedUnitSquare.gameObject.SetActive(true);
            _selectedUnitSquare.transform.position = SelectedUnit.transform.position;
        }
        else
        {
            _selectedUnitSquare.gameObject.SetActive(false);
        }
    }

    private void GetGoldIncome(PlayerType playerTurn)
    {
        var villages = FindObjectsOfType<Village>();
        foreach (var village in villages)
        {
            if (village.PlayerType != playerTurn)
            {
                continue;
            }
            if (playerTurn == PlayerType.Blue)
            {
                _bluePlayerGold += village.GoldPerTurn;
            }
            else
            {
                _redPlayerGold += village.GoldPerTurn;
            }
        }
        UpdateGoldText();
    }

    public void UpdateGoldText()
    {
        _bluePlayerGoldText.text = _bluePlayerGold.ToString();
        _redPlayerGoldText.text = _redPlayerGold.ToString();
    }
    
    private void EndTurn()
    {
        PlayerTurn = PlayerTurn == PlayerType.Blue ? PlayerType.Red : PlayerType.Blue;
        _playerIndicator.sprite = PlayerTurn == PlayerType.Blue ? _bluePlayerIndicator : _redPlayerIndicator;
        GetGoldIncome(PlayerTurn);
        if (HasSelectedUnit)
        {
            SelectedUnit.Selected = false;
            SelectedUnit = null;
        }
        ResetTiles();
        var allUnits = FindObjectsOfType<Unit>();
        foreach (var unit in allUnits)
        {
            unit.HasMoved = false;
            unit.WeaponIcon.SetActive(false);
            unit.HasAttacked = false;
        }
        _barrack.CloseMenus();
    }

    public void MoveStatsPanel(Unit unit)
    {
        if (unit == _viewedUnit)
        {
            StatsPanel.Instance.transform.position = (Vector2)unit.transform.position + _statsPanelShift;
        }
    }

    public void RemoveStatsPanel(Unit unit)
    {
        if (unit == _viewedUnit)
        {
            StatsPanel.Instance.SetVisible(false);
            _viewedUnit = null;
        }
    }
}

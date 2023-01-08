using UnityEngine;
using UnityEngine.UI;

public class Barrack : MonoBehaviour
{
    [SerializeField] private Button _redPlayerToggleButton;
    [SerializeField] private Button _bluePlayerToggleButton;
    [SerializeField] private GameObject _bluePlayerMenu;
    [SerializeField] private GameObject _redPlayerMenu;
    [SerializeField] private GameMaster _gameMaster;
    
    private void Update()
    {
        _redPlayerToggleButton.interactable = _gameMaster.PlayerTurn == PlayerType.Red;
        _bluePlayerToggleButton.interactable = _gameMaster.PlayerTurn == PlayerType.Blue;
    }

    public void ToggleMenu(GameObject menu)
    {
        menu.SetActive(!menu.activeSelf);
    }

    public void CloseMenus()
    {
        _bluePlayerMenu.SetActive(false);
        _redPlayerMenu.SetActive(false);
    }

    public void BuyItem(BarrackItem item)
    {
        if (_gameMaster.PlayerTurn == PlayerType.Red && item.Cost <= _gameMaster.RedPlayerGold)
        {
            _gameMaster.RedPlayerGold -= item.Cost;
            _redPlayerMenu.SetActive(false);
        }
        else if (_gameMaster.PlayerTurn == PlayerType.Blue && item.Cost <= _gameMaster.BluePlayerGold)
        {
            _gameMaster.BluePlayerGold -= item.Cost;
            _bluePlayerMenu.SetActive(false);
        }
        else
        {
            Debug.Log("[Barrack] not enough code");
            return;
        }
        _gameMaster.UpdateGoldText();
        _gameMaster.PurchasedItem = item;
        if (_gameMaster.SelectedUnit != null)
        {
            _gameMaster.SelectedUnit.Selected = false;
            _gameMaster.SelectedUnit = null;
        }
        GetCreatableTiles();
    }

    public void GetCreatableTiles()
    {
        var tiles = FindObjectsOfType<Tile>();
        foreach (var tile in tiles)
        {
            if (tile.IsClear())
            {
                tile.SetCreatable();
            }
        }
    }
}

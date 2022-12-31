using UnityEngine;

public class GameMaster : MonoBehaviour
{
    public bool HasSelectedUnit => SelectedUnit != null;
    public Unit SelectedUnit { get; set; }
    public PlayerType PlayerTurn { get; private set; } = PlayerType.Blue;

    [SerializeField] private GameObject _selectedUnitSquare;

    public static void ResetTiles()
    {
        var allTiles = FindObjectsOfType<Tile>();
        foreach (var tile in allTiles)
        {
            tile.ResetToDefault();
        }
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

    private void EndTurn()
    {
        PlayerTurn = PlayerTurn == PlayerType.Blue ? PlayerType.Red : PlayerType.Blue;
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
        }
    }
}

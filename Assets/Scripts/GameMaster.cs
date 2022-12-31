using UnityEngine;

public class GameMaster : MonoBehaviour
{
    public Unit SelectedUnit { get; set; }

    public PlayerType PlayerTurn { get; set; } = PlayerType.Blue;

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
    }

    private void EndTurn()
    {
        PlayerTurn = PlayerTurn == PlayerType.Blue ? PlayerType.Red : PlayerType.Blue;
        if (SelectedUnit != null)
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

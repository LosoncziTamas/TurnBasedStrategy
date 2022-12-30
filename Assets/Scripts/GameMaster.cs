using UnityEngine;

public class GameMaster : MonoBehaviour
{
    public Unit SelectedUnit { get; set; }

    public void ResetTiles()
    {
        var allTiles = FindObjectsOfType<Tile>();
        foreach (var tile in allTiles)
        {
            tile.ResetToDefault();
        }
    }
   
}

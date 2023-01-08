using UnityEngine;

public class Village : MonoBehaviour
{
    public PlayerType PlayerType => _playerType;
    public int GoldPerTurn => _goldPerTurn;
    
    [SerializeField] private int _goldPerTurn;
    [SerializeField] private PlayerType _playerType;
}

using TMPro;
using UnityEngine;

public class BarrackItem : MonoBehaviour
{
    public int Cost => _cost;
    
    [SerializeField] private int _cost;
    [SerializeField] private TextMeshProUGUI _text;

    private void Start()
    {
        _text.text = _cost.ToString();
    }
}

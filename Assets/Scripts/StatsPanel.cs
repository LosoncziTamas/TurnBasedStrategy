using TMPro;
using UnityEngine;

public class StatsPanel : MonoBehaviour
{
    public static StatsPanel Instance { private set; get; }

    [SerializeField] private TextMeshProUGUI _heartText;
    [SerializeField] private TextMeshProUGUI _shieldText;
    [SerializeField] private TextMeshProUGUI _swordText;
    [SerializeField] private TextMeshProUGUI _arrowText;
    [SerializeField] private CanvasGroup _canvasGroup;

    public TextMeshProUGUI HeartText => _heartText;
    public TextMeshProUGUI ShieldText => _shieldText;
    public TextMeshProUGUI SwordText => _swordText;
    public TextMeshProUGUI ArrowText => _arrowText;
    
    private void Awake()
    {
        Instance = this;
        SetVisible(false);
    }

    public void SetVisible(bool visible)
    {
        _canvasGroup.alpha = visible ? 1.0f : 0.0f;
        _canvasGroup.interactable = visible;
        _canvasGroup.blocksRaycasts = visible;
    }

    public void UpdateStats(Unit unit)
    {
        _heartText.text = unit.Health.ToString();
        _shieldText.text = unit.Armor.ToString();
        _swordText.text = unit.AttackDamage.ToString();
        _arrowText.text = unit.DefenseDamage.ToString();
    }
}

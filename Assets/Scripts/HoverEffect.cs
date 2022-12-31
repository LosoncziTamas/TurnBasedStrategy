using UnityEngine;

public class HoverEffect : MonoBehaviour
{
    [SerializeField] private float _hoverAmount;
        
    private void OnMouseEnter()
    {
        transform.localScale += Vector3.one * _hoverAmount;
    }
    
    private void OnMouseExit()
    {
        transform.localScale -= Vector3.one * _hoverAmount;
    }
}
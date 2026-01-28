using UnityEngine;

public class SortingHandler : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void LateUpdate()
    {
        spriteRenderer.sortingOrder = Mathf.RoundToInt(transform.position.y * -2000 + transform.position.x * 100);
    }
}

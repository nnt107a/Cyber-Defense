using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UnitDragHandler : MonoBehaviour
{
    public static UnitDragHandler Instance;
    private Canvas canvas;
    public GameObject dragPreview;
    private RectTransform dragRect;
    public GameObject shopElement;
    public bool dragging = false;
    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        // Find the parent canvas (required for proper positioning)
        canvas = GetComponentInParent<Canvas>();
    }
    public void BeginDrag(ShopElement shopElement)
    {
        // Simulate the drag events
        this.shopElement = shopElement.gameObject;
        // Create a copy (duplicate this UI element)
        dragPreview = Instantiate(this.shopElement, canvas.transform);
        dragRect = dragPreview.GetComponent<RectTransform>();
        dragRect.sizeDelta = shopElement.GetComponent<RectTransform>().sizeDelta;

        // Optional: make it semi-transparent to show it’s a preview
        CanvasGroup cg = dragPreview.AddComponent<CanvasGroup>();
        cg.alpha = 0.8f;
        cg.blocksRaycasts = false;
        cg.interactable = false;
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!dragging)
            {
                dragging = true;
            }
        }
        if (dragRect == null || !dragging) return;
        // Move the preview with mouse
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            Input.mousePosition,
            canvas.worldCamera,
            out Vector2 localPos
        );
        dragRect.localPosition = localPos;
    }
}

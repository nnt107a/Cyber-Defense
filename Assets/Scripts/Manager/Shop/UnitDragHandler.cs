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
    public bool isSellAction = false;
    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        // Find the parent canvas (required for proper positioning)
        canvas = GetComponentInParent<Canvas>();
    }
    public void BeginDrag(GameObject _gameObject, bool isSellAction = false)
    {
        this.isSellAction = isSellAction;
        // Simulate the drag events
        shopElement = _gameObject;
        // Create a copy (duplicate this UI element)
        dragPreview = Instantiate(shopElement, canvas.transform);
        dragRect = dragPreview.GetComponent<RectTransform>();
        dragRect.sizeDelta = _gameObject.GetComponent<RectTransform>().sizeDelta;

        RectTransform source = _gameObject.GetComponent<RectTransform>();
        RectTransform target = dragRect;

        Vector3[] corners = new Vector3[4];
        source.GetWorldCorners(corners);

        Vector3 worldSize = corners[2] - corners[0];

        Vector2 localSize = target.parent.InverseTransformVector(worldSize);

        target.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, localSize.x * 1.2f);
        target.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, localSize.y * 1.2f);

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
        if (Input.GetMouseButtonDown(1))
        {
            if (dragging)
            {
                dragging = false;
                if (dragPreview != null)
                {
                    Destroy(dragPreview);
                }
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

using UnityEngine;
using UnityEngine.UI;

public class UISellButton : MonoBehaviour
{
    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(() =>
        {
            if (UnitDragHandler.Instance.dragPreview != null && UnitDragHandler.Instance.dragPreview.gameObject.activeInHierarchy)
            {
                Destroy(UnitDragHandler.Instance.dragPreview);
            }
            UnitDragHandler.Instance.BeginDrag(gameObject, true);
        });
    }
}

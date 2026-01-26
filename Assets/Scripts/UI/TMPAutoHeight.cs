using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class TMPAutoHeight : MonoBehaviour
{
    private TextMeshProUGUI tmp;
    private RectTransform rect;

    void Awake()
    {
        tmp = GetComponent<TextMeshProUGUI>();
        rect = GetComponent<RectTransform>();
    }

    void LateUpdate()
    {
        FitHeight();
    }

    public void FitHeight()
    {
        // Force TMP to update its geometry
        tmp.ForceMeshUpdate();

        float height = tmp.preferredHeight;

        rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
    }
}

using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
public class UIConnector : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public float lineWidth = 10f;

    void Update()
    {
        if (pointA == null || pointB == null)
            return;

        UpdateTransform();
    }

    void UpdateTransform()
    {
        RectTransform rt = GetComponent<RectTransform>();

        Vector3 centerPos = (pointA.position + pointB.position) / 2f;
        rt.position = centerPos;

        Vector3 direction = pointB.position - pointA.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        rt.localRotation = Quaternion.Euler(0, 0, angle);

        float distance = direction.magnitude;
        rt.sizeDelta = new Vector2(distance, lineWidth);
    }
}

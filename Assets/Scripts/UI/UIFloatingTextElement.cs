using UnityEngine;

public class UIFloatingTextElement : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI textMesh;
    [SerializeField] private float floatSpeed = 1f;
    [SerializeField] private float duration = 1f;
    private float timer;
    public void SetText(string text, Color color)
    {
        textMesh.text = text;
        textMesh.color = color;
        timer = 0f;
    }
    private void Update()
    {
        transform.Translate(Vector2.up * floatSpeed * Time.deltaTime);
        timer += Time.deltaTime;
        if (timer >= duration)
        {
            Destroy(gameObject);
        }
    }
}

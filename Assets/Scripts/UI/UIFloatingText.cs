using UnityEngine;

public class UIFloatingText : MonoBehaviour
{
    public static UIFloatingText Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    [SerializeField] private GameObject floatingTextPrefab;
    public void ShowFloatingText(string text, Vector3 position, Color color)
    {
        GameObject floatingTextObj = Instantiate(floatingTextPrefab, position, Quaternion.identity, transform);
        UIFloatingTextElement floatingTextElement = floatingTextObj.GetComponent<UIFloatingTextElement>();
        if (floatingTextElement != null)
        {
            floatingTextElement.SetText(text, color);
        }
    }
}

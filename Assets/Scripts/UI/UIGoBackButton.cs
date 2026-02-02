using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIGoBackButton : MonoBehaviour
{
    [SerializeField]
    private Button button;

    private void Start()
    {
        button.onClick.AddListener(ReturnToMainMenu);
    }

    private void OnDestroy()
    {
        button.onClick.RemoveListener(ReturnToMainMenu);
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}

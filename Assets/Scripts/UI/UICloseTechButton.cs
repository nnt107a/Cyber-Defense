using UnityEngine;
using UnityEngine.UI;
public class UICloseTechButton : MonoBehaviour
{
    [SerializeField]
    private Button button;
    [SerializeField] private GameObject techInfoPanel;

    [SerializeField]
    private LoadoutBoardController loadoutBoard;

    private void Start()
    {
        button.onClick.AddListener(loadoutBoard.Hide);
        button.onClick.AddListener(HideTechInfoPanel);
    }

    private void OnDestroy()
    {
        button.onClick.RemoveListener(loadoutBoard.Hide);
        button.onClick.RemoveListener(HideTechInfoPanel);
    }

    private void HideTechInfoPanel()
    {
        techInfoPanel.SetActive(false);
    }
}

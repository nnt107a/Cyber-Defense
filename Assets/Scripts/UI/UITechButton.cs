using UnityEngine;
using UnityEngine.UI;
public class UITechButton : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private LoadoutBoardController loadoutBoard;

    private void Start()
    {
        button.onClick.AddListener(loadoutBoard.Show);
    }

    private void OnDestroy()
    {
        button.onClick.RemoveListener(loadoutBoard.Show);
    }
}

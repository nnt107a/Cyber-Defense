using UnityEngine;
using UnityEngine.UI;
public class UITechButton : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private LoadoutBoardController loadoutBoard;
    [SerializeField] private WikiManager wikiBoard;

    private void Start()
    {
        button.onClick.AddListener(() => {
            loadoutBoard.Show();
            if (wikiBoard.gameObject.activeInHierarchy)
                wikiBoard.Hide();
        });
    }

    private void OnDestroy()
    {
        button.onClick.RemoveAllListeners();
    }
}

using UnityEngine;
using UnityEngine.UI;

public class UIWikiButton : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private WikiManager wikiBoard;
    [SerializeField] private LoadoutBoardController techBoard;

    private void Start()
    {
        button.onClick.AddListener(HandleWikiBoard);
    }

    private void OnDestroy()
    {
        button.onClick.RemoveListener(HandleWikiBoard);
    }
    private void HandleWikiBoard()
    {
        if (wikiBoard.gameObject.activeInHierarchy)
        {
            wikiBoard.Hide();
        }
        else
        {
            wikiBoard.Show();
            if (techBoard.gameObject.activeInHierarchy)
                techBoard.Hide();
        }
    }
}

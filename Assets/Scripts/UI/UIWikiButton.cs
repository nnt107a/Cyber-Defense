using UnityEngine;
using UnityEngine.UI;

public class UIWikiButton : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private WikiManager wikiBoard;

    private void Start()
    {
        button.onClick.AddListener(wikiBoard.Show);
    }

    private void OnDestroy()
    {
        button.onClick.RemoveListener(wikiBoard.Show);
    }
}

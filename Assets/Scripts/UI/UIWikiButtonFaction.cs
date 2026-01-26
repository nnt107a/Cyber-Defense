using UnityEngine;

public class UIWikiButtonFaction : MonoBehaviour
{
    public WikiManager wikiManager;
    public bool isTurret;
    void Start()
    {
        GetComponent<UnityEngine.UI.Button>().onClick.AddListener(OnClick);
    }
    void OnClick()
    {
        wikiManager.SelectFaction(isTurret);
    }
}

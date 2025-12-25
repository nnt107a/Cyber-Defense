using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class GlobalUIButtonSound : MonoBehaviour
{
    void Awake()
    {
        HookAllButtons();
    }

    void HookAllButtons()
    {
        Button[] buttons = FindObjectsByType<Button>(FindObjectsInactive.Include, FindObjectsSortMode.None);

        foreach (Button btn in buttons)
        {
            if (btn.gameObject.GetComponent<ShopElement>() != null || btn.gameObject.GetComponent<LoadoutSlot>() != null || btn.gameObject.GetComponent<UISellButton>() != null)
            {
                btn.onClick.RemoveAllListeners();
                btn.onClick.AddListener(PlayUnitSelectClick);
            }
            else
            {
                btn.onClick.RemoveAllListeners();
                btn.onClick.AddListener(PlaySound);
            }
        }
    }

    void PlaySound()
    {
        SoundManager.Instance?.PlayUIClick();
    }
    void PlayUnitSelectClick()
    {
        SoundManager.Instance?.PlayUnitSelectClick();
    }
}

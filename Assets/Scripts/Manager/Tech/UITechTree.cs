using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UITechTree : MonoBehaviour
{
    public TechNode[] techNodes;

    [Header("UI References")]
    [SerializeField] private GameObject techNodesHolder;
    [SerializeField] private GameObject techInfoPanel;
    [SerializeField] TechUnlockPath[] techUnlockPaths;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (techNodesHolder != null)
            techNodes = techNodesHolder.GetComponentsInChildren<TechNode>(true);

        foreach (TechNode node in techNodes)
        {
            node.Setup(node.techData);
        }

        if (TechManager.Instance != null)
        {
            TechManager.Instance.OnDataChanged += UpdateVisuals;
            UpdateVisuals();
        }
    }

    void OnDestroy()
    {
        if (TechManager.Instance != null)
        {
            TechManager.Instance.OnDataChanged -= UpdateVisuals;
        }
    }

    public void UpdateVisuals()
    {

        foreach (TechNode node in techNodes)
        {
            TechState stateToSet = CalculateState(node.techData);
            node.SetState(stateToSet);
        }

        if (techUnlockPaths == null)
            return;
        foreach (TechUnlockPath path in techUnlockPaths)
        {
            foreach (Image connector in path.connectors)
            {
                if (TechManager.Instance.IsUnlocked(path.techData))
                {
                    connector.color = Color.white;
                }
                else
                {
                    connector.color = new Color(0.2f, 0.2f, 0.2f, 1f);
                }
            }
        }
    }

    private TechState CalculateState(TechData data)
    {
        if (TechManager.Instance.IsUnlocked(data))
        {
            return TechState.Researched;
        }
        if (TechManager.Instance.CanUnlock(data))
        {
            return TechState.Available;
        }
        else
        {
            return TechState.Locked;
        }
    }

    public void OnTechNodeClicked(TechData data)
    {
        if (techInfoPanel != null)
        {
            techInfoPanel.SetActive(true);
            techInfoPanel.GetComponent<TechInfo>().SetUp(data);
        }
    }

    public void OnClosePanel()
    {
        if (techInfoPanel != null)
            techInfoPanel.SetActive(false);
    }

    public void OnPurchaseClicked(TechData data)
    {
        bool success = TechManager.Instance.TryResearch(data);

        if (success)
        {
            // OnClosePanel();
        }
        else
        {
            Debug.Log("Cannot unlock tech: " + data.TechName);
            // Có thể thêm hiệu ứng rung lắc báo lỗi
        }
    }

}

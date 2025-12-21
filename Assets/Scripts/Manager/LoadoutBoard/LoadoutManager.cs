using System.Collections.Generic;
using UnityEngine;

public class LoadoutManager : MonoBehaviour
{
    public static LoadoutManager Instance;

    [Header("Limits")]
    public int maxSlots = 6;

    [Header("Shop Slots")]
    public GameObject[] shopSlots;
    
    public LoadoutSlot[] loadoutSlots;

    private List<TurretData> selectedTurrets = new();

    void Awake()
    {
        Instance = this;
    }

    public bool IsSelected(TurretData data)
    {
        return selectedTurrets.Contains(data);
    }

    public bool CanSelect()
    {
        return selectedTurrets.Count < maxSlots;
    }

    public void SelectTurret(TurretData data)
    {
        if (IsSelected(data) || !CanSelect())
            return;

        selectedTurrets.Add(data);
        RefreshShopBar();
    }

    public void UnselectTurret(TurretData data)
    {
        if (!IsSelected(data))
            return;

        selectedTurrets.Remove(data);
        RefreshShopBar();

        foreach (var slot in loadoutSlots)
        {
            if (slot.turretData == data)
            {
                slot.Refresh();
                break;
            }
        }
    }

    public void RefreshShopBar()
    {
        foreach (var slot in shopSlots)
        {
            slot.GetComponentInChildren<ShopElement>().Clear();
            slot.SetActive(false);
        }

        for (int i = 0; i < selectedTurrets.Count; i++)
        {
            shopSlots[i].GetComponentInChildren<ShopElement>().Setup(selectedTurrets[i]);
            shopSlots[i].SetActive(true);
        }
    }
}

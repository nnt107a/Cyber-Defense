using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIBookManager : MonoBehaviour
{
    [Header("Nơi hiển thị thông tin")]
    public Image displayIcon;
    public TextMeshProUGUI displayNameText;
    public TextMeshProUGUI displayDescriptionText;
    
    [Header("Nơi hiển thị chỉ số")]
    public TextMeshProUGUI displayDamageText;
    public TextMeshProUGUI displayRangeText;
    public TextMeshProUGUI displayFireRateText;
    public TextMeshProUGUI displayCostText;

    // Hàm này được gọi từ các nút bấm
    public void UpdateTurretInfo(TurretData data)
    {
        if (data == null) return;

        // Cập nhật hình ảnh và tên
        if (displayIcon != null) displayIcon.sprite = data.turretIcon;
        if (displayNameText != null) displayNameText.text = data.turretName;
        if (displayDescriptionText != null) displayDescriptionText.text = data.description;

        // Cập nhật các chỉ số (Lưu ý: Tên biến đã khớp với TurretData mới)
        if (displayDamageText != null) displayDamageText.text = "DMG: " + data.attackDamage.ToString();
        if (displayRangeText != null) displayRangeText.text = "Range: " + data.range.ToString();
        if (displayFireRateText != null) displayFireRateText.text = "Speed: " + data.attackSpeed.ToString();
        if (displayCostText != null) displayCostText.text = "Cost: " + data.eCoreCost.ToString();
    }
}
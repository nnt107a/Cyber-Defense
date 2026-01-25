using UnityEngine;
using UnityEngine.UI;
using TMPro; // Nhớ dùng namespace này vì bạn dùng TextMeshPro

public class UITurretDetailView : MonoBehaviour
{
    [Header("Gắn các GameObject trong DetailArea vào đây")]
    public TextMeshProUGUI turretNameText;
    public Image turretImage;
    public TextMeshProUGUI turretDescriptionText;
    public TextMeshProUGUI statsText;

    // Hàm này sẽ được gọi bởi các nút con
    public void UpdateDisplay(TurretData data)
    {
        if (data != null)
        {
            // Cập nhật tên
            if(turretNameText) turretNameText.text = data.turretName; 

            // Cập nhật ảnh (Sprite)
            // Lưu ý: data.icon hoặc data.turretSprite tùy vào biến trong TurretData của bạn
            if(turretImage) turretImage.sprite = data.turretSprite; 

            // Cập nhật mô tả
            if(turretDescriptionText) turretDescriptionText.text = data.description;

            // Cập nhật chỉ số (ví dụ)
            if(statsText) statsText.text = $"Damage: {data.damage}\nRange: {data.range}";
        }
    }

    // Hàm dùng cho nút CloseButton
    public void ClosePanel()
    {
        gameObject.SetActive(false);
    }
}

using UnityEngine;
using UnityEngine.UI;
using TMPro; // Sử dụng TextMeshPro
using System.Collections.Generic;

public class UIBookManager : MonoBehaviour
{
    [Header("Data Source")]
    public List<TurretData> allTurrets; // Kéo thả 8 file Data vào đây

    [Header("Left Side - List Generation")]
    public Transform contentContainer;  // Nơi chứa các nút (Content của ScrollView)
    public UIBookDescriptionTurretButton buttonPrefab; // Prefab nút bấm ta sẽ tạo ở Bước 4

    [Header("Right Side - Detail View")]
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;
    public Image previewImage;          // Ảnh lớn hiển thị chi tiết
    public TextMeshProUGUI statsText;   // Hiển thị các chỉ số (Damage, Range...)

    private void Start()
    {
        GenerateButtons();

        // Mặc định hiển thị thông tin trụ đầu tiên khi mở sách
        if (allTurrets.Count > 0)
        {
            ShowTurretInfo(allTurrets[0]);
        }
    }

    // Tạo ra các nút dựa trên list data
    void GenerateButtons()
    {
        // Xóa các nút cũ (nếu có) để tránh trùng lặp
        foreach (Transform child in contentContainer)
        {
            Destroy(child.gameObject);
        }

        // Tạo nút mới
        foreach (TurretData data in allTurrets)
        {
            UIBookDescriptionTurretButton newBtn = Instantiate(buttonPrefab, contentContainer);
            newBtn.Setup(data, this);
        }
    }

    // Hàm hiển thị thông tin chi tiết (Được gọi từ nút con)
    public void ShowTurretInfo(TurretData data)
    {
        // Cập nhật text
        nameText.text = data.turretName;
        descriptionText.text = data.description;
        
        // Cập nhật ảnh lớn
        previewImage.sprite = data.turretIcon;
        previewImage.preserveAspect = true;

        // Cập nhật chỉ số (Bạn có thể trình bày đẹp hơn tùy ý)
        statsText.text = $"Sát thương: {data.damage}\n" +
                         $"Tầm bắn: {data.range}\n" +
                         $"Tốc độ bắn: {data.fireRate}\n" +
                         $"Giá: {data.cost}";
    }
}
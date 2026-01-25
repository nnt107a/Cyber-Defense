using UnityEngine;
using UnityEngine.UI;

public class UIBookDescriptionTurretButton : MonoBehaviour
{
    [Header("Components")]
    public Button btnFrame;        // Nút bấm (đóng vai trò là cái khung)
    public Image iconImage;        // Ảnh bên trong (Con của nút)

    private TurretData myData;     // Dữ liệu của trụ mà nút này đang giữ
    private UIBookManager bookManager; // Tham chiếu đến quản lý

    // Hàm này được gọi từ Manager khi tạo nút
    public void Setup(TurretData data, UIBookManager manager)
    {
        myData = data;
        bookManager = manager;

        // Gán hình ảnh từ data vào Image con
        if (data.turretIcon != null)
        {
            iconImage.sprite = data.turretIcon;
            // Đảm bảo icon giữ đúng tỷ lệ
            iconImage.preserveAspect = true; 
        }

        // Lắng nghe sự kiện click
        btnFrame.onClick.RemoveAllListeners();
        btnFrame.onClick.AddListener(OnButtonClick);
    }

    void OnButtonClick()
    {
        // Khi click, bảo manager hiển thị thông tin của myData
        bookManager.ShowTurretInfo(myData);
    }
}
using UnityEngine;
using UnityEngine.UI;

public class UIBookDescriptionTurretButton : MonoBehaviour
{
    [Header("Dữ liệu trụ cho nút này")]
    public TurretData turretData; // Kéo file data trụ vào đây

    [Header("Tham chiếu")]
    [SerializeField] private Image buttonIconImage; // Ảnh trên nút
    [SerializeField] private UIBookManager bookManager; // Kéo UIBookManager vào

    private void Start()
    {
        // Tự động gán ảnh cho nút nếu có data
        if (turretData != null && buttonIconImage != null)
        {
            buttonIconImage.sprite = turretData.turretIcon; // Lấy icon từ data
        }

        // Bắt sự kiện click
        GetComponent<Button>().onClick.AddListener(DisplayInfo);
    }

    void DisplayInfo()
    {
        if (bookManager != null && turretData != null)
        {
            // GỌI HÀM UpdateTurretInfo (Khớp với bên UIBookManager)
            bookManager.UpdateTurretInfo(turretData);
        }
        else
        {
            Debug.LogWarning("Chưa gán BookManager hoặc TurretData cho nút: " + gameObject.name);
        }
    }
}
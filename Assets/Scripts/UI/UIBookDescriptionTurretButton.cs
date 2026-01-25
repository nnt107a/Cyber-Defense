using UnityEngine;
using UnityEngine.UI;

public class UIBookDescriptionTurretButton : MonoBehaviour
{
    [Header("Gán Panel chứa thông tin vào đây")]
    public GameObject turretBookPanel; 

    private Button btn;

    void Start()
    {
        btn = GetComponent<Button>();
        // Khi click nút này sẽ gọi hàm OpenBook
        btn.onClick.AddListener(OpenBook);
    }

    void OpenBook()
    {
        if(turretBookPanel != null)
        {
            turretBookPanel.SetActive(true);
            // Có thể thêm âm thanh mở sách ở đây
        }
    }
}
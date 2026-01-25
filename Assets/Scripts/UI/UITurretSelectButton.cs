using UnityEngine;
using UnityEngine.UI;

public class UITurretSelectButton : MonoBehaviour
{
    [Header("Dữ liệu của trụ này")]
    public TurretData myData; // Kéo file TurretData (ScriptableObject) vào đây

    [Header("Panel cha quản lý hiển thị")]
    public UITurretDetailView detailView; 

    private Button btn;

    void Start()
    {
        btn = GetComponent<Button>();
        btn.onClick.AddListener(OnButtonClick);
        
        // Tự động gán hình ảnh của nút thành hình icon trụ (nếu muốn)
        if(myData != null && GetComponent<Image>() != null)
        {
            GetComponent<Image>().sprite = myData.turretSprite;
        }
    }

    void OnButtonClick()
    {
        if (detailView != null && myData != null)
        {
            // Gửi dữ liệu của chính mình lên cho Panel hiển thị
            detailView.UpdateDisplay(myData);
        }
    }
}
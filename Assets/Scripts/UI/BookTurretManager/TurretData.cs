using UnityEngine;

[CreateAssetMenu(fileName = "NewTurretData", menuName = "ScriptableObjects/Turret Data")]
public class TurretData : ScriptableObject
{
    [Header("Thông tin cơ bản")]
    public string turretName;       // Tên trụ
    [TextArea(3, 5)]
    public string description;      // Mô tả cốt truyện hoặc công dụng
    public Sprite turretIcon;       // Hình ảnh đại diện (Icon)

    [Header("Chỉ số chiến đấu")]
    public float damage;            // Sát thương
    public float range;             // Tầm bắn
    public float fireRate;          // Tốc độ bắn
    public int cost;                // Giá mua
    public int upgradeCost;         // Giá nâng cấp
}
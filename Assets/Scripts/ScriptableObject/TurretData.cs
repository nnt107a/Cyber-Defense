using UnityEngine;

[CreateAssetMenu(fileName = "NewTurretData", menuName = "Turret/TurretData")]
public class TurretData : ScriptableObject
{
    [Header("Thông tin hiển thị trên sách (UI)")]
    public string turretName;       // Tên trụ hiển thị
    [TextArea(3, 5)] 
    public string description;      // Mô tả trụ
    public Sprite turretIcon;       // Hình ảnh icon của trụ

    [Header("Chỉ số trong Game")]
    public int eCoreCost;           // Giá tiền (Tên cũ là eCoreCost)
    public float rechargeTime;      // Thời gian hồi chiêu (Cần cho Shop)
    public float maxHealth;         // Máu tối đa (Cần cho Turret)
    
    public float attackDamage;      // Sát thương (Tên cũ là attackDamage)
    public float attackSpeed;       // Tốc độ bắn (Tên cũ là attackSpeed)
    public float range;             // Tầm bắn
    
    [Header("Prefabs")]
    public GameObject turretPrefab;    // Prefab trụ
    public GameObject projectilePrefab; // Prefab đạn (Tên cũ là projectilePrefab)
}
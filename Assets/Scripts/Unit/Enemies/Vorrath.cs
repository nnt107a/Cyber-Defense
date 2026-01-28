using System.Collections.Generic;
using UnityEngine;

public class Vorrath : Enemy
{
    [Header("Vorrath Movement")]
    [Tooltip("Khoảng cách boss đi vào map (tính theo đơn vị Unity, ví dụ 1.0 = 1 ô)")]
    [SerializeField] private float moveDistance = 1.0f; // Đi vào 1 ô rồi dừng

    [Header("Summon Skills")]
    [SerializeField] private List<GameObject> minionPrefabs; // Danh sách quái con có thể gọi
    [SerializeField] private float summonInterval = 3f;      // Thời gian hồi chiêu gọi đệ
    [SerializeField] private int minSpawnCount = 1;          // Số lượng tối thiểu mỗi lần gọi
    [SerializeField] private int maxSpawnCount = 3;          // Số lượng tối đa mỗi lần gọi

    private Vector3 startPosition;
    private bool hasReachedDestination = false; 
    private float summonTimer;

    protected override void Awake()
    {
        base.Awake();
        // Không set startPosition ở đây để tránh lỗi khi dùng Object Pooling
    }

    // Hàm này chạy mỗi khi Boss được sinh ra (Override từ Enemy.cs)
    public new void OnSpawn()
    {
        base.OnSpawn();
        startPosition = transform.position;
        hasReachedDestination = false;
        summonTimer = summonInterval;
    }

    // GHI ĐÈ hàm Update để dùng logic riêng của Boss thay vì logic đi thẳng của Enemy thường
    protected override void Update()
    {
        // 1. Kiểm tra điều kiện chung (Game đang chạy, Boss chưa chết)
        if (!GameManager.Instance.isLevelOnGoing || isDying) return;

        // 2. Logic di chuyển hoặc đứng yên
        if (!hasReachedDestination)
        {
            HandleMovementPhase();
        }
        else
        {
            HandleSummonPhase();
        }
    }

    private void HandleMovementPhase()
    {
        // Boss tự di chuyển sang trái (không dùng StateRun để dễ kiểm soát việc dừng lại)
        transform.position += Vector3.left * enemyData.moveSpeed * Time.deltaTime;
        
        if (animator != null) animator.SetBool("isRunning", true);

        // Kiểm tra khoảng cách đã đi
        if (Vector3.Distance(startPosition, transform.position) >= moveDistance)
        {
            StopMovement();
        }
    }

    private void StopMovement()
    {
        hasReachedDestination = true;
        
        // Dừng vật lý và Animation
        if (rb != null) rb.linearVelocity = Vector2.zero; 
        if (animator != null) animator.SetBool("isRunning", false);
    }

    private void HandleSummonPhase()
    {
        // Boss đứng yên, đếm ngược thời gian để gọi đệ
        summonTimer -= Time.deltaTime;

        if (summonTimer <= 0)
        {
            // Reset timer
            summonTimer = summonInterval;
            
            // Thực hiện tấn công (Triệu hồi)
            Attack();
        }
    }

    // Ghi đè hàm Attack để thay vì bắn đạn thì gọi quái
    public override void Attack()
    {
        // Chạy animation Attack
        if (animator != null) animator.SetTrigger("attack");

        // Gọi hàm sinh quái
        SpawnMinions();
    }

    private void SpawnMinions()
    {
        if (minionPrefabs == null || minionPrefabs.Count == 0) return;
        
        // Random số lượng quái sẽ gọi
        int count = Random.Range(minSpawnCount, maxSpawnCount + 1);

        // Lấy số hàng (lane) từ GridManager để đảm bảo chính xác
        int totalLanes = GridManager.height; // Dựa vào GridManager.cs bạn cung cấp

        for (int i = 0; i < count; i++)
        {
            // A. Chọn ngẫu nhiên loại quái và hàng (Lane)
            GameObject prefabToSpawn = minionPrefabs[Random.Range(0, minionPrefabs.Count)];
            int randomLaneIndex = Random.Range(0, totalLanes);

            // B. TÍNH VỊ TRÍ CHUẨN DỰA TRÊN GRID MANAGER
            // Lấy tham chiếu ô grid tại (x=0, y=randomLane) để biết toạ độ Y chính xác của lane đó
            GridCell targetCell = GridManager.Instance.GetCell(0, randomLaneIndex);
            
            if (targetCell != null)
            {
                // Lấy Y của lane từ GridCell
                float spawnY = targetCell.transform.position.y;
                
                // Vị trí X spawn ngay tại vị trí của Boss (hoặc thụt lùi 1 chút tùy ý)
                float spawnX = transform.position.x;

                Vector3 spawnPos = new Vector3(spawnX, spawnY, 0);

                // C. Sinh quái bằng ObjectPool
                GameObject minionObj = ObjectPool.Instance.Spawn(prefabToSpawn, spawnPos, Quaternion.identity);

                // D. SETUP QUÁI CON (Quan trọng để nó chạy đúng đường)
                Enemy minionScript = minionObj.GetComponent<Enemy>();
                if (minionScript != null)
                {
                    // 1. Reset thông số quái
                    minionScript.OnSpawn();
                    
                    // 2. Gán lane cho quái (Hàm Place trong Enemy.cs sẽ lo việc thêm vào GameManager)
                    minionScript.Place(randomLaneIndex);
                }
            }
        }
    }
}
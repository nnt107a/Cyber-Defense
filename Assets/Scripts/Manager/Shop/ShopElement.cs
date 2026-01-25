using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopElement : MonoBehaviour
{
    public GameObject towerPrefab;
    [SerializeField] private TextMeshProUGUI towerCostText;
    [SerializeField] private Image filler;

    private float rechargeTime;
    private float rechargeTimer = 0f;

    private void Awake()
    {
        rechargeTime = towerPrefab.GetComponent<Turret>().turretData.rechargeTime;
        rechargeTimer = rechargeTime;
    }

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(() => 
        {
            if (GameManager.Instance.isTransitioningAfterChoosingLoadout)
            {
                return;
            }
            if (GameManager.Instance.isLevelOnGoing == false)
            {
                LoadoutManager.Instance.UnselectTurret(towerPrefab?.GetComponent<Turret>().turretData);
                return;
            }
            if (LevelManager.Instance.eCoreCount < towerPrefab?.GetComponent<Turret>().turretData.eCoreCost || rechargeTimer < rechargeTime)
            {
                return;
            }
            UnitDragHandler.Instance.shopElement = null;
            if (UnitDragHandler.Instance.dragPreview != null && UnitDragHandler.Instance.dragPreview.gameObject.activeInHierarchy)
            {
                Destroy(UnitDragHandler.Instance.dragPreview);
            }
            UnitDragHandler.Instance.BeginDrag(gameObject);
        });
        towerCostText.text = towerPrefab?.GetComponent<Turret>().turretData.eCoreCost.ToString();
    }
    private void Update()
    {
        rechargeTimer += Time.deltaTime;
        filler.fillAmount = Mathf.Clamp01(1f - rechargeTimer / rechargeTime);

        if (LevelManager.Instance.eCoreCount < towerPrefab?.GetComponent<Turret>().turretData.eCoreCost && GameManager.Instance.isLevelOnGoing)
        {
            towerCostText.color = Color.darkRed;
        }
        else
        {
            towerCostText.color = Color.white;
        }
    }
    public void Recharge()
    {
        rechargeTimer = 0f;
    }
    public void Setup(TurretData data)
    {
        towerPrefab = data.turretPrefab;

        rechargeTime = data.rechargeTime;
        rechargeTimer = rechargeTime;

        GetComponent<Image>().sprite =
            data.turretIcon;

        towerCostText.text = data.eCoreCost.ToString();
    }

    public void Clear()
    {
        towerPrefab = null;
    }

}

using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public enum TechState
{
    Locked,
    Available,
    Researched,
}

public class TechNode : MonoBehaviour
{
    [Header("UI Components")]
    public Image backgroundImage;
    public Image techIconImage;
    public Button nodeButton;
    public Image borderGlowImage;
    [Header("Settings - Colors")]
    public Material techLockedMaterial;
    public Color colorLocked = Color.gray;
    public Color colorAvailable = Color.cyan;
    public Color colorResearchedGlow = Color.yellow;

    [Header("Data")]
    public TechData techData;
    public TechState currentState;

    private void Start()
    {
        techIconImage.sprite = techData.techIcon;
        GetComponent<Button>()
            .onClick.AddListener(() => TechManager.Instance.ShowTechInfo(techData));
    }

    public void Setup(TechData data)
    {
        techData = data;
        techIconImage.sprite = data.techIcon;
    }

    public void SetState(TechState newState)
    {
        currentState = newState;

        switch (currentState)
        {
            case TechState.Locked:
                backgroundImage.material = techLockedMaterial;
                techIconImage.material = techLockedMaterial;
                techIconImage.color = Color.gray;
                break;

            case TechState.Available:
                techIconImage.color = Color.white;
                backgroundImage.material = null;
                if (borderGlowImage != null)
                {
                    borderGlowImage.gameObject.SetActive(true);
                    borderGlowImage.color = Color.white;
                    // Hiệu ứng "Thở" (Fade in/out nhẹ)
                    borderGlowImage.DOFade(0.5f, 0.8f).SetLoops(-1, LoopType.Yoyo);
                }
                break;

            case TechState.Researched:
                backgroundImage.material = null;
                techIconImage.material = null;
                techIconImage.color = Color.white;
                if (borderGlowImage != null)
                {
                    borderGlowImage.gameObject.SetActive(true);
                    borderGlowImage.color = colorResearchedGlow;

                    // Reset Alpha về 1 (nếu lỡ bị hiệu ứng Fade của Available làm mờ)
                    var c = borderGlowImage.color;
                    c.a = 1f;
                    borderGlowImage.color = c;

                }
                break;
        }
    }
}
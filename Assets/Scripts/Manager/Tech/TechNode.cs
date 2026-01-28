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
    public Image techIconImage;
    public Button nodeButton;
    public Image borderGlowImage;
    public Image overlayImage;
    [Header("Settings - Colors")]
    public Color colorLocked = Color.gray;
    public Color colorResearched = Color.cyan;
    public Color colorResearchedGlow = Color.yellow;

    [Header("Data")]
    public TechData techData;
    public TechState currentState;

    private void Start()
    {
        techIconImage.sprite = techData.techIcon;
        GetComponent<Button>()
            .onClick.AddListener(() => OnClick());
    }

    public void OnClick()
    {
        UITechTree controller = FindObjectOfType<UITechTree>();
        if (controller != null)
        {
            controller.OnTechNodeClicked(techData);
        }
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
                overlayImage.color = colorLocked;
                break;

            case TechState.Available:
                overlayImage.color = Color.clear;
                if (borderGlowImage != null)
                {
                    borderGlowImage.gameObject.SetActive(true);
                    borderGlowImage.color = Color.white;
                    // Hiệu ứng "Thở" (Fade in/out nhẹ)
                    borderGlowImage.DOFade(0.5f, 0.8f).SetLoops(-1, LoopType.Yoyo);
                }
                break;

            case TechState.Researched:
                overlayImage.color = colorResearched;
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
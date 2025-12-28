using UnityEngine;
using UnityEngine.EventSystems;

public class UISliderSoundHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private bool isDragging = false;

    public void OnPointerDown(PointerEventData eventData)
    {
        isDragging = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!isDragging) return;

        isDragging = false;
        SoundManager.Instance?.PlayUIClick();
    }
}

using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

public class UIButton : Button
{
    [Inject] private AudioManager audioManager;

    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);

        audioManager.PlaySound("Click");
    }
}

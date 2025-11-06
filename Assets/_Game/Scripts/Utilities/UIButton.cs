using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

public class UIButton : Button
{
    [SerializeField] private GameObject blockImg;

    [Inject] private AudioManager audioManager;

    #region Unity Methods
    protected override void OnValidate()
    {
        base.OnValidate();
        UpdateBlockImage();
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);

        if (interactable) audioManager.PlaySound("Click");
    }

    #endregion

    #region Public Methods

    public void SetInteractable(bool interactable = true)
    {
        this.interactable = interactable;
        UpdateBlockImage();
    }

    #endregion

    #region Private Methods

    private void UpdateBlockImage()
    {
        if (blockImg != null)
        {
            blockImg.SetActive(!interactable);
        }
    }
    #endregion
}

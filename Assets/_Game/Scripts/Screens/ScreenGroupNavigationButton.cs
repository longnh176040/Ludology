using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenGroupNavigationButton : MonoBehaviour
{
	#region Inspector Variables

	[SerializeField] private GameObject selectedImg;
	[SerializeField] private RectTransform iconRect;

    #endregion

    #region Member Variables

    private float minSize = 132f;
    private float maxSize = 190f;
    private float selectedY = 30f;
    private float unselectedY = 0f;
    private float lerpDuration = 0.1f;

    private Coroutine currentAnim;

    #endregion

    #region Public Methods

    public void SetSelected(bool isSelected)
	{
        ShowSelectImage(isSelected);
        ShowIcon(isSelected);
	}

    #endregion

    #region Private Methods

    private void ShowSelectImage(bool isSelected)
    {
        selectedImg.SetActive(isSelected);
    }

    private void ShowIcon(bool isSelected)
    {
        if (currentAnim != null)
            StopCoroutine(currentAnim);

        float targetSize = isSelected ? maxSize : minSize;
        float targetY = isSelected ? selectedY : unselectedY;

        currentAnim = StartCoroutine(AnimateIcon(targetSize, targetY));
    }

    private IEnumerator AnimateIcon(float targetSize, float targetY)
    {
        Vector2 startSize = iconRect.sizeDelta;
        Vector2 endSize = new Vector2(targetSize, targetSize);

        Vector2 startPos = iconRect.anchoredPosition;
        Vector2 endPos = new Vector2(startPos.x, targetY);

        float elapsed = 0f;

        while (elapsed < lerpDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / lerpDuration);

            iconRect.sizeDelta = Vector2.Lerp(startSize, endSize, t);
            iconRect.anchoredPosition = Vector2.Lerp(startPos, endPos, t);

            yield return null;
        }

        // Snap to final values
        iconRect.sizeDelta = endSize;
        iconRect.anchoredPosition = endPos;
    }

    #endregion
}

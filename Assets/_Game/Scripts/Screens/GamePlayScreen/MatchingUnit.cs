using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class MatchingUnit : MonoBehaviour
{
    #region Inspector Variables

    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private Image bgImg;

    [SerializeField] private TeamColor color;
    [SerializeField] private AnchorConstraint anchorConstraint;
    [SerializeField] private PlayerFrame playerFrame;

    #endregion

    #region Member Variables

    [Inject] private GameController gameController;
    [Inject] private ScreenManager screenManager;
    private Vector2 anchorPosition;

    #endregion

    #region Unity Methods
    void Start()
    {
        bgImg.color = gameController.GetColor(color);
        anchorPosition = screenManager.GetAnchorPosition(anchorConstraint);
        playerFrame.SetPosition(anchorPosition / 2);
        playerFrame.Color = color;
    }

    #endregion

    #region Public Methods
    public void PlayInAnimation(float duration = 1f)
    {
        rectTransform.anchoredPosition = anchorPosition * 2;
        PlayAnimation(anchorPosition, duration, ()=>
        {
            playerFrame.RandomFakePlayer();
        });
    }

    public void PlayOutAnimation(float duration = 1f)
    {
        Vector2 target = anchorPosition * 2;
        PlayAnimation(target, duration);
    }

    #endregion

    #region Private Methods

    private void PlayAnimation(Vector3 targetPos, float duration = 1f, Action callback = null)
    {
        StartCoroutine(RectTransformExtensions.IELerpRectTransform(rectTransform, targetPos, duration, callback));
    }

    #endregion
}

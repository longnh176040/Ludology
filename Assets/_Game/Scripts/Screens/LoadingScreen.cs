using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingScreen : MonoBehaviour
{
    #region Inspector Variables
    [SerializeField] private CanvasGroup canvasGroup;
    #endregion

    #region Public Methods

    public void Show(float time)
    {
        canvasGroup.alpha = 1;
        gameObject.SetActive(true);

    }

    #endregion
}

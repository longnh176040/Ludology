using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class MatchingScreen : MonoBehaviour
{
    #region Inspector Variales    

    [SerializeField] private Image bgImg;
    [SerializeField] private CanvasGroup cg;

    [SerializeField] private GameObject mainGameObj;

    [SerializeField] private MatchingUnit[] matchingUnit;
    [SerializeField] private Animator vsAnimator;

    #endregion

    #region Member Variables

    [Inject] private SignalBus signalBus;

    #endregion

    #region Public Methods

    public void OnStartGameClick()
    {
        mainGameObj.SetActive(false);
        cg.alpha = 1;
        bgImg.enabled = true;
        StartCoroutine(IE_PlayMatchingAnimation());
    }

    #endregion

    private IEnumerator IE_PlayMatchingAnimation(float duration = 2.5f)
    {
        foreach (MatchingUnit unit in matchingUnit)
        {
            unit.PlayInAnimation(duration);
        }

        vsAnimator.Play("VS_Show");

        yield return new WaitForSeconds(duration + 2f);

        bgImg.enabled = false;
        mainGameObj.SetActive(true);

        foreach (MatchingUnit unit in matchingUnit)
        {
            unit.PlayOutAnimation(duration);
        }

        vsAnimator.Play("VS_Hide");
        yield return new WaitForSeconds(.5f);

        StartCoroutine(CanvasGroupExtensions.IELerpAlpha(cg, 0, 0.5f));

        signalBus.Fire(new StartMainGameSignal());
    }
}

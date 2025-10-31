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
    [Inject] protected AudioManager audioManager;

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

    private IEnumerator IE_PlayMatchingAnimation(float animDuration = 2.5f)
    {
        float maxFindOpponentTime = float.MinValue;
        foreach (MatchingUnit unit in matchingUnit)
        {
            float minFODuration = 0.5f;
            float maxFODuration = 2f;
            float FODuration = Random.Range(minFODuration, maxFODuration);
            if (FODuration > maxFindOpponentTime) maxFindOpponentTime = FODuration;
            unit.PlayInAnimation(animDuration, FODuration);
        }

        vsAnimator.Play("VS_Show");

        yield return new WaitForSeconds(animDuration + maxFindOpponentTime + .5f);
        audioManager.PlaySound("Match");

        bgImg.enabled = false;
        mainGameObj.SetActive(true);
        vsAnimator.Play("VS_Hide");
        yield return new WaitForSeconds(.35f);

        foreach (MatchingUnit unit in matchingUnit)
        {
            unit.PlayOutAnimation(animDuration);
        }

        StartCoroutine(CanvasGroupExtensions.IELerpAlpha(cg, 0, 0.25f));

        signalBus.Fire(new StartMainGameSignal());
    }
}

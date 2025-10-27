using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMonoBehaviour : MonoBehaviour
{
    #region Properties

    public RectTransform RectT { get { return transform as RectTransform; } }

	public CanvasGroup CG
	{
		get
		{
			if (canvasGroup == null)
			{
				canvasGroup = gameObject.GetComponent<CanvasGroup>();

				if (canvasGroup == null)
				{
					canvasGroup = gameObject.AddComponent<CanvasGroup>();
				}
			}

			return canvasGroup;
		}
	}

	#endregion

	#region Member Variables

	private CanvasGroup canvasGroup;

	#endregion
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomScreenGroup : CustomScreen
{
	#region Classes

	[System.Serializable]
	private class SubScreen
	{
		public CustomScreen screen = null;
		public ScreenGroupNavigationButton navButton = null;
	}

	#endregion

	#region Inspector Variables

	[Space]
	[SerializeField] private List<SubScreen> subScreens = null;
	[SerializeField] private int mainSubScreenId;
	#endregion

	#region Member Variables

	private SubScreen currentSubScreen;

	#endregion

	#region Public Methods

	public override void Initialize()
	{
		base.Initialize();

		if (subScreens.Count > 0)
		{
			RectTransform screensContainer = ScreenManager.CreateScreenContainer(transform, siblingIndex: 0);

			for (int i = 0; i < subScreens.Count; i++)
			{
				SubScreen subScreen = subScreens[i];

				subScreen.screen.transform.SetParent(screensContainer, false);

				subScreen.screen.Initialize();
				subScreen.screen.gameObject.SetActive(true);
				subScreen.screen.Hide(false, true);
			}
		}
	}

	public void ShowSubScreen(string subScreenId)
	{
		ShowSubScreen(subScreenId, false);
	}

	public void ShowSubScreen(string subScreenId, bool immediate)
	{
		SubScreen subScreen = GetSubScreen(subScreenId);

		if (subScreen != null && currentSubScreen != subScreen)
		{
			ShowSubScreen(subScreen, immediate);
		}
	}

	public override void OnShowing(bool back)
	{
		base.OnShowing(back);

		if (currentSubScreen != null)
		{
			currentSubScreen.screen.OnShowing(back);
		}
		else
		{
			ShowSubScreen(subScreens[mainSubScreenId], true);
		}
	}

	public override void OnHiding()
	{
		base.OnHiding();

		if (currentSubScreen != null)
		{
			currentSubScreen.screen.OnHiding();
		}
	}

	public CustomScreen GetSubCustomScreen(string screenId)
    {
		for (int i = 0; i < subScreens.Count; i++)
		{
			SubScreen subScreen = subScreens[i];

			if (subScreen.screen.Id == screenId)
			{
				return subScreen.screen;
			}
		}

		return null;
	}

	#endregion

	#region Private Methods

	private SubScreen GetSubScreen(string screenId)
	{
		for (int i = 0; i < subScreens.Count; i++)
		{
			SubScreen subScreen = subScreens[i];

			if (subScreen.screen.Id == screenId)
			{
				return subScreen;
			}
		}

		return null;
	}

	private void ShowSubScreen(SubScreen subScreen, bool immediate)
	{
		bool transitionLeft = currentSubScreen == null || subScreens.IndexOf(currentSubScreen) > subScreens.IndexOf(subScreen);

		if (currentSubScreen != null)
		{
			currentSubScreen.screen.Hide(transitionLeft, immediate);

			if (currentSubScreen.navButton != null)
			{
				currentSubScreen.navButton.SetSelected(false);
			}
		}

		subScreen.screen.Show(transitionLeft, immediate);

		if (subScreen.navButton != null)
		{
			subScreen.navButton.SetSelected(true);
		}

		currentSubScreen = subScreen;
	}

	#endregion
}

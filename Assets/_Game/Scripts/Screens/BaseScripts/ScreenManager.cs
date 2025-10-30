using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ScreenManager : MonoBehaviour
{
	#region Inspector Variables

	[SerializeField] private string mainScreenId = "";
	[SerializeField] private List<CustomScreen> screens = null;

	[SerializeField] private LoadingScreen loadingScreen;

	#endregion

	#region Member Variables

	// The screen that is currently being shown
	private CustomScreen currentScreen;

	[Inject] private AudioManager audioManager;

	#endregion

	#region Properties

	public string MainScreenId { get { return mainScreenId; } }
	public string CurrentScreenId { get { return currentScreen == null ? "" : currentScreen.Id; } }

	/// <summary>
	/// Invoked when the ScreenController is transitioning from one screen to another. The first argument is the current showing screen id, the
	/// second argument is the screen id of the screen that is about to show (null if its the first screen). The third argument id true if the screen
	/// that is being show is an overlay
	/// </summary>
	public System.Action<string, string> OnSwitchingScreens;

	/// <summary>
	/// Invoked when ShowScreen is called
	/// </summary>
	public System.Action<string> OnShowingScreen;

	#endregion

	#region Unity Methods

	private void Start()
	{
		if (screens.Count == 0)
		{
			return;
		}

		CustomScreen firstScreen = screens[0];

		CreateScreenContainer(firstScreen.transform.parent, screens, firstScreen.transform.GetSiblingIndex());

		// Initialize and hide all the screens
		for (int i = 0; i < screens.Count; i++)
		{
			CustomScreen screen = screens[i];

			// Add a CanvasGroup to the screen if it does not already have one
			if (screen.gameObject.GetComponent<CanvasGroup>() == null)
			{
				screen.gameObject.AddComponent<CanvasGroup>();
			}

			// Force all screens to hide right away
			screen.Initialize();
			screen.gameObject.SetActive(true);
			screen.Hide(false, true);
		}

		// Show the home screen when the app starts up
		Show(MainScreenId, false, true);
		audioManager.PlayMusic("bg_music");
	}

    #endregion

    #region Public Methods

    #region Screen Function
    public static RectTransform CreateScreenContainer(Transform containerParent, List<CustomScreen> screensInContainer = null, int siblingIndex = -1)
	{
		GameObject screenContainerObj = new GameObject("screen_container");
		RectTransform screenContainerRect = screenContainerObj.AddComponent<RectTransform>();

		screenContainerRect.SetParent(containerParent, false);

		screenContainerRect.anchorMin = Vector2.zero;
		screenContainerRect.anchorMax = Vector2.one;
		screenContainerRect.offsetMin = Vector2.zero;
		screenContainerRect.offsetMax = Vector2.zero;

		if (screensInContainer != null)
		{
			for (int i = 0; i < screensInContainer.Count; i++)
			{
				screensInContainer[i].transform.SetParent(screenContainerRect, false);
			}
		}

		if (siblingIndex > -1)
		{
			screenContainerRect.SetSiblingIndex(siblingIndex);
		}

		return screenContainerRect;
	}

	public void Show(string screenId)
	{
		if (CurrentScreenId == screenId)
		{
			return;
		}

		Show(screenId, false, false);
	}

	public void ShowSubScreen(string screenId, string subScreenId)
	{
		CustomScreen screen = GetScreenById(screenId);

		if (screen != null)
		{
			(screen as CustomScreenGroup).ShowSubScreen(subScreenId, false);
		}
	}

	public CustomScreen GetScreenById(string id)
	{
		for (int i = 0; i < screens.Count; i++)
		{
			if (id == screens[i].Id)
			{
				return screens[i];
			}
		}

		Debug.LogError("[ScreenManager] No Screen exists with the id " + id);

		return null;
	}

	public void ShowLoading()
    {
		loadingScreen.Show(3f);
    }

	#endregion

	#region Anchoring Function

	public Vector2 GetAnchorPosition(AnchorConstraint constraint)
	{
		float widthCenter = GameManager.ScreenWidth / 2;
		float heightCenter = GameManager.ScreenHeight / 2;

		switch (constraint) 
		{
			case AnchorConstraint.TOP_LEFT_CORNER:
				return new Vector2 (-widthCenter, heightCenter);
			case AnchorConstraint.TOP_RIGHT_CORNER:
				return new Vector2 (widthCenter, heightCenter);
			case AnchorConstraint.BOTTOM_LEFT_CORNER: 
				return new Vector2 (-widthCenter, -heightCenter);
			case AnchorConstraint.BOTTOM_RIGHT_CORNER: 
				return new Vector2 (widthCenter, -heightCenter);
		}
		return Vector2.zero;
	}

    #endregion

    #endregion

    #region Private Methods

    private void Show(string screenId, bool back, bool immediate)
	{
		// Get the screen we want to show
		CustomScreen screen = GetScreenById(screenId);

		if (screen == null)
		{
			Debug.LogError("[ScreenController] Could not find screen with the given screenId: " + screenId);

			return;
		}

		// Check if there is a current screen showing
		if (currentScreen != null)
		{
			// Hide the current screen
			currentScreen.Hide(back, immediate);

			if (OnSwitchingScreens != null)
			{
				OnSwitchingScreens(currentScreen.Id, screenId);
			}
		}

		// Show the new screen
		screen.Show(back, immediate);

		// Set the new screen as the current screen
		currentScreen = screen;

		if (OnShowingScreen != null)
		{
			OnShowingScreen(screenId);
		}
	}

	#endregion

}
public enum AnchorConstraint
{
    TOP_LEFT_CORNER,
    TOP_RIGHT_CORNER,
    BOTTOM_LEFT_CORNER,
    BOTTOM_RIGHT_CORNER
}

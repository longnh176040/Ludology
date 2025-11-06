using UnityEngine;

public class GameScreen : CustomScreen
{
    #region Inspector Variables
    
    [Space]
    [SerializeField] private DiceButton diceBtn;
    [SerializeField] private UIButton[] skillsBtns;
    [SerializeField] private UIButton luckyBtn;

    #endregion

    #region Public Methods

    public override void Initialize()
    {
        base.Initialize();
        HideControlPanel();
    }

    public override void OnShowing(bool back)
    {
        base.OnShowing(back);
    }

    public void ShowControlPanel()
    {
        ActiveControlPanel();
    }

    public void HideControlPanel()
    {
        ActiveControlPanel(false);
    }

    #endregion

    #region Private Methods

    public void ActiveControlPanel(bool active = true)
    {
        diceBtn.SetInteractable(active);
        foreach (UIButton b in skillsBtns)
        {
            b.interactable = active;
        }

        //TODO: Check item availability
        luckyBtn.interactable = active;
    }

    #endregion
}

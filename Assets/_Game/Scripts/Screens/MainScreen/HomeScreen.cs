using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HomeScreen : CustomScreen
{
    #region Inspector Variables

    [Space]
    [SerializeField] private List<UIButton> buttonHandlers;

    #endregion

    #region Member Variables

    private List<IButtonHandler> buttonHandlersList = new ();

    #endregion

    #region Unity Methods

    private void OnDestroy()
    {
        foreach (var button in buttonHandlersList)
            button.Dispose();
    }

    #endregion

    #region Public Methods

    public override void Initialize()
    {
        base.Initialize();

        foreach (var button in buttonHandlers)
            if (button.TryGetComponent(out IButtonHandler buttonHandler))
                buttonHandlersList.Add(buttonHandler);

        foreach (var button in buttonHandlersList)
            button.Initialize();
    }

    #endregion
}

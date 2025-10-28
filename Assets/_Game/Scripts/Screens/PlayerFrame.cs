using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerFrame : MonoBehaviour
{

    #region Inspector Variables

    [SerializeField] private Image characterImage;
    [SerializeField] private TextMeshProUGUI turnTxt;
    [SerializeField] private Image frameImage;

    #endregion

    #region Public Methods

    public void SetPlayerTurn(bool isPlayer = false)
    {
        //TODO: change character image if the player purchased a new avatar

        turnTxt.text = isPlayer ? "YOU" : "PLAYER";
        frameImage.color = isPlayer ? Color.red : Color.white;
    }

    #endregion

}

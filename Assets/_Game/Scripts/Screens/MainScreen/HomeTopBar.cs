using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class HomeTopBar : MonoBehaviour
{
    #region Inspector Variables

    [SerializeField] private PlayerFrame playerFrame;
    [SerializeField] private TextMeshProUGUI nameTxt;
    [SerializeField] private TextMeshProUGUI levelTxt;
    [SerializeField] private TextMeshProUGUI expTxt;
    [SerializeField] private Image expImg;

    [SerializeField] private TextMeshProUGUI energyTxt;
    [SerializeField] private TextMeshProUGUI energyCounterTxt;

    [SerializeField] private TextMeshProUGUI diamondTxt;

    #endregion

    #region Member Variables

    [Inject] private DataManager dataManager;

    #endregion

    #region Public Methods

    public void Load()
    {
        playerFrame.ShowPlayerInfo();
        nameTxt.text = dataManager.playerInfoData.info.name;
        levelTxt.text = dataManager.playerInfoData.level.ToString();
        expTxt.text = dataManager.playerInfoData.exp.ToString();

        energyTxt.text = dataManager.playerInfoData.energy + "/" + Constant.MAX_ENERGY;
        diamondTxt.text = dataManager.playerInfoData.diamond.ToString();
    }

    #endregion

    #region Private Methods

    private void UpdateEnergyBar(float amount)
    {
        
    }

    #endregion
}

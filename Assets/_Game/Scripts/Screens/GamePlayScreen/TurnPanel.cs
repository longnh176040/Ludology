using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class TurnPanel : MonoBehaviour
{
    #region Inspector Variables

    [Header("Turn Info")]
    [SerializeField] private PlayerFrame playerFrame; 
    [SerializeField] private Image labelImg;
    [SerializeField] private TextMeshProUGUI labelTxt;

    [Header("Turn Announcement")]
    [SerializeField] private Image turnBgImg;
    [SerializeField] private Image characterImg;
    [SerializeField] private Animation animClip;
    [SerializeField] private TextMeshProUGUI turnAnnounceTxt;


    #endregion

    #region Member Variables

    [Inject] private GameController gameController;
    [Inject] private FrameDataManager frameDataManager;
    [Inject] private AudioManager audioManager;

    #endregion

    #region Public Methods

    public void SetTurnPanel(PlayerInfo playerInfo, TeamColor teamColor)
    {
        playerFrame.ShowPlayerInfo(playerInfo);
        //TODO: If this turn is of the player => set the name to "YOU"
        characterImg.sprite = frameDataManager.GetAvatarItemByID(playerInfo.avatarID).sprite;
        turnBgImg.color = gameController.GetColor(teamColor, false);
        turnAnnounceTxt.text = teamColor switch
        {
            TeamColor.RED => "RED'S TURN",
            TeamColor.GREEN => "GREEN'S TURN",
            TeamColor.BLUE => "BLUE'S TURN",
            TeamColor.YELLOW => "YELLOW'S TURN",
            _ => ""
        };


        audioManager.PlaySound("Woosh");
        PlayTurnAnim();
    }

    #endregion

    #region Private Methods

    private void PlayTurnAnim()
    {
        if (animClip.isPlaying)        // Check if *any* animation is currently playing
        {
            animClip.Stop();           // Stop it
        }
        animClip.Rewind();
        animClip.Play();
    }

    #endregion

}

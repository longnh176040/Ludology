using UnityEngine;

[CreateAssetMenu(fileName = "PlayerFrameData", menuName = "Data/PlayerFramData")]
public class PlayerFrameDataSO : ScriptableObject
{
    public Sprite[] frameSprites;
    public Sprite[] backgroundSprites;

    public Sprite[] redAvatars;
    public Sprite[] blueAvatars; 
    public Sprite[] greenAvatars; 
    public Sprite[] yellowAvatars;
}
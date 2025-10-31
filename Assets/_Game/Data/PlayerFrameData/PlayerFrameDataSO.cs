using UnityEngine;

[CreateAssetMenu(fileName = "RepresentationData", menuName = "Data/RepresentationData")]
public class PlayerFrameDataSO : ScriptableObject
{
    public FrameItemSO[] frameItems;
    public BackgroundItemSO[] backgroundItems;

    public AvatarItemSO[] redAvatars;
    public AvatarItemSO[] blueAvatars; 
    public AvatarItemSO[] greenAvatars; 
    public AvatarItemSO[] yellowAvatars;
} 




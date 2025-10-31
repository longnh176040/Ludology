using UnityEngine;

[CreateAssetMenu(fileName = "FrameItem", menuName = "Data/Frame Item")]
public class FrameItemSO : ScriptableObject
{
    public string id;
    public Sprite sprite;
    public int price;
}


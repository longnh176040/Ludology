using UnityEngine;

[CreateAssetMenu(fileName = "AvatarItem", menuName = "Data/Avatar Item")]
public class AvatarItemSO : ScriptableObject
{
    public string id;
    public Sprite sprite;
    public string displayName;
    public int price;
}

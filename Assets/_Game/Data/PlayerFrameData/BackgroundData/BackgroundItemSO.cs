using UnityEngine;

[CreateAssetMenu(fileName = "BackgroundItem", menuName = "Data/Background Item")]
public class BackgroundItemSO : ScriptableObject
{
    public string id;
    public Sprite sprite;
    public int price;
}

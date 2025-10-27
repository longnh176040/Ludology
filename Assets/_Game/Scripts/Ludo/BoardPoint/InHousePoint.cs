using UnityEngine;

public class InHousePoint : BoardPoint
{
    [SerializeField] private bool isFinal;

    public bool IsFinal => isFinal;
    public int ID => id;

    private int id; //ID FROM 0-5


    public void Init(int id)
    {
        this.id = id;
        pieceInThis.Clear();
    }
}

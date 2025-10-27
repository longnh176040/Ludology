public class MovePoint : BoardPoint
{
    public bool IsBlocked;
    public bool IsStartPoint;
    protected int id;

    public int ID => id;

    public void Init(int id)
    {
        this.id = id;
        pieceInThis.Clear();
    }
}

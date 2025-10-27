using System;
using UnityEngine;

public class BoardManager : Singleton<BoardManager>
{
    [SerializeField] private MovePoint[] movePoints;
    [SerializeField] private InHouse[] inHousesList;

    private void Start()
    {
        for (int i = 0; i < movePoints.Length; i++)
        {
            movePoints[i].Init(i);
        }

        foreach (var inhouse in inHousesList)
        {
            inhouse.Init();
        }
    }

    public InHouse GetInHousePointsByColor(TeamColor color)
    {
        foreach (var house in inHousesList)
        {
            if (color == house.TeamColor) return house;
        }
        return null;
    }

    public InHousePoint[] GetInHousePoint(TeamColor color, int step = 6)
    {
        InHousePoint[] inHousePoints = GetInHousePointsByColor(color).HousePoints;

        if (step == 6) return inHousePoints;
        else
        {
            InHousePoint[] resInHousePoints = new InHousePoint[step];
            for (int i = 0; i < step; i++)
            {
                resInHousePoints[i] = inHousePoints[i];
            }
            return resInHousePoints;
        }
    }

    public InHousePoint[] GetInHousePoint(TeamColor color, int start, int step = 6)
    {
        if (start + step >= 6) return null;

        InHousePoint[] inHousePoints = GetInHousePointsByColor(color).HousePoints;

        InHousePoint[] resInHousePoints = new InHousePoint[step];

        for (int i = 0; i < resInHousePoints.Length; i++)
        {
            resInHousePoints[i] = inHousePoints[start + i + 1];
        }
        return resInHousePoints;

    }

    public MovePoint FindNextMovePoint(MovePoint curMovePoint)
    {
        for (var i = 0; i < movePoints.Length; i++)
        {
            if (movePoints[i] == curMovePoint)
            {
                if (i + 1 == movePoints.Length)
                {
                    return movePoints[0];
                }
                else return movePoints[i + 1];
            }
        }
        return null;
    }

    /*
        Find the path between two points in an anti-clockwise direction
        Used to kick an emeny piece back to its corner
     */
    public MovePoint[] FindPathBetweenMovePoints(int curPointId, int targetPointId)
    {
        int pathLength = curPointId < targetPointId ? (curPointId + movePoints.Length - targetPointId) : (curPointId - targetPointId);
        MovePoint[] path = new MovePoint[pathLength];

        int tmpPointId = curPointId;
        for (int i = 0; i < path.Length; i++)
        {
            int prevPointId = tmpPointId - 1;
            if (prevPointId < 0) prevPointId = movePoints.Length - 1;

            path[i] = GetMovePointByID(prevPointId);
            tmpPointId = prevPointId;
        }

        return path;
    }

    public BoardPoint[] CreatePath(TeamColor color, BoardPoint curPoint, int step)
    {
        if (curPoint is InHousePoint) //Only piece with the same color can go in house
        {
            InHousePoint cihp = (InHousePoint)curPoint;
            //Debug.Log("======== " + cihp.ID + " step " + step);
            return GetInHousePoint(color, cihp.ID, step);
        }
        else if (curPoint is ArrowPoint)
        {
            ArrowPoint ap = (ArrowPoint)curPoint;
            if (color == ap.TeamColor) //Piece finishes the round
            {
                return GetInHousePoint(color, step);
            }
            else //Keep moving
            {
                return FindPoint();
            }
        }
        else //curPoint is MovePoint
        {
            return FindPoint();
        }

        BoardPoint[] FindPoint()
        {
            MovePoint mp = (MovePoint)curPoint;
            BoardPoint[] path = new BoardPoint[step];

            for (int i = 0; i < step; i++)
            {
                MovePoint point = FindNextMovePoint(mp);
                if (point.IsBlocked) return null;

                if (point is ArrowPoint)
                {
                    ArrowPoint arp = (ArrowPoint)point;
                    if (color == arp.TeamColor) //Piece finishes the round
                    {
                        path[i] = point; i++;
                        int stepLeft = step - i;
                        InHousePoint[] inHousePoints = GetInHousePoint(color, stepLeft);
                        for (int j = i; j < step; j++)
                        {
                            path[j] = inHousePoints[j - i];
                        }
                        return path;
                    }
                    else
                    {
                        path[i] = point;
                        mp = point;
                    }
                }
                else
                {
                    path[i] = point;
                    mp = point;
                }
            }
            return path;
        }
    }

    private MovePoint GetMovePointByID(int id)
    {
        if (id >= movePoints.Length)
            return null;
        return movePoints[id];
    }
}

[Serializable]
public class InHouse
{
    [SerializeField] private TeamColor teamColor;
    [SerializeField] private InHousePoint[] housePoints;

    public TeamColor TeamColor => teamColor;
    public InHousePoint[] HousePoints => housePoints;

    public void Init()
    {
        for (int i = 0; i < housePoints.Length; i++)
        {
            housePoints[i].Init(i);
        }
    }
}

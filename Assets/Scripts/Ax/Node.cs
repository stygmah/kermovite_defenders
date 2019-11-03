using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{

    public Pointer GridPosition { get; set; }
    public TileScript TileRef { get; private set; }
    public Vector2 WorldPosition { get; private set; }
    public Node Parent { get; set; }
    public int G { get; set; }
    public int H { get; set; }
    public int F { get; set; }

    public Node(TileScript tileRef)
    {
        this.TileRef = tileRef;
        this.GridPosition = tileRef.GridPosition;
        this.WorldPosition = new Vector2(tileRef.GridPosition.X, tileRef.GridPosition.Y);
    }

    public void CalcValues(Node parent, Node goal, int gCost)
    {
        this.Parent = parent;
        this.G = parent.G + gCost;
        this.H = ((Math.Abs(GridPosition.X - goal.GridPosition.X)) + Math.Abs((goal.GridPosition.Y - GridPosition.Y))) * 10;
        this.F = G + H;
    }

}

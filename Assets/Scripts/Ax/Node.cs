using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{

    public Pointer GridPosition { get; set; }
    public TileScript TileRef { get; private set; }

    public Node(TileScript tileRef)
    {
        this.TileRef = tileRef;
        this.GridPosition = tileRef.GridPosition;
    }


}

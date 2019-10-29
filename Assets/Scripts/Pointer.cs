using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Pointer 
{
    public int X { get; set; }
    public int Y { get; set; }

    public Pointer(int x, int y)
    {
        this.X = x;
        this.Y = y;
    }
}

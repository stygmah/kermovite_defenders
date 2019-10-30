using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Ax
{
    private static Dictionary<Pointer, Node> nodes;

    private static void CreateNodes()
    {
        nodes = new Dictionary<Pointer, Node>();

        foreach (TileScript tile in LevelManager.Instance.Tiles.Values)
        {
            nodes.Add(tile.GridPosition, new Node(tile));
        }
    }

    public static void GetPath(Pointer start)
    {

        if(nodes == null)
        {
            CreateNodes();
        }

        HashSet<Node> openList = new HashSet<Node>();

        Node currentNode = nodes[start];

        openList.Add(currentNode);

        /****TO REMOVE LATER***/
        GameObject.Find("Debug").GetComponent<AxDebug>().DebugPath(openList);
    }

}

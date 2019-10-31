using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    public static Stack<Node> GetPath(Pointer spawn, Pointer goal)
    {

        if(nodes == null)
        {
            CreateNodes();
        }

        HashSet<Node> openList = new HashSet<Node>();
        HashSet<Node> closedList = new HashSet<Node>();
        Stack<Node> finalPath = new Stack<Node>();

        Node currentNode = nodes[spawn];

        //add start node
        openList.Add(currentNode);

        while (openList.Count > 0)
        {
            //Loops start through neighbours
            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {

                    Pointer neighbourPointer = new Pointer(currentNode.GridPosition.X - x, currentNode.GridPosition.Y - y);
                    //check bounds
                    if (LevelManager.Instance.Tiles.ContainsKey(neighbourPointer))
                    {
                        //check walkable and if it is not the same node
                        if (neighbourPointer != currentNode.GridPosition && LevelManager.Instance.Tiles[neighbourPointer].IsWalkable)
                        {
                            int g = 0;
                            if (Math.Abs(x - y) == 1)
                            {
                                g = 10;
                            }
                            else
                            {
                                if(!IsDiagonal(currentNode, nodes[neighbourPointer]))
                                {
                                    continue;
                                }
                                g = 14;
                            }



                            //Add neighbours
                            Node neighbour = nodes[neighbourPointer];



                            if (openList.Contains(neighbour))
                            {
                                if (currentNode.G + g < neighbour.G)
                                {
                                    neighbour.CalcValues(currentNode, nodes[goal], g);
                                }
                            }
                            else if (!closedList.Contains(neighbour))
                            {
                                openList.Add(neighbour);
                                neighbour.CalcValues(currentNode, nodes[goal], g);
                            }
                        }
                    }
                }
            }
            //move nodes
            openList.Remove(currentNode);
            closedList.Add(currentNode);

            //sort
            if (openList.Count > 0)
            {
                currentNode = openList.OrderBy(n => n.F).First();
            }
            //reach goal
            if (currentNode == nodes[goal])
            {
                while (currentNode.GridPosition != spawn)
                {
                    finalPath.Push(currentNode);
                    currentNode = currentNode.Parent;

                }
                break;
            }
        }
        return finalPath;
        /****TO REMOVE LATER***/
        //GameObject.Find("Debug").GetComponent<AxDebug>().DebugPath(openList, closedList);
    }
    private static bool IsDiagonal(Node currentNode, Node neighbour)
    {
        Pointer direction = neighbour.GridPosition - currentNode.GridPosition;
        Pointer first = new Pointer(currentNode.GridPosition.X + direction.X, currentNode.GridPosition.Y);
        Pointer second = new Pointer(currentNode.GridPosition.X, currentNode.GridPosition.Y + direction.Y);

        if (!LevelManager.Instance.Tiles[first].IsWalkable)
        {
            return false; 
        }
        if (!LevelManager.Instance.Tiles[second].IsWalkable)
        {
            return false;
        }

        return true;

    }

}

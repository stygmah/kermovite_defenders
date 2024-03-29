﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    //properties
    public Dictionary<Pointer, TileScript> Tiles { get; set; }

    [SerializeField]
    public GameObject spawnPoint;
    [SerializeField]
    public GameObject goalPoint;

    public Spawn Spawn { get; set; }
    public Spawn Goal { get; set; }

    private Stack<Node> endPath;
    public Stack<Node> EndPath
    {
        get
        {
            if(endPath == null)
            {
                GeneratePath();
            }
            return new Stack<Node>(new Stack<Node>(endPath));
        }
    }


    private void Awake()
    {
        Tiles = new Dictionary<Pointer, TileScript>();
    }
    // Start is called before the first frame update
    void Start()
    {
        Spawn = spawnPoint.GetComponent<Spawn>();
        Goal = goalPoint.GetComponent<Spawn>();
    }


    public void GeneratePath()
    {
        endPath = Ax.GetPath(Spawn.location ,Goal.location);
    }

    public Stack<Node> GenerateReversePath(Pointer start)
    {
        return Ax.GetPath(start, Spawn.location);
    }

}

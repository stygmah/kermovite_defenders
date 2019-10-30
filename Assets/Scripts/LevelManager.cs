using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : Singleton<LevelManager>
{
    //properties
    public Dictionary<Pointer, TileScript> Tiles { get; set; }



    private void Awake()
    {
        Tiles = new Dictionary<Pointer, TileScript>();
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

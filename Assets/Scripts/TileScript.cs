using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileScript : MonoBehaviour
{

    public Pointer GridPosition { get; set; }
    public int nom = 1;

    [SerializeField]
    public LevelManager Level;

    // Start is called before the first frame update
    void Start()
    {
        GridPosition = new Pointer((int)this.transform.position.x, (int)this.transform.position.y);
        GameObject.Find("LevelManager").gameObject.GetComponent<LevelManager>().Tiles.Add(GridPosition, this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}

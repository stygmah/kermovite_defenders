using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TileScript : MonoBehaviour
{

    //todelete
    public bool Debugging { get; set; }

    public Pointer GridPosition { get; set; }
    public bool IsBuildable { get; set; }
    public bool IsWalkable { get; set; }


    private Color32 forbiddenColor = new Color32(255, 50, 50, 255);
    public SpriteRenderer SpriteRenderer { get; set; }


    [SerializeField]
    public LevelManager Level;

    // Start is called before the first frame update
    void Start()
    {
        Setup();
        SpriteRenderer = GetComponent<SpriteRenderer>();
        GridPosition = new Pointer((int)this.transform.position.x, (int)this.transform.position.y);
        LevelManager.Instance.Tiles.Add(GridPosition, this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Setup
    private void Setup()
    {
        IsBuildable = true;
        IsWalkable = true;
    }



    //Mouse Events
    private void OnMouseOver()
    {
        if (Input.GetMouseButton(0) && GameManager.Instance.ClickedBtn != null)
        {
            if (!IsBuildable)
            {
                SpriteRenderer.color = forbiddenColor;
            }
            else if (!EventSystem.current.IsPointerOverGameObject() && GameManager.Instance.ClickedBtn != null)
            {
                PlaceTower();
            }
        }
    }
    private void OnMouseExit()
    {
        //DELETE IF STATEMENT
        if (!Debugging)
        {
            SpriteRenderer.color = Color.white;
        }
    }



    //Private functions
    private void PlaceTower()
    {
            Instantiate(GameManager.Instance.ClickedBtn.TowerPrefab, transform.position, Quaternion.identity);
            GameManager.Instance.BuyTower(GameManager.Instance.ClickedBtn);
            IsBuildable = false;
              //*************DELETE
            IsWalkable = false;
    }


}

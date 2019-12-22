using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TileScript : MonoBehaviour
{
    //setup props
    [SerializeField]
    private bool walkable;
    [SerializeField]
    private bool buildable;


    public Pointer GridPosition { get; set; }
    public bool IsBuildable { get; set; }
    public bool IsWalkable { get; set; }

    private Tower tower;
    private Range range;
    private Color32 forbiddenColor = new Color32(255, 50, 50, 255);
    private Color32 rangeColor = new Color32(0,255,22,29);
    private Color32 transparent = new Color32(0, 0, 0, 0);
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
        IsBuildable = buildable;
        IsWalkable = walkable;

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
        }else if (!EventSystem.current.IsPointerOverGameObject() && GameManager.Instance.ClickedBtn == null && Input.GetMouseButtonDown(0))
        {
            if (tower != null)
            {
                GameManager.Instance.SelectTower(tower);
            }
            else
            {
                GameManager.Instance.DeselectTower();
            }
        }
    }

    private void OnMouseExit()
    {
        SpriteRenderer.color = Color.white;
    }

    //Private functions
    private void PlaceTower()
    {
            tower = Instantiate(GameManager.Instance.ClickedBtn.TowerPrefab, transform.position, Quaternion.identity).GetComponent<Tower>();
            tower.totalSpent = GameManager.Instance.ClickedBtn.Price;
            range = tower.transform.GetChild(0).GetComponent<Range>();
            GameManager.Instance.BuyTower(GameManager.Instance.ClickedBtn);
            IsBuildable = false;
    }

    public void RemoveTower()
    {
        IsBuildable = true;
        Destroy(tower.gameObject);
    }
}

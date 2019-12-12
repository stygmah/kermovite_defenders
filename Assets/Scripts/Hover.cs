using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hover : Singleton<Hover>
{
    [SerializeField]
    private GameObject rangeSpriteRenderer;

    private SpriteRenderer spriteRenderer;
    private GameObject range;

    // Start is called before the first frame update

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        range = Instantiate(rangeSpriteRenderer, Camera.main.ScreenToWorldPoint(Input.mousePosition), Quaternion.identity);
        Destroy(range.GetComponent<Range>());
        range.transform.SetParent(transform);
        range.transform.position = transform.position;
        range.GetComponent<SpriteRenderer>().sortingOrder = 2;
        range.active = false;
    }

    // Update is called once per frame
    void Update()
    {
        FollowMouse();
        range.transform.position = transform.position;
    }

    private void FollowMouse()
    {
        if (spriteRenderer.enabled)
        {
            transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        }
    }

    public void Activate(Sprite sprite, GameObject towerPrefab)
    {
        SetRange(towerPrefab);
        spriteRenderer.sprite = sprite;
        spriteRenderer.enabled = true;
        range.active = true;
    }
    public void Deactivate()
    {
        spriteRenderer.enabled = false;
        GameManager.Instance.ClickedBtn = null;
        range.active = false;
    }
    public void SetRange(GameObject tower)
    {
        float rangeRadius = tower.GetComponent<Tower>().GetRangeRadius();
        range.transform.localScale = new Vector3(rangeRadius, rangeRadius, 1f);
    }
}

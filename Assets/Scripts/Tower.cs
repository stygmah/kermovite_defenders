using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{

    private Range range;
    private SpriteRenderer rangeRenderer;


    // Start is called before the first frame update
    void Start()
    {
        rangeRenderer = transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>();
        rangeRenderer.enabled = false;
        range = transform.GetChild(0).gameObject.GetComponent<Range>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Select()
    {
        rangeRenderer.enabled = !rangeRenderer.enabled;
    }

    public void Attack()
    {
        if (range.target != null)
        {
            Projectile projectile = new Projectile();
        }
    }
}

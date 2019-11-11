using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{

    public Range range;
    private SpriteRenderer rangeRenderer;
    [SerializeField]
    private GameObject projectile;
    [SerializeField]
    private float speed;
    [SerializeField]
    private float cooldown;
    [SerializeField]
    private int damage;


    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(transform.GetChild(0));
        rangeRenderer = transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>();
        rangeRenderer.enabled = false;
        range = transform.GetChild(0).gameObject.GetComponent<Range>();
        InvokeRepeating("Attack", 0f, cooldown);

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
            Projectile projectileShot = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<Projectile>();
            projectileShot.victim = range.target;
            projectileShot.speed = speed;
            projectileShot.damage = damage;
            projectileShot.tower = this;
        }
      
    }

}

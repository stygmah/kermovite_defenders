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
    [SerializeField]
    public bool freeze;
    [SerializeField]
    private int freezeFactor;
    [SerializeField]
    private float freezeTime;

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
            SearchToFreeze();
            Creep toKill = freeze ? SearchToFreeze() : range.target;
            if (toKill != null)
            {
                CreateProjectile(toKill);
            }
        }
      
    }
    public void CreateProjectile(Creep target)
    {
        Projectile projectileShot = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<Projectile>();
        projectileShot.victim = target;
        projectileShot.speed = speed;
        projectileShot.damage = damage;
        projectileShot.freeze = freeze;
        projectileShot.freezeFactor = freezeFactor;
        projectileShot.freezeTime = freezeTime;
        projectileShot.tower = this;
    }
    private Creep SearchToFreeze()
    {
        if (!range.target.frozen)
        {
            return range.target;
        }
        foreach (Creep creep in range.killList)
        {
            if (!creep.frozen)
            {
                return creep;
            }
        }
        return null;
    }

}

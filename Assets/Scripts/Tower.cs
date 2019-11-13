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

    [SerializeField]
    private bool laser;
    private SpriteRenderer laserSprite;

    // Start is called before the first frame update
    void Start()
    {
        rangeRenderer = transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>();
        rangeRenderer.enabled = false;
        range = transform.GetChild(0).gameObject.GetComponent<Range>();
        InvokeRepeating("Attack", 0f, cooldown);
        laserSprite = transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>();
        laserSprite.enabled = false;
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
        projectileShot.laser = laser;
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
    public void LaserShot(Vector3 enemyPosition)
    {
        laserSprite.transform.localScale = new Vector3(laserSprite.transform.localScale.x, Vector2.Distance(transform.position, enemyPosition), 1f);
        StartCoroutine(LaserEffect(rotationTowards(transform.position, enemyPosition), 0.2f));
    }

    //asign to z
    public float rotationTowards(Vector3 origin, Vector3 target)
    {
        Vector3 dir = target - origin;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        return angle+265f;
    }

    private IEnumerator LaserEffect(float angle, float duration)
    {
        laserSprite.enabled = true;
        laserSprite.transform.rotation = Quaternion.Euler(0, 0, angle);
        yield return new WaitForSeconds(duration);
        laserSprite.enabled = false;
    }

}

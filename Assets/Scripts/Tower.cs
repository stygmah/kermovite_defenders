using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{

    public Range range;
    public Sprite sprite;
    private SpriteRenderer rangeRenderer;
    public int level;

    [SerializeField]
    public string towerName;
    [SerializeField]
    private int[] upgradeCosts = new int[4];

    [SerializeField]
    private GameObject projectile;
    [SerializeField]
    private float speed;
    [SerializeField]
    private float cooldown;
    [SerializeField]
    private int damage;
    [SerializeField]
    private float rangeRadius;

    [SerializeField]
    public bool freeze;
    [SerializeField]
    private int freezeFactor;
    [SerializeField]
    private float freezeTime;

    [SerializeField]
    private bool laser;
    private SpriteRenderer laserSprite;

    [SerializeField]
    public bool splash;

    private int upgradePrice;
    public int totalSpent;

    // Start is called before the first frame update
    void Start()
    {
        rangeRenderer = transform.GetChild(0).gameObject.GetComponent<SpriteRenderer>();
        rangeRenderer.enabled = false;
        range = transform.GetChild(0).gameObject.GetComponent<Range>();
        InvokeRepeating("Attack", 0f, cooldown);
        if (laser)
        {
            laserSprite = transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>();
            laserSprite.enabled = false;
        }
        sprite = GetComponent<SpriteRenderer>().sprite;
        level = 1;
        UpdateUpgradePrice();
        UpdateRange();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Select()
    {
        rangeRenderer.enabled = !rangeRenderer.enabled;
        GameManager.Instance.ShowInfoTower(this);
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
        StartCoroutine(LaserEffect(rotationTowards(transform.position, enemyPosition), 0.1f));
    }

    public void UpdateRange()
    {
        range.transform.localScale = new Vector3(rangeRadius, rangeRadius, 1f);
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
        yield return StartCoroutine(FadeOut(laserSprite));
        laserSprite.enabled = false;
    }
    private IEnumerator FadeOut(SpriteRenderer sr)
    {
        for (float i = 1; i >= 0; i -= Time.deltaTime*3)
        {
            // set color with i as alpha
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, i);
            yield return null;
        }
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 1);
    }
    //Get text methods
    public string GetAttackAndSpeed()
    {
        return "Att: "+damage+" / Sp: "+(15-(cooldown*10));
    }
    public string GetSpecialText()
    {
        string result = "";
        Projectile pr = projectile.GetComponent<Projectile>();
        if (freeze)
        {
            result = "Freeze time: " + freezeTime + "s";
        }
        else if(pr.splash)
        {
            result = "Splash: " + pr.splashRange * 10;
        }else if (laser)
        {
            result = "Perforates armor";
        }
        return result;
    }
    //Tower menu actions
    public void LevelUp()
    {
        level++;
        UpdateUpgradePrice();
        UpgradeAttributes();
        UpdateRange();
    }
    public int GetUpgradePrice()
    {
        return upgradePrice;
    }
    public void UpdateUpgradePrice()
    {
        totalSpent += upgradePrice;
        upgradePrice = upgradeCosts[level - 1];
    }
    public bool CheckUpgradeable()
    {
        return level < upgradeCosts.Length;
    }
    public void UpgradeAttributes()
    {
        damage += (int)((float)damage * ((float)level / 5));
        rangeRadius += rangeRadius * ((float)level / 20);
        if (freeze)
        {
            freezeTime += 0.30f+((float)level/15);
        }
        else if (splash)
        {
            Projectile pr = projectile.GetComponent<Projectile>();
            pr.splashRange += pr.splashRange * 0.1f;
        }
        UpdateRange();
    }
    public int GetSalePrice()
    {
        return (int)((float)totalSpent * 0.9f);
    }

    //
}

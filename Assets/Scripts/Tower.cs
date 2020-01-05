using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    //Main func
    public Range range;
    public Sprite sprite;
    private SpriteRenderer rangeRenderer;
    private GameObject barrel;
    public int level;

    //options
    [SerializeField]
    private Sprite fullSprite;
    [SerializeField]
    private AudioClip shootSound;
    [Range(0, 1)]
    public float volume;

    //Stats
    [SerializeField]
    public string towerName;
    [SerializeField]
    private int[] upgradeCosts = new int[4];
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
    [SerializeField]
    public bool splash;
    [SerializeField]
    private float splashRange;

    //helpers
    [SerializeField]
    private GameObject projectile;
    private bool shootingLaser;
    private SpriteRenderer laserSprite;
    public int totalSpent;
    private int upgradePrice;




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
        SetBarrel();
        shootingLaser = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!shootingLaser)TurnBarrelTowards();
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
        GetComponent<AudioSource>().PlayOneShot(shootSound, volume);
        Projectile projectileShot = Instantiate(projectile, transform.position, Quaternion.identity).GetComponent<Projectile>();
        projectileShot.victim = target;
        projectileShot.speed = speed;
        projectileShot.damage = damage;
        projectileShot.freeze = freeze;
        projectileShot.freezeFactor = freezeFactor;
        projectileShot.freezeTime = freezeTime;
        projectileShot.tower = this;
        projectileShot.laser = laser;
        if (splash) projectileShot.splashRange = splashRange;//TODO subsitute in projectile
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
        shootingLaser = true;
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
        shootingLaser = false;
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 1);
    }
    //Get text methods
    public float GetRangeRadius()
    {
        return rangeRadius;
    }
    public string GetAttackAndSpeed()
    {
        return "Att: "+damage+" / Sp: "+(15-(cooldown*10));
    }
    public string GetSpecialText()
    {
        string result = "";
        if (freeze)
        {
            result = "Freeze time: " + (int)(freezeTime*1000) + "ms";
        }
        else if(splash)
        {
            result = "Splash: " + (int) (splashRange * 10);
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
        if (CheckUpgradeable())
        {
            return upgradePrice;
        }
        return 0;
    }
    public void UpdateUpgradePrice()
    {
        totalSpent += upgradePrice;
        if (CheckUpgradeable())
        {
            upgradePrice = upgradeCosts[level - 1];
        }

    }
    public bool CheckUpgradeable()
    {
        return level < upgradeCosts.Length+1;
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
            splashRange += splashRange * 0.11f;
        }
        UpdateRange();
    }
    public int GetSalePrice()
    {
        return (int)((float)totalSpent * 0.9f);
    }

    //rotation
    private void SetBarrel()
    {

        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).tag == "barrel")
            {
                barrel = transform.GetChild(i).gameObject;
            }
        }

    }
    private void TurnBarrelTowards()
    {
        Creep target = range.GetTarget();
        if (target != null && barrel != null)
        {
            barrel.transform.rotation = Quaternion.Euler(0, 0, rotationTowards(transform.position, target.transform.position));
        }
    }

    public Sprite GetFullSprite()
    {
        return fullSprite;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creep : MonoBehaviour
{
    [SerializeField]
    private float speed;
    private Stack<Node> path;
    public Pointer GridPosition { get; set; }
    private Vector3 destination;
    [SerializeField]
    private int health;
    [SerializeField]
    public int money;
    private float startHealth;
    private GameObject healthBar;
    public bool frozen;
    private float initialSpeed;
    public bool resistant;

    private bool dying;


    // Start is called before the first frame update
    void Start()
    {
        startHealth = (float)health;
        healthBar = this.transform.GetChild(0).gameObject;
        initialSpeed = speed;
        dying = false;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    public void Spawn()
    {
        transform.position = LevelManager.Instance.Spawn.transform.position;
        SetPath(LevelManager.Instance.EndPath);
    }

    private void Move()
    {
        transform.position = Vector2.MoveTowards(transform.position, destination, speed*Time.deltaTime);

        if (transform.position == destination)
        {
            if(path !=null && path.Count > 0)
            {
                GridPosition = path.Peek().GridPosition;
                destination = path.Pop().WorldPosition;
            }
        }
    }
    private void SetPath(Stack<Node> newPath)
    {
        if(newPath != null)
        {
            this.path = newPath ;

            GridPosition = path.Peek().GridPosition;
            destination = path.Pop().WorldPosition;
        }
    }
    //collisions
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag== "goal")
        {
            ReachGoal();
        }else if(collision.tag == "projectile")
        {
            Hit(collision.gameObject);
        }
    }
    private void ReachGoal()
    {
        GameManager.Instance.DeadOrGoalCreature();
        Destroy(gameObject);
        if (GameManager.Instance.Health > 0)
        {
            GameManager.Instance.LoseLife();
        }
        if (GameManager.Instance.Health == 0)
        {
            GameManager.Instance.GameOver();
        }
    }
    private void Hit(GameObject projectile)
    {
        Projectile pScript = projectile.GetComponent<Projectile>();

        if (GetInstanceID() == pScript.victim.GetInstanceID())
        {
            hitType(projectile, pScript);
        }
    }
    public void TakeDamage(GameObject projectile, bool splash)
    {
        Projectile projectileScript = projectile.GetComponent<Projectile>();

        if (projectileScript.laser)
        {
            projectileScript.tower.LaserShot(transform.position);
        }

        InflictDamage(projectileScript);

        if (health <= 0)
        {
            projectileScript.tower.range.CritterDead();
            Die();
        }
        else
        {
            healthBar.transform.localScale = new Vector3((float)health/startHealth, healthBar.transform.localScale.y, healthBar.transform.localScale.z);
        }

        if (!splash)
        {
            Destroy(projectile);
        }
    }
    private IEnumerator Freeze(int freezeFactor, float time)//factor is 1-100
    {
        frozen = true;
        float toFreeze = 1f - (float)freezeFactor/100;
        speed = speed * toFreeze;
        yield return new WaitForSeconds(time);
        frozen = false;
        speed = initialSpeed;
    }
    public void FreezeCreature(int freezeFactor, float time)
    {
        if (!frozen)
        {
            StartCoroutine(Freeze(freezeFactor, time));
        }
    }
    private void hitType(GameObject projectile, Projectile pScript)
    {
        if (pScript.splash)
        {
            pScript.SplashDamage();
        }
        else if (pScript.freeze)
        {
            pScript.freezeDamage(this.gameObject);
        }
        else
        {
            TakeDamage(projectile, false);
        }
    }

    private void Die()
    {

        if (!dying)
        {
            dying = true;
            GameManager.Instance.ChangeMoney(money, true);
            GameManager.Instance.DeadOrGoalCreature();
            Destroy(gameObject);
        }
    }

    private void InflictDamage(Projectile projectile)
    {
        if (projectile.laser && resistant)
        {
            health = health - projectile.damage*2;
        }
        else if(resistant)
        {
            health = health - projectile.damage / 2;
        }
        else
        {
            health = health - projectile.damage;
        }
    }

    public void SetHealth(float multiplier, int wave)
    {
        health += ((int)(health * multiplier) + wave);
    }
}

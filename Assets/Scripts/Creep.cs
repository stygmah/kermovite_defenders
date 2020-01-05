using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creep : MonoBehaviour
{
    //STATS
    [SerializeField]
    public string nameCreep;
    [SerializeField]
    private float speed;
    [SerializeField]
    private int health;
    [SerializeField]
    public int money;
    [SerializeField]
    public float spawnRate;

    //Pathfinding
    private Stack<Node> path;
    public Pointer GridPosition { get; set; }
    private Vector3 destination;

    //status
    public bool resistant;
    public bool frozen;

    //kermovite
    private GameObject kermovite;


    //helpers
    private float startHealth;
    private GameObject healthBar;
    private float initialSpeed;
    private bool dying;
    private bool picking;
    private bool reachedGoal;


    // Start is called before the first frame update
    void Start()
    {
        startHealth = (float)health;
        healthBar = this.transform.GetChild(0).gameObject;
        initialSpeed = speed;
        dying = false;
        reachedGoal = false;
        picking = false;
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
            if (path !=null && path.Count > 0)
            {
                GridPosition = path.Peek().GridPosition;
                destination = path.Pop().WorldPosition;
                Turn();
            }
        }
    }
    private void Turn()
    {
        float x = transform.position.x - destination.x;
        float y = transform.position.y - destination.y;
        if(y == 0)
        {
            if (x < 0)
            {
                Rotate(270);
            }
            else
            {
                Rotate(90);
            }
        }else
        {
            if (y < 0)
            {
                Rotate(0);
            }
            else
            {
                Rotate(180);
            }
        }

    }
    private void Rotate(int angle)
    {
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0,0,angle),360);
        transform.GetChild(0).transform.position = new Vector3(transform.position.x, transform.position.y + 0.6f, 1);
        transform.GetChild(0).transform.rotation = Quaternion.identity;
    }


    private void SetPath(Stack<Node> newPath)
    {
        if(newPath != null)
        {
            this.path = newPath ;

            GridPosition = path.Peek().GridPosition;
            destination = path.Pop().WorldPosition;
            Turn();
        }
    }
    //collisions
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag== "goal")
        {
            Reactor reactor = collision.gameObject.GetComponent<Reactor>();
            kermovite = reactor.LooseCrystal();
            reachedGoal = true;
            if(kermovite != null)Glow();
            ReturnToSpawn();
        }
        else if(collision.tag == "projectile")
        {
            Hit(collision.gameObject);
        }
        else if(collision.tag == "kermovite" && kermovite == null)
        {
            if (picking)
            {
                return;
            }
            picking = true;
            kermovite = GameManager.Instance.GetAuxKermovite();
            Destroy(collision.gameObject);
            if (!reachedGoal)
            {
                ReturnToSpawn();
                reachedGoal = true;
            }
            Glow();
        }
        else if (collision.tag == "spawn" && reachedGoal)
        {
            ReachGoal();
        }
    }
    private void ReachGoal()
    {
        GameManager.Instance.DeadOrGoalCreature(false);
        if (kermovite != null) LooseLife();
        Destroy(gameObject);
    }
    private void LooseLife()
    {
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
            GameManager.Instance.DeadOrGoalCreature(true);
            if(kermovite)Instantiate(kermovite, new Vector3(transform.position.x,transform.position.y, -1f), Quaternion.identity);
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

    public int GetHealth()
    {
        return health;
    }
    public void ReturnToSpawn()
    {
        path = LevelManager.Instance.GenerateReversePath(new Pointer((int)transform.position.x, (int)transform.position.y));
        destination = path.Pop().WorldPosition;
    }
    private void Glow()
    {
        Color glow = new Color(0f, 255f, 0f);
        glow.a = 0.9f; 
        if (GetComponent<SpriteRenderer>() != null) GetComponent<SpriteRenderer>().color = glow;
    }
}

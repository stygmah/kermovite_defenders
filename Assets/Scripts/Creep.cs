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



    // Start is called before the first frame update
    void Start()
    {
        
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
        if (pScript.splash)
        {
            pScript.SplashDamage();
        }
        else
        {
            TakeDamage(projectile, false);
        }

    }
    private void TakeDamage(GameObject projectile, bool splash)
    {
        Projectile projectileScript = projectile.GetComponent<Projectile>();
        health = health - projectileScript.damage;
        if (health <= 0)
        {
            projectileScript.tower.range.CritterDead();
            Destroy(gameObject);
        }
        if (!splash)
        {
            Destroy(projectile);
        }
    }
}

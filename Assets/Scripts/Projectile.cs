using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Creep victim;
    public int damage;
    public float speed;
    public Tower tower;
    public bool freeze;
    public bool splash;
    public float splashRange;
    public int freezeFactor;
    public float freezeTime;
    public bool laser;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    public void Move()
    {
        if(victim != null)
        {
            transform.position = Vector2.MoveTowards(transform.position, victim.transform.position, speed * Time.deltaTime);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    public void SplashDamage()
    {
        Debug.Log("splashing");
        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, splashRange);
        foreach (Collider2D col in enemies)
        {
            if (col.gameObject.tag == "critter")
            {
                Creep creep = col.gameObject.GetComponent<Creep>();
                creep.TakeDamage(this.gameObject,true);
            }
        }
        Destroy(this.gameObject);
    }
    public void freezeDamage(GameObject creep)
    {
        creep.GetComponent<Creep>().FreezeCreature(freezeFactor,freezeTime);
        Destroy(this.gameObject);
    }
}

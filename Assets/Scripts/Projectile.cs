using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Creep victim;
    public int damage;
    public float speed;
    public Tower tower;

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
}

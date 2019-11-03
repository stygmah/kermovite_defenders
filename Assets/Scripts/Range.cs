using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Range : MonoBehaviour
{
    public Creep target;
    public Queue<Creep> killList;

    // Start is called before the first frame update
    void Start()
    {
        killList = new Queue<Creep>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "critter")
        {
            Debug.Log(collision.GetInstanceID());
            if (target != null)
            {
                killList.Enqueue(collision.GetComponent<Creep>());
            }
            else
            {
                target = collision.GetComponent<Creep>();
            }
        }
    }
    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "critter")
        {
            target = killList.Count > 0 ? killList.Dequeue() : null;
        }
    }
    public void CritterDead()
    {
        target = killList.Count > 0 ? killList.Dequeue() : null;
    }

}

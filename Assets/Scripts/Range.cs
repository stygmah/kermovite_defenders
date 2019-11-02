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
        Debug.Log(collision);
        if(collision.tag == "creep")
        {
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
        if (collision.tag == "creep")
        {
            if (killList.Count > 0)
            {
                target = killList.Dequeue();
            }
            else
            {
                target = null;
            }
        }
    }

}

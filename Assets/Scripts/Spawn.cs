using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    public Pointer location;
    // Start is called before the first frame update
    void Start()
    {
        location = new Pointer((int)transform.position.x, (int)transform.position.y);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

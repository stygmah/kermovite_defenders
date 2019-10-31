using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField]
    private GameObject[] objectPrefabs;

    public GameObject GetObject(string type)
    {
        foreach (var obj in objectPrefabs)
        {
            if(obj.name == type)
            {
                GameObject newObject = Instantiate(obj);
                newObject.name = type;
                Debug.Log(newObject);
                return newObject;
            }
        }
        return null;
    }
}

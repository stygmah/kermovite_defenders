using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Wave")]
public class Wave : ScriptableObject
{
    public int reward;
    public float multiplier;
    public GameObject enemy;
    public bool group;
    public bool isBoss;

}

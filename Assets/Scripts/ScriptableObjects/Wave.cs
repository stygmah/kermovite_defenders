using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Wave : ScriptableObject
{
    public Creep creature;
    public int hpIncrement;
    public int number = 30;
    public bool isBoss = false;
    public int moneyPerKill;
}

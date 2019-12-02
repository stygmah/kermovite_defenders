using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : Singleton<WaveManager>
{
    [SerializeField]
    private Wave[] waves;
    private Wave currentWave;
    private int nextWaveN;
    private float multiplier;

    // Start is called before the first frame update
    void Start()
    {
        nextWaveN = 0;
        multiplier = 0f;

        NextWave();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Wave GetCurrentWave()
    {
        return this.currentWave;
    }
    public float GetMultiplier()
    {
        return this.multiplier;
    }

    public void NextWave()
    {
        if (waves.Length > nextWaveN)
        {
            SetNextHealth();
            currentWave = waves[nextWaveN];
            multiplier += currentWave.multiplier;
            nextWaveN++;
        } 
    }

    public int GetWaveN()
    {
        return nextWaveN;
    }

    public Wave GetNextWaveInfo()
    {
        if (waves.Length > nextWaveN)
        {
            return waves[nextWaveN + 1];
        }else
        {
            return null;
        }

    }

    private void SetNextHealth()
    {
        if (waves.Length > nextWaveN)
        {
            Creep nextCreep = waves[nextWaveN].enemy.GetComponent<Creep>();
            nextCreep.SetHealth(multiplier, nextWaveN);
        }

    }

}

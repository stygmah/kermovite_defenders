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

    private void Awake()
    {
        nextWaveN = 0;
        multiplier = 0f;

        NextWave();
    }
    // Start is called before the first frame update
    void Start()
    {

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
        Debug.Log("nextWaven:" + nextWaveN + " length:" + waves.Length);
        if (waves.Length > nextWaveN)
        {
            SetNextHealth();
            currentWave = waves[nextWaveN];
            multiplier += currentWave.multiplier;
            nextWaveN++;
            GameManager.Instance.SetInfoNextWave();
        }
        else
        {
            if (GameManager.Instance.Health > 0)
            {
                GameManager.Instance.Victory();
            }
            
        }
    }

    public int GetWaveN()
    {
        return nextWaveN;
    }

    public bool IsLast()
    {
        return waves.Length <= nextWaveN;
    }

    private void SetNextHealth()
    {
        if (waves.Length > nextWaveN)
        {
            Creep nextCreep = waves[nextWaveN].enemy.GetComponent<Creep>();
            nextCreep.SetHealth(multiplier, nextWaveN);
        }

    }
    public bool IsBoss()
    {
        return currentWave.isBoss;
    }

}

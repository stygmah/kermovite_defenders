using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField]
    private Wave[] waves;
    private Wave currentWave;
    private int waveN;
    private float multiplier;

    // Start is called before the first frame update
    void Start()
    {
        waveN = 0;
        multiplier = 0f;

        this.currentWave = waves[0];//DELETE
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Wave GetCurrentWave()
    {
        return this.currentWave;
    }

    public void nextWave()
    {
        if (waves.Length > waveN)
        {
            currentWave = waves[waveN];
            multiplier += currentWave.multiplier;
            waveN++;
        } 
    }


}

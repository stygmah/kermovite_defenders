using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectLevelManager : Singleton<SelectLevelManager>
{
    public bool skipTutorial;
    // Start is called before the first frame update
    void Start()
    {
        skipTutorial = false;
    }

    public void ToggleTutorial()
    {
        skipTutorial = !skipTutorial;
    }
}

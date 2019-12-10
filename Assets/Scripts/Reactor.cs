using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reactor : MonoBehaviour
{

    [SerializeField]
    private Sprite[] phases;
    [SerializeField]
    private GameObject kermovite;

    private SpriteRenderer spriteRenderer;
    private int remainingCrystals;
    private int total;

    // Start is called before the first frame update
    void Start()
    {
        total = phases.Length;
        remainingCrystals = total;
        spriteRenderer = GetComponent<SpriteRenderer>();
        UpdateSprite();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void UpdateSprite()
    {
        spriteRenderer.sprite = phases[phases.Length - remainingCrystals];
    }

    public GameObject LooseCrystal()
    {
        if (remainingCrystals > 1)
        {
            Debug.Log(total+" "+ remainingCrystals);
            remainingCrystals--;
            UpdateSprite();
            return kermovite;
        }
        else
        {
            return null;
        }
    }
    public void RecoverCrystal()
    {
        if (total > remainingCrystals)
        {
            remainingCrystals++;
            UpdateSprite();
        }
    }
    public int RemainingCrystals()
    {
        return remainingCrystals;
    }

    public void RestoreReactor()
    {
        int toRestore = GameManager.Instance.Health - (remainingCrystals - 1);
        for(int i = 0; i < toRestore; i++)
        {
            RecoverCrystal();
        }
        GameManager.Instance.SpendOnCollect();
        GameObject[] ks = GameObject.FindGameObjectsWithTag("kermovite");
        foreach (GameObject k in ks)
        {
            Destroy(k);
        }
    }
}

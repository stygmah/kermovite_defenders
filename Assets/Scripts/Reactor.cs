using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reactor : MonoBehaviour
{

    [SerializeField]
    private Sprite[] phases;

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

    public void LooseCrystal()
    {
        if (remainingCrystals >= 0)
        {
            remainingCrystals--;
            UpdateSprite();
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
}

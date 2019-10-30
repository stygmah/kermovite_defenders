using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerBtn : MonoBehaviour
{

    [SerializeField]
    private GameObject towerPrefab;
    [SerializeField]
    private Sprite sprite;
    [SerializeField]
    private int price;
    [SerializeField]
    private Text priceText;

    public GameObject TowerPrefab {get {return towerPrefab;}}
    public Sprite Sprite { get => sprite;}
    public int Price { get => price; set => price = value; }

    // Start is called before the first frame update
    void Start()
    {
        priceText.text = "$" + Price;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

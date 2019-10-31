using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{

    [SerializeField]
    private Text moneyText;
    public TowerBtn ClickedBtn { get; set; }
    public int Money;
    public ObjectPool Pool { get; set; }
    /// <summary>
    /// Changle later for wave
    /// </summary>
    [SerializeField]
    private GameObject creep;
    private Creep nextCreep;


    private void Awake()
    {
        Pool = GetComponent<ObjectPool>();
        nextCreep = creep.GetComponent<Creep>();
    }

    // Start is called before the first frame update
    void Start()
    {
        ChangeMoneyText(Money);
    }

    // Update is called once per frame
    void Update()
    {
        CancelOption();
    }

    public void PickTower(TowerBtn towerBtn)
    {
        if (Money >= towerBtn.Price)
        {
            this.ClickedBtn = towerBtn;
            Hover.Instance.Activate(towerBtn.Sprite);
        }

    }

    public void BuyTower(TowerBtn towerBtn)
    {
        Hover.Instance.Deactivate();
        Money = Money - towerBtn.Price;
        ChangeMoneyText(Money);
    }

    private void CancelOption()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Hover.Instance.Deactivate();
        }
    }
    private void ChangeMoneyText(int quantity)
    {
        moneyText.text = "$" + quantity;
    }

    public void NextWave()
    {
        StartCoroutine(SpawnWave());
    }

    private IEnumerator SpawnWave()
    {
        LevelManager.Instance.GeneratePath();
        Creep thisCreep = Instantiate(nextCreep);
        thisCreep.Spawn();

        yield return new WaitForSeconds(2.5f);
    }
}

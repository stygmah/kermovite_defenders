using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{

    [SerializeField]
    private Text moneyText;
    [SerializeField]
    private Text healthText;
    public int Money;
    public int Health;


    private bool gameOver = false;
    public bool paused = false;

    public TowerBtn ClickedBtn { get; set; }

    private Creep nextCreep;
    [SerializeField] /*TODO:DELETE SF*/private int nCreeps;
    private bool betweenWaves;

    [SerializeField]
    private GameObject gameOverMenu;
    [SerializeField]
    private GameObject pauseMenu;
    [SerializeField]
    private GameObject towerInfoPanel;
    [SerializeField]
    private GameObject waveInfoPanel;
    [SerializeField]
    private GameObject waveButton;


    private Tower selectedTower;

    /*


         */
    private void Awake()
    {

    }

    // Start is called before the first frame update
    void Start()
    {
        ChangeMoneyText(Money);
        ChangeHealthText(Health);
        gameOverMenu.SetActive(false);
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
        towerInfoPanel.active = false;
        betweenWaves = true;
        waveButton.active = true;
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
            ClickedBtn = towerBtn;
            Hover.Instance.Activate(towerBtn.Sprite);
        }

    }

    public void BuyTower(TowerBtn towerBtn)
    {
        Hover.Instance.Deactivate();
        ChangeMoney(towerBtn.Price,false);
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
    private void ChangeHealthText(int quantity)
    {
        healthText.text = quantity.ToString();
    }

    public void LoseLife()
    {
        Health--;
        ChangeHealthText(Health);
    }
    public void GameOver()
    {
        gameOver = true;
        gameOverMenu.SetActive(true);
    }

    //TOWERS
    public void SelectTower(Tower tower)
    {
        if(selectedTower != null)
        {
            DeselectTower();
        }
        selectedTower = tower;
        selectedTower.Select();
    }
    public void DeselectTower()
    {
        if (selectedTower != null)
        {
            selectedTower.Select();
        }
        selectedTower = null;
        towerInfoPanel.active = false;
    }

    //OPTIONS
    public void Pause()
    {
        if (paused)
        {
            Time.timeScale = 1;
            paused = false;
            pauseMenu.SetActive(false);
        }
        else
        {
            Time.timeScale = 0;
            paused = true;
            pauseMenu.SetActive(true);
        }
    }
    public void ChangeMoney(int amount, bool isSum)
    {
        if (isSum)
        {
            Money = Money + amount;
        }
        else
        {
            Money = Money - amount;
        }
        ChangeMoneyText(Money);
    }
    public void ShowInfoTower(Tower tower)
    {
        towerInfoPanel.active = true;
        SetTowerInfo(tower);
    }

    private void SetTowerInfo(Tower tower)
    {
        for (int i = 0; i < towerInfoPanel.transform.childCount; i++)
        {
            GameObject child = towerInfoPanel.transform.GetChild(i).gameObject;
            TowerMenuSwitch(child, tower);
        }
    }

    private void TowerMenuSwitch(GameObject component, Tower tower)
    {
        switch (component.name)
        {
            case "Image":
                component.GetComponent<Image>().sprite = tower.sprite;
                break;
            case "TowerNameAndLevel":
                component.GetComponent<Text>().text = tower.towerName + " - " + tower.level;//change to pub method
                break;
            case "DamageSpeed":
                component.GetComponent<Text>().text = tower.GetAttackAndSpeed();
                break;
            case "Specials":
                component.GetComponent<Text>().text = tower.GetSpecialText();
                break;
            case "Buttons":
                SetupTowerButtons(component);
                break;
            default:
                break;
        }
    }
    public void SetupTowerButtons(GameObject buttonContainer)
    {
        buttonContainer.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = selectedTower.CheckUpgradeable() ? "Upgrade\n$"+selectedTower.GetUpgradePrice(): "Fully Upgraded";
        buttonContainer.transform.GetChild(1).GetChild(0).GetComponent<Text>().text = "Sell\n$"+ selectedTower.GetSalePrice();
    }
    //tower menu actions
    public void BuyUpgrade()
    {
        if (Money >= selectedTower.GetUpgradePrice() && selectedTower.CheckUpgradeable())
        {
            ChangeMoney(selectedTower.GetUpgradePrice(), false);
            selectedTower.LevelUp();
            SetTowerInfo(selectedTower);
        }
    }
    public void SellTower()
    {
        ChangeMoney(selectedTower.GetSalePrice(), true);
        TileScript tile = LevelManager.Instance.Tiles[new Pointer((int)selectedTower.transform.position.x, (int)selectedTower.transform.position.y)];
        tile.RemoveTower();
        towerInfoPanel.active = false;
    }

    /*
    *               *  
    * WAVE ACTIONS  *
    *               *
    */

    //Create Waves
    public void NextWave()
    {
        betweenWaves = false;
        waveButton.active = false;
        nCreeps = WaveManager.Instance.GetCurrentWave().isBoss ? 1 : 30;
        float speed = WaveManager.Instance.GetCurrentWave().group ? 0.5f : 0.9f;
        StartCoroutine(LoopWave(nCreeps, speed));
    }

    private GameObject setCreep()
    {
        GameObject creepObj = WaveManager.Instance.GetCurrentWave().enemy;
        Creep creep = creepObj.GetComponent<Creep>();
        creep.money = WaveManager.Instance.GetCurrentWave().reward;
     
        return creepObj;
    }
    private void SpawnWave()
    {
        GameObject creep = setCreep();
        LevelManager.Instance.GeneratePath();
        Creep thisCreep = Instantiate(creep.GetComponent<Creep>());
        thisCreep.Spawn();
    }
    private IEnumerator LoopWave(int n, float speed)
    {
        for (int i = 0; i < n; i++)
        {
            SpawnWave();
            yield return new WaitForSeconds(speed);

        }
    }

    //End Waves
    private void WaveEnd()
    {
        WaveManager.Instance.NextWave();
        waveButton.active = true;
        betweenWaves = true;
    }
    public void DeadOrGoalCreature()
    {
        nCreeps--;
        if (nCreeps <= 0)
        {
            WaveEnd();
        }
    }

    //next wave info
    private void NextWaveInfo()
    {
        Wave nextWave = WaveManager.Instance.GetNextWaveInfo();
        if (nextWave != null)
        {
            //reller info
        }
        else
        {
            //ultima hola
        }
    }

}

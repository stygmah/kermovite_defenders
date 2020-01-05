using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{


    //Main stats
    public int Money;
    public int Health = 5;
    public float Interest;
    private int SpecialPoints;
    private int Score;
    private int inReactor;

    //Game states
    private bool gameOver = false;
    public bool paused = false;

    //Tower selector
    public TowerBtn ClickedBtn { get; set; }

    //Creep/Wave Control
    private int nCreeps;
    private int creepsKilled;
    private bool betweenWaves;


    //UI GameObjects
    [SerializeField]
    private Text moneyText;
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
    [SerializeField]
    private GameObject endWaveInfo;
    [SerializeField]
    private GameObject specialPanel;
    [SerializeField]
    private GameObject victoryPanel;
    [SerializeField]
    private GameObject tutorialMenu;
    [SerializeField]
    private GameObject auxKermovite;

    //speed btn
    private GameObject fastForwardBtn;
    private GameObject normalSpeedBtn;

    public bool inTutorial;
    private int tutorialIndex;
    GameObject[] tutorialMessages;

    private Tower selectedTower;



    /*


         */
    private void Awake()
    {
        pauseMenu.transform.parent.gameObject.active = true;
        if (SoundController.Instance) Destroy(SoundController.Instance.gameObject);
        if (SelectLevelManager.Instance)
        {
            if(SelectLevelManager.Instance.skipTutorial) Destroy(SelectLevelManager.Instance.gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        ChangeMoneyText(Money);
        gameOverMenu.SetActive(false);
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
        towerInfoPanel.active = false;
        betweenWaves = true;
        waveButton.active = true;
        Interest = 0.03f;
        SpecialPoints = 1;
        SetSpecialPanelText();
        ActivateDeactivateSpecialButtons();
        Score = 0;
        inReactor = Health;

        //set color
        Text colorWaveInfo = endWaveInfo.GetComponent<Text>();
        colorWaveInfo.color = new Color(colorWaveInfo.color.r, colorWaveInfo.color.g, colorWaveInfo.color.b, 0);

        //set speed buttons
        fastForwardBtn = GameObject.Find("FastForward");
        normalSpeedBtn = GameObject.Find("NormalSpeed");

        fastForwardBtn.active = true;
        normalSpeedBtn.active = false;


        if (IsTutorialSelected()) StartTutorial();
    }

    // Update is called once per frame
    void Update()
    {
        CancelOption();
    }

    public void PickTower(TowerBtn towerBtn)
    {
        if (Money >= towerBtn.Price && !paused)
        {
            ClickedBtn = towerBtn;
            Hover.Instance.Activate(towerBtn.Sprite, towerBtn.TowerPrefab);
        }

    }


    //TODO: Start methods, group into set values, set texts, etc

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

    public void LoseLife()
    {
        Health--;
    }

    public void GameOver()
    {
        gameOver = true;
        gameOverMenu.SetActive(true);
        Time.timeScale = 0;
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
        if (gameOver || victoryPanel.active) return;
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
        if (paused) return;
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
                component.GetComponent<Image>().sprite = tower.GetFullSprite();
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
        if (paused) return;
        betweenWaves = false;
        waveButton.active = false;
        nCreeps = WaveManager.Instance.GetCurrentWave().isBoss ? 1 : 30;
        if (WaveManager.Instance.GetCurrentWave().group) nCreeps = 50; 
        creepsKilled = 0;
        float spawnSpeed = WaveManager.Instance.GetCurrentWave().enemy.GetComponent<Creep>().spawnRate;//TODO: Spawn speed
        ActivateDeactivateSpecialButtons();
        StartCoroutine(LoopWave(nCreeps, spawnSpeed));
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
        NormalSpeed();
        PayInterest();
        betweenWaves = true;
        waveButton.active = true;
        SetEndInfoMessage();
        StartCoroutine(ShowEndWaveInfo());
        GainSpecialPoint();
        Score += CalculateScore(WaveManager.Instance.GetCurrentWave());
        WaveManager.Instance.NextWave();
        ActivateDeactivateSpecialButtons();
    }
    private void PayInterest()
    {
        Money += (int)(Money * Interest);
        ChangeMoneyText(Money);
    }
    public void DeadOrGoalCreature(bool killed)
    {
        if(killed) creepsKilled++;
        nCreeps--;
        if (nCreeps <= 0)
        {
            WaveEnd();
        }
    }
    private IEnumerator ShowEndWaveInfo()
    {
        Text txt = endWaveInfo.GetComponent<Text>();
        yield return StartCoroutine(FadeInMessage(txt));
        yield return new WaitForSeconds(2.5f);
        yield return StartCoroutine(FadeOutMessage(txt));
    }
    private IEnumerator FadeOutMessage(Text sr)
    {
        for (float i = 1; i >= 0; i -= Time.deltaTime * 2)
        {
            // set color with i as alpha
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, i);
            yield return null;
        }
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 0);
    }
    private IEnumerator FadeInMessage(Text sr)
    {
        for (float i = 0; i <= 0; i += Time.deltaTime)
        {
            // set color with i as alpha
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, i);
            yield return null;
        }
        sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 1);
    }
    private void SetEndInfoMessage()
    {
        Wave wave = WaveManager.Instance.GetCurrentWave();
        string interestMsg = "Interest: $" + Money + " x " + ((int)(Interest*100f))+1 + "% = $"+ (int)(Money * Interest);
        string points = "Points: $" + wave.reward +
               " * Killed Units:" + creepsKilled +
               ":10 * Kermovite: " + Health +
               " = "+ CalculateScore(wave);//TODO: Replace Kermovite;
        string special = wave.isBoss ? "Earned 2 Special Points!": "";
        endWaveInfo.GetComponent<Text>().text = interestMsg + "\n" + points + "\n" + special;
    }
    //next wave info
    public void SetInfoNextWave()
    {
        Wave next = WaveManager.Instance.GetCurrentWave();
        if (WaveManager.Instance.IsLast())
        {
            waveInfoPanel.transform.GetChild(0).GetComponent<Image>().sprite = null;
            waveInfoPanel.transform.GetChild(1).GetComponent<Text>().text = "WAVES ENDED";
            waveInfoPanel.transform.GetChild(2).GetComponent<Text>().text = "";
        }
        else
        {

            waveInfoPanel.transform.GetChild(0).GetComponent<Image>().sprite = next.enemy.GetComponent<SpriteRenderer>().sprite;//FALLA ESTE
            waveInfoPanel.transform.GetChild(1).GetComponent<Text>().text = "Wave: " + WaveManager.Instance.GetWaveN() + " - " + next.enemy.GetComponent<Creep>().nameCreep;
            waveInfoPanel.transform.GetChild(2).GetComponent<Text>().text = next.enemy.GetComponent<Creep>().GetHealth() + "HP - $" + next.reward;
        }

    }
    //special panel
    private void SetSpecialPanelText()
    {
        specialPanel.transform.GetChild(2).GetComponent<Text>().text = "Special points: "+SpecialPoints;
    }
    private void ActivateDeactivateSpecialButtons()
    {
        //TODO: One line conditionals depending on price
        if (SpecialPoints > 0 && betweenWaves)
        {
            specialPanel.transform.GetChild(0).GetComponent<Button>().interactable = true;
            specialPanel.transform.GetChild(1).GetComponent<Button>().interactable = true;
        }
        else
        {
            specialPanel.transform.GetChild(0).GetComponent<Button>().interactable = false;
            specialPanel.transform.GetChild(1).GetComponent<Button>().interactable = false;
        }
    }
    private void GainSpecialPoint()
    {
        if (WaveManager.Instance.IsBoss())
        {
            SpecialPoints += 3;
            SetSpecialPanelText();
            ActivateDeactivateSpecialButtons();
        }
    }
    private void SubtractSpecialPoint(int amount)
    {
        if ((SpecialPoints - amount) >=0)
        {
            SpecialPoints -= amount;
            SetSpecialPanelText();
            ActivateDeactivateSpecialButtons();
        }
    }
    public void SpendOnInterest()
    {
        Interest += 0.01f;
        SubtractSpecialPoint(1);
        string msg = "Interest increment to " + (int)(Interest * 100f) + "%";
        CustomMessage(msg);
    }
    public void SpendOnCollect()
    {
        SubtractSpecialPoint(1);
        string msg = "All the Kermovite lying around has been returned to the reactor";
        CustomMessage(msg);
    }
    public void CustomMessage(string msg)
    {
        endWaveInfo.GetComponent<Text>().text = msg;
        StartCoroutine(ShowEndWaveInfo());
    }
    //Score
    public int CalculateScore(Wave wave)
    {
        return wave.isBoss ? wave.reward * creepsKilled * Health + Money : wave.reward * (creepsKilled / 10) * Health + +Money;
    }
    //End
    public void Victory()
    {
        victoryPanel.transform.GetChild(0).GetComponent<Text>().text = "Score: " + Score;
        victoryPanel.active = true;
        Time.timeScale = 0;
    }
    public GameObject GetAuxKermovite()
    {
        return auxKermovite;
    }


    //TUTORIAL
    private bool IsTutorialSelected()
    {
        if (SelectLevelManager.Instance)
        {
            return !SelectLevelManager.Instance.skipTutorial;
        }else
        {
            return false;
        }
    }

    private void StartTutorial()
    {
        tutorialMenu.active = true;
        tutorialMessages = GetTutorialMessages();
        inTutorial = true;
        tutorialIndex = 0;
        tutorialMessages[tutorialIndex].active = true;
    }
    private GameObject[] GetTutorialMessages()
    {
        GameObject[] tutorialMessages = new GameObject[tutorialMenu.transform.childCount];
        for (int i = 0; i < tutorialMenu.transform.childCount; i++)
        {
            tutorialMessages[i] = tutorialMenu.transform.GetChild(i).gameObject;
        }
        return tutorialMessages;
    }
    public void NextTutorialMessage()
    {
        if (tutorialMessages.Length-1 > tutorialIndex)
        {
            tutorialMessages[tutorialIndex].active = false;
            tutorialIndex++;
            tutorialMessages[tutorialIndex].active = true;
        }
        else
        {
            EndTutorial();
        }
    }
    private void EndTutorial()
    {
        tutorialMenu.active = false;
        inTutorial = false;
        Destroy(SelectLevelManager.Instance.gameObject);
    }

    //speed buttons
    public void NormalSpeed()
    {
        if (gameOver || paused || victoryPanel.active) return;
        Time.timeScale = 1;
        fastForwardBtn.active = true;
        normalSpeedBtn.active = false;
    }

    public void FastForward()
    {
        if (gameOver || paused || victoryPanel.active) return;
        Time.timeScale = 4;
        fastForwardBtn.active = false;
        normalSpeedBtn.active = true;
    }
    public bool RemoveFromReactor()
    {
        if (inReactor > 0)
        {
            inReactor--;
            return true;
        }
        else
        {
            return false;
        }
    }public void RestoreReactor()
    {
        inReactor = Health;
    }
}

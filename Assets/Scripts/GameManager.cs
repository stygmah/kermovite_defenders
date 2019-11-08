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

    /// <summary>
    /// Changle later for wave
    /// </summary>
    [SerializeField]
    private GameObject creep;
    private Creep nextCreep;

    [SerializeField]
    private GameObject gameOverMenu;
    [SerializeField]
    private GameObject pauseMenu;

    private Tower selectedTower;


    private void Awake()
    {
        nextCreep = creep.GetComponent<Creep>();
    }

    // Start is called before the first frame update
    void Start()
    {
        ChangeMoneyText(Money);
        ChangeHealthText(Health);
        gameOverMenu.SetActive(false);
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
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
    private void ChangeHealthText(int quantity)
    {
        healthText.text = quantity.ToString();
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

    public void SelectTower(Tower tower)
    {
        Debug.Log("se");
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
    }
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
}

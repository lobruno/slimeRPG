using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public GameObject lostTransition;
    public GameObject winTransition;
    public Character character;
    public List<GameObject> enemiesModel = new List<GameObject>();
    public List<GameObject> enemies;

    public TextMeshProUGUI coinsText;
    private int coins = 0;
    public TextMeshProUGUI ATKLevelText;
    public TextMeshProUGUI ATKCostText;
    private int ATKLevel = 1;
    private int ATKCost = 1 ;
    public TextMeshProUGUI ASPDLevelText;
    public TextMeshProUGUI ASPDCostText;
    private int ASPDLevel = 1;
    private int ASPDCost = 1;
    public TextMeshProUGUI HPLevelText;
    public TextMeshProUGUI HPCostText;
    private int HPLevel = 1;
    private int HPCost = 1;

    public float CHAttackSpeed;
    public float CHhealth;
    public float CHattackValue;


    public float ENAttackSpeed;
    public float ENhealth;
    public float ENattackValue;
    public float ENSpeed;

    public TextMeshProUGUI LevelText;
    public int Level = 1;
    // Start is called before the first frame update
    void Awake()
    {
        LoadPrefs();
        ENAttackSpeed = 2f;
        ENhealth = 80f ;
        ENattackValue = 5f;
        ENSpeed = 0.55f;
    }
    void Start()
    {
        SpawnEnemy();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (enemies.Count == 0)
        {
            Level++;
            StartCoroutine(Win());
            LevelText.text = "Level " + Level + "";
            character.health = character.HEALTH_MAX;
            character.healthBar.fillAmount = character.health/character.HEALTH_MAX;
            SpawnEnemy();
        }

        for(int i = 0; i < enemies.Count; i++)
        {
            if(enemies[i] == null) { enemies.RemoveAt(i); }
        }
    }

    public void UpgradeATK()
    {
        if(coins >= ATKCost)
        {
            coins -= ATKCost;
            ATKLevel += 1;
            CHattackValue = 10 + 10 * ATKLevel;
            character.attackValue = CHattackValue;
            ATKLevelText.text = "Lv " + ATKLevel + "";
            ATKCost = ATKLevel;
            ATKCostText.text = ATKCost + "";
            coinsText.text = coins + "";
            SavePrefs();
        }
    }

    public void UpgradeASPD()
    {
        if (coins >= ASPDCost)
        {
            coins -= ASPDCost;
            ASPDLevel += 1;
            CHAttackSpeed =1f - 0.01f * ASPDLevel;
            if (CHAttackSpeed < 0.1f) { CHAttackSpeed = 0.1f; }
            character.AttackSpeed = CHAttackSpeed;
            ASPDLevelText.text = "Lv " + ASPDLevel + "";
            ASPDCost = ASPDLevel;
            ASPDCostText.text = ASPDCost + "";
            coinsText.text = coins + "";
            SavePrefs();

        }
    }

    public void UpgradeHP()
    {
        if (coins >= HPCost)
        {
            coins -= HPCost;
            HPLevel += 1;
            CHhealth = 80 + HPLevel * 10;
            character.HEALTH_MAX = CHhealth;
            HPLevelText.text = "Lv " + HPLevel + "";
            HPCost = HPLevel;
            HPCostText.text = HPCost + "";
            coinsText.text = coins + "";
            SavePrefs();

        }
    }

    public void LoadPrefs()
    {
        Level = PlayerPrefs.GetInt("Level", 1);
        LevelText.text = "Level " + Level + "";
        coins = PlayerPrefs.GetInt("Coins", 0);
        coinsText.text = coins + "";

        CHAttackSpeed = PlayerPrefs.GetFloat("CHAttackSpeed", 1f);
        CHattackValue = PlayerPrefs.GetFloat("CHattackValue", 10f);
        CHhealth = PlayerPrefs.GetFloat("CHhealth", 80f);

        ATKLevel = PlayerPrefs.GetInt("ATKLevel", 1);
        ATKLevelText.text = "Lv " + ATKLevel + "";
        ATKCost = ATKLevel;
        ATKCostText.text = ATKCost + "";

        ASPDLevel = PlayerPrefs.GetInt("ASPDLevel", 1);
        ASPDLevelText.text = "Lv " + ASPDLevel + "";
        ASPDCost = ASPDLevel;
        ASPDCostText.text = ASPDCost + "";

        HPLevel = PlayerPrefs.GetInt("HPLevel", 1);
        HPLevelText.text = "Lv " + HPLevel + "";
        HPCost = HPLevel;
        HPCostText.text = HPCost + "";

        CHhealth += HPLevel * 10;
        CHattackValue += 10 * ATKLevel;
        CHAttackSpeed -= 0.01f * ASPDLevel;
    }

    public void SavePrefs()
    {
        PlayerPrefs.SetInt("Level", Level);
        PlayerPrefs.SetInt("Coins", coins);

        PlayerPrefs.SetFloat("CHAttackSpeed", 1f);
        PlayerPrefs.SetFloat("CHattackValue", 10f);
        PlayerPrefs.SetFloat("CHhealth", 80f);

        PlayerPrefs.SetInt("ATKLevel", ATKLevel);
        

        PlayerPrefs.SetInt("ASPDLevel", ASPDLevel);
        
        
        PlayerPrefs.SetInt("HPLevel", HPLevel);
       
    }

    public void AddMoney()
    {
        coins += 2 + (int)(Level*0.2);
        coinsText.text = coins + "";
        SavePrefs();

    }

    private void OnApplicationQuit()
    {
        SavePrefs();
    }

    void SpawnEnemy()
    {
        for (int i = 0; i < 5; i++)
        {
            Enemy enemy;
            if (i == 0)
            {
                enemy = Instantiate(enemiesModel[Random.Range(0, enemiesModel.Count)], new Vector3(Random.Range(3.5f, 4.5f), 0.5f, Random.Range(-1f, -5f)), Quaternion.Euler(0f, -90f, 0f)).GetComponent<Enemy>();
            }
            else
            {
                 enemy = Instantiate(enemiesModel[Random.Range(0, enemiesModel.Count)], new Vector3(enemies[i - 1].transform.position.x + Random.Range(0.25f, 1.5f), 0.5f, Random.Range(-1f, -6f)), Quaternion.Euler(0f, -90f, 0f)).GetComponent<Enemy>();
            }
            enemy.gameManager = gameObject.GetComponent<GameManager>();
            enemies.Add(enemy.gameObject);
        }
    }

    public void Restart()
    {
        StartCoroutine(RestartGame());
    }


    IEnumerator RestartGame()
    {
        lostTransition.GetComponent<Animator>().SetBool("trans", true);

        yield return new WaitForSeconds(0.5f);

        Clean();

        if (enemies.Count == 0)
        {
            character.health = character.HEALTH_MAX;
            character.healthBar.fillAmount = character.health / character.HEALTH_MAX;
            character.gameObject.SetActive(true);
            SpawnEnemy();
            lostTransition.GetComponent<Animator>().SetBool("trans", false);
        }
        else
        {
            Clean();
        }
    }


    IEnumerator Win()
    {
        winTransition.GetComponent<Animator>().SetBool("trans", true);

        yield return new WaitForSeconds(0.5f);
        winTransition.GetComponent<Animator>().SetBool("trans", false);
    }


    void Clean()
    {
        for (int i = 0; i < enemies.Count; i++)
        {
            Destroy(enemies[i].gameObject);
            enemies.RemoveAt(i);
        }

        if (enemies.Count > 0)
        {
            Clean();
        }
    }

}

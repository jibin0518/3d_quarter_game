using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Net.Http.Headers;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;
using UnityEngine.Rendering;

public class GameManager : MonoBehaviour
{
    public GameObject menuCa;
    public GameObject gameCa;
    public Player player;
    public Boss boss;
    public int stage;
    public float playTime;
    public bool isbattle;
    public int enemycntA;
    public int enemycntB;
    public int enemycntC;
    public int enemycntD;

    public bool gamesta;

    public GameObject noticeshop;

    public Transform[] enemyZones;
    public GameObject[] enemies;
    public List<int> enemylist;
    public Shop shopmanger;

    public GameObject startzone;
    public GameObject menupanel;
    public GameObject gamepanel;
    public GameObject overpanel;
    public GameObject escpanel;
    public Text maxscoreTxT;
    public Text scoreTxT;
    public Text stageTxT;
    public Text playTimeTxT;
    public Text playerHealth;
    public Text playerAmmoTxT;
    public Text playerCoineTxt;
    public Image weapon1img;
    public Image weapon2img;
    public Image weapon3img;
    public Image weaponRimg;
    public Text enemyATxt;
    public Text enemyBTxt;
    public Text enemyCTxt;
    public RectTransform bossHealthGr;
    public RectTransform bossHealthBar;
    public Text curscoreText;
    public Text bestText;
    public Text plusText;
    public string[] cointext;

    void Awake()
    {
        enemylist = new List<int>();
        maxscoreTxT.text = string.Format("{0:n0}", PlayerPrefs.GetInt("MaxScore"));

        if (PlayerPrefs.HasKey("MaxScore"))
        {
            PlayerPrefs.SetInt("MaxScore", 0);
        }
    }
    public void GameStart()
    {
        menuCa.SetActive(false);
        gameCa.SetActive(true);

        menupanel.SetActive(false);
        gamepanel.SetActive(true);

        player.gameObject.SetActive(true);
    }

    public void GameOver()
    {
        gamepanel.SetActive(false);
        overpanel.SetActive(true);
        curscoreText.text = scoreTxT.text;

        int maxscore = PlayerPrefs.GetInt("MAXscore");
        if(player.score > maxscore)
        {
            bestText.gameObject.SetActive(true);
            PlayerPrefs.SetInt("MAXscore" , player.score);
        }
    }

    public void Restart()
    {
        SceneManager.LoadScene(0);
    }

    public void StageStart()
    {
        gamesta = true;
        startzone.SetActive(false);
        foreach (Transform zone in enemyZones)
        {
            zone.gameObject.SetActive(true);
        }
        isbattle = true;
        StartCoroutine(inbattle());
    }

    void StageEnd()
    {
        gamesta = false;
        player.transform.position = Vector3.up * 0.2f;

        startzone.SetActive(true);
        foreach (Transform zone in enemyZones)
        {
            zone.gameObject.SetActive(false);
        }
        player.coin += 150;
        StartCoroutine(notice());
        noticeshop.SetActive(true);
        isbattle = false;
        stage++;
    }

    IEnumerator inbattle()
    {
        noticeshop.SetActive(false);
        if (stage % 10 == 0)
        {
            enemycntD++;
            GameObject instantenemy = Instantiate(enemies[3], enemyZones[0].position, enemyZones[0].rotation);
            Enemy enemy = instantenemy.GetComponent<Enemy>();
            enemy.target = player.transform;
            enemy.manager = this;
            boss = instantenemy.GetComponent<Boss>();
        }
        else
        {
            for (int index = 0; index < stage; index++)
            {
                int ran = Random.Range(0, 3);
                enemylist.Add(ran);

                switch (ran)
                {
                    case 0:
                        enemycntA++;
                        break;
                    case 1:
                        enemycntB++;
                        break;
                    case 2:
                        enemycntC++;
                        break;
                }
            }

            while (enemylist.Count > 0)
            {
                int ranzone = Random.Range(0, 4);
                GameObject instantenemy = Instantiate(enemies[enemylist[0]], enemyZones[ranzone].position, enemyZones[ranzone].rotation);
                Enemy enemy = instantenemy.GetComponent<Enemy>();
                enemy.target = player.transform;
                enemy.manager = this;
                enemylist.RemoveAt(0);
                yield return new WaitForSeconds(4f);
            }
        }
        while (enemycntA + enemycntB + enemycntC + enemycntD > 0)
        {
            yield return null;
        }

        yield return new WaitForSeconds(4f);

        boss = null;
        StageEnd();
    }

    void Update()
    {
        if (isbattle) playTime += Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Escape)) Exit();
    }

    void Exit()
    {
        if (shopmanger.Itemshop == true || shopmanger.Weaponshop == true)
        {
            shopmanger.itemExit();
            shopmanger.weaponExit();
        }
        if(shopmanger.Itemshop==false && shopmanger.Weaponshop==false) escpanel.SetActive(true);
    }

    IEnumerator notice()
    {
        plusText.text = cointext[0];
        yield return new WaitForSeconds(2.5f);
        plusText.text = cointext[1];
    }

    void LateUpdate()
    {
        scoreTxT.text = string.Format("{0:n0}", player.score);
        stageTxT.text = "STAGE" + stage;

        int hour = (int)(playTime / 3600);
        int min = (int)((playTime - hour * 3600) / 60);
        int second = (int)(playTime % 60);

        playTimeTxT.text = string.Format("{0:00}", hour) + ":" + string.Format("{0:00}", min) + ":" + string.Format("{0:00}", second);

        playerHealth.text = player.health + "/" + player.maxhealth;
        playerCoineTxt.text = string.Format("{0:n0}", player.coin);
        if (player.nowweapon == null)
        {
            playerAmmoTxT.text = "- /" + player.ammo;
        }
        else if (player.nowweapon.type == Weapon.Type.mel)
        {
            playerAmmoTxT.text = "- /" + player.ammo;
        }
        else
        {
            playerAmmoTxT.text = player.nowweapon.curammo + " /" + player.ammo;
        }
        weapon1img.color = new Color(1, 1, 1, player.hasweapon[0] ? 1 : 0);
        weapon2img.color = new Color(1, 1, 1, player.hasweapon[1] ? 1 : 0);
        weapon3img.color = new Color(1, 1, 1, player.hasweapon[2] ? 1 : 0);
        weaponRimg.color = new Color(1, 1, 1, player.hasgre > 0 ? 1 : 0);

        enemyATxt.text = enemycntA.ToString();
        enemyBTxt.text = enemycntB.ToString();
        enemyCTxt.text = enemycntC.ToString();

        if (boss != null)
        {
            bossHealthGr.anchoredPosition = Vector3.down * 30;
            bossHealthBar.localScale = new Vector3((float)boss.curhealth / boss.maxhealth, 1, 1);
        }
        else
        {
            bossHealthGr.anchoredPosition = Vector3.up * 200;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class GameManager : MonoBehaviour
{
    public enum GameStat
    {
        Start,
        Ready,
        Battle,
        GameOver
    }
    public GameStat stat = GameStat.Start;

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

    public bool itemshop;
    public bool weaponshop;
    public bool escboo;

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
    public GameObject minimap;

    public Text maxscoreTxT;
    public Text scoreTxT;
    public Text stageTxT;

    public Text EscStageTxt;
    public Text EscScoreTxt;

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

        ShowMinimap(false);
    }

    void Update()
    {
        ShopOpen();
        if (isbattle) playTime += Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Escape)) EscControl();
    }

    //상점 열기
    void ShopOpen()
    {
        if (Input.GetButtonDown("ItemShop")) itemshop = true;
        if (Input.GetButtonDown("WeaponShop")) weaponshop = true;

        if (itemshop && !gamesta)
        {
            shopmanger.Itemshopopen();
            shopmanger.weaponExit();
            player.openshop = true;
        }

        if (weaponshop && !gamesta)
        {
            shopmanger.Weaponshopopen();
            shopmanger.itemExit();
            player.openshop = true;
        }
    }

    void EscControl()
    {
        shopmanger.Escpoanelscen();
        ShopClose();
    }

    //상점 닫기
    void ShopClose()
    {
        shopmanger.itemExit();
        shopmanger.weaponExit();
        itemshop = false;
        weaponshop = false;
        player.openshop = false;
    }

    //게임 시작
    public void GameStart()
    {
        menuCa.SetActive(false);
        gameCa.SetActive(true);

        menupanel.SetActive(false);
        gamepanel.SetActive(true);

        player.gameObject.SetActive(true);

        stat = GameStat.Ready;
    }

    //게임오버
    public void GameOver()
    {
        stat = GameStat.GameOver;
        gamepanel.SetActive(false);
        overpanel.SetActive(true);
        ShowMinimap(false);
        curscoreText.text = scoreTxT.text;

        int maxscore = PlayerPrefs.GetInt("MAXscore");
        if (player.score > maxscore)
        {
            bestText.gameObject.SetActive(true);
            PlayerPrefs.SetInt("MAXscore", player.score);
        }
    }

    //다시시작
    public void Restart()
    {
        SceneManager.LoadScene(0);
    }

    //스테이지시작
    public void StageStart()
    {
        if (!escboo)
        {
            ShowMinimap(true);
            player.openshop = false;
            gamesta = true;
            startzone.SetActive(false);
            ShopClose();
            foreach (Transform zone in enemyZones)
            {
                zone.gameObject.SetActive(true);
            }
            isbattle = true;
            StartCoroutine(inbattle());
            stat = GameStat.Battle;
        }
    }

    //스테이지 끝남
    void StageEnd()
    {
        ShowMinimap(false);
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
        stat = GameStat.Ready;
    }

    //스테이지시작(몬스터 소환)
    IEnumerator inbattle()
    {
        ShowMinimap(true);
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

    //스테이지 클리어 보너스 코인
    IEnumerator notice()
    {
        plusText.text = cointext[0];
        yield return new WaitForSeconds(2.5f);
        plusText.text = cointext[1];
    }

    //다른 메뉴
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

    //미니맵 끄기
    void ShowMinimap(bool value)
    {
        if (value) minimap.SetActive(true);
        else minimap.SetActive(false);
    }
}
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

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

    public GameObject startzone;
    public GameObject menupanel;
    public GameObject gamepanel;
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

    void Awake()
    {
        maxscoreTxT.text = string.Format("{0:n0}", PlayerPrefs.GetInt("MaxScore"));
    }
    public void GameStart()
    {
        menuCa.SetActive(false);
        gameCa.SetActive(true);

        menupanel.SetActive(false);
        gamepanel.SetActive(true);

        player.gameObject.SetActive(true);
    }

    public void StageStart()
    {
        startzone.SetActive(false);
        isbattle = true;
        StartCoroutine(inbattle());
    }

    void StageEnd()
    {
        player.transform.position = Vector3.up * 0.2f;

        startzone.SetActive(true);
        isbattle = false;
        stage++;
    }

    IEnumerator inbattle()
    {
        yield return new WaitForSeconds(5);
        StageEnd();
    }

    void Update()
    {
        if (isbattle)
        {
            playTime += Time.deltaTime;
        }
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
        if(player.nowweapon == null)
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

        bossHealthBar.localScale = new Vector3((float)boss.curhealth / boss.maxhealth, 1, 1);
    }
}

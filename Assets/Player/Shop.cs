using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    public GameObject Itemshop;
    public GameObject Weaponshop;
    public GameManager manger;
    public GameObject Escpanel;
    public GameObject playerhealthbar;
    public Text itemtalktext;
    public Text weatalktext;
    public string[] talkdata;
    public void Itemshopopen()
    {
        Itemshop.SetActive(true);
        Weaponshop.SetActive(false);
        playerhealthbar.SetActive(false);
        manger.itemshop = true;
        StartCoroutine(talk());
    }

    public void Weaponshopopen()
    {
        Weaponshop.SetActive(true);
        Itemshop.SetActive(false);
        playerhealthbar.SetActive(false);
        manger.weaponshop = true;
        StartCoroutine(talk());
    }

    public void weaponExit()
    {
        Weaponshop.SetActive(false);
        playerhealthbar.SetActive(true);
        manger.weaponshop = false;
    }

    public void itemExit()
    {
        Itemshop.SetActive(false);
        playerhealthbar.SetActive(true);
        manger.itemshop = false;
    }

    public void Escpoanelscen()
    {
        Time.timeScale = 0;
        Escpanel.SetActive(true);
        manger.escboo = true;
    }
    public void Contigame()
    {
        Time.timeScale = 1;
        Escpanel.SetActive(false);
        manger.escboo = false;
    }
    public void Exitgame()
    {
        Time.timeScale = 1;
        Escpanel.SetActive(false);
        manger.GameOver();
    }

    IEnumerator talk()
    {
        itemtalktext.text = talkdata[1];
        weatalktext.text = talkdata[0];
        yield return null;
    }
}

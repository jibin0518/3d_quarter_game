using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    public GameObject Itemshop;
    public GameObject Weaponshop;
    public GameManager manger;
    public GameObject Escpanel;
    public void Itemshopopen()
    {
        Itemshop.SetActive(true);
        Weaponshop.SetActive(false);
        manger.itemshop = true;
    }

    public void Weaponshopopen()
    {
        Weaponshop.SetActive(true);
        Itemshop.SetActive(false);
        manger.weaponshop = true;
    }

    public void weaponExit()
    {
        Weaponshop.SetActive(false);
        manger.weaponshop = false;
    }

    public void itemExit()
    {
        Itemshop.SetActive(false);
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
}

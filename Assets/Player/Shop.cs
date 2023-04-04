using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    bool itemshop=false;
    bool weaponshop=false;
    public bool escboo=false;
    public GameObject Itemshop;
    public GameObject Weaponshop;
    public GameManager manger;
    public GameObject Escpanel;

    void Update()
    {
        key();
        shopokey();
    }

    void key()
    {
        itemshop = Input.GetButtonDown("ItemShop");
        weaponshop = Input.GetButtonDown("WeaponShop");
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (escboo) Contigame();
            else if (!itemshop && !weaponshop) Escpoanelscen();
        }
    }

    void shopokey()
    {
            if (itemshop && manger.gamesta!=true && !escboo)
            {
                Itemshop.SetActive(true);
                Weaponshop.SetActive(false);
                itemshop = true;
            }
            if (weaponshop && manger.gamesta != true && !escboo)
            {
                Weaponshop.SetActive(true);
                Itemshop.SetActive(false);
                weaponshop = true;
            }
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Weaponshop.SetActive(false);
                weaponshop = false;
                Itemshop.SetActive(false);
                itemshop = false;
            }
    }
    public void Itemshopopen()
    {
        Itemshop.SetActive(true);
        Weaponshop.SetActive(false);
        itemshop = true;
    }

    public void Weaponshopopen()
    {
        Weaponshop.SetActive(true);
        Itemshop.SetActive(false);
        weaponshop = true;
    }

    public void weaponExit()
    {
        Weaponshop.SetActive(false);
        weaponshop = false;
        Debug.Log(weaponshop);
    }

    public void itemExit()
    {
        Itemshop.SetActive(false);
        itemshop = false;
    }

    public void Escpoanelscen()
    {
        Escpanel.SetActive(true);
        escboo = true;
    }
    public void Contigame()
    {
        Escpanel.SetActive(false);
        escboo = false;
    }
    public void Exitgame()
    {
        Escpanel.SetActive(false);
        manger.GameOver();
    }
}

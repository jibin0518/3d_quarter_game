using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    bool itemshop;
    bool weaponshop;
    public GameObject Itemshop;
    public GameObject Weaponshop;
    public GameManager manger;


    void Update()
    {
        key();
        shopokey();
    }

    void key()
    {
        itemshop = Input.GetButtonDown("ItemShop");
        weaponshop = Input.GetButtonDown("WeaponShop");
    }

    void shopokey()
    {
            if (itemshop && manger.gamesta!=true)
            {
                //Debug.Log("111");
                Itemshop.SetActive(true);
                Weaponshop.SetActive(false);
                itemshop = true;
            }
            if (weaponshop)
            {
                //Debug.Log("222");
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

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Weaponshop.SetActive(false);
            weaponshop = false;
            Itemshop.SetActive(false);
            itemshop = false;
        }
    }

    public void Weaponshopopen()
    {
        Weaponshop.SetActive(true);
        Itemshop.SetActive(false);
        weaponshop = true;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Weaponshop.SetActive(false);
            weaponshop = false;
            Itemshop.SetActive(false);
            itemshop = false;
        }
    }

    public void weaponExit()
    {
        Weaponshop.SetActive(false);
        weaponshop = false;
        //Debug.Log("1!1");
    }

    public void itemExit()
    {
        Itemshop.SetActive(false);
        itemshop = false;
        //Debug.Log("2!2");
    }
}

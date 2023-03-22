using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    bool itemshop;
    bool weaponshop;
    public GameObject Itemshop;
    public GameObject Weaponshop;
    public Player player;

    //public Text talktext;
    //public int[] itemprice;
    //public int[] weaponprice;
    //public string[] talkdata;


    void Update()
    {
        key();
        shop();
    }

    void key()
    {
        itemshop = Input.GetButtonDown("ItemShop");
        weaponshop = Input.GetButtonDown("WeaponShop");
    }

    void shop()
    {
            if (itemshop)
            {
                Debug.Log("111");
                Itemshop.SetActive(true);
                Weaponshop.SetActive(false);
                itemshop = true;
            }
            if (weaponshop)
            {
                Debug.Log("222");
                Weaponshop.SetActive(true);
                Itemshop.SetActive(false);
                weaponshop = true;
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

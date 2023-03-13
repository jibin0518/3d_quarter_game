using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    bool itemshop;
    bool weaponshop;
    public GameObject Itemshop;
    public GameObject Weaponshop;

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
        if (itemshop && !weaponshop)
        {
            Debug.Log("111");
            Itemshop.SetActive(true);
            itemshop = true;
        }
        if (weaponshop && !itemshop)
        {
            Debug.Log("222");
            Weaponshop.SetActive(true);
            weaponshop=true;
        }
    }

    public void weaponExit()
    {
        Weaponshop.SetActive(false);
        weaponshop = false;
        Debug.Log("1!1");
    }

    public void itemExit()
    {
        Itemshop.SetActive(false);
        itemshop = false;
        Debug.Log("2!2");
    }
}

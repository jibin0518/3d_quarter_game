using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.UI;

public class Weapon_Shop_Buy : MonoBehaviour
{
    public Player player;

    public GameObject weaponpanel;
    public Text talktext;
    public int[] weaponprice;
    public string[] talkdata;
    bool weaponpay=false;

    public void Buy(int index)
    {
        int price = weaponprice[index];
        if (index > 0) {
            StartCoroutine(readyitem());
        }
        if (weaponpay == true && index!>0)
        {
            StartCoroutine(buyinfo());
        }
        if (price > player.coin)
        {
            StopCoroutine(talk());
            StartCoroutine(talk());
            return;
        }
        else if (!weaponpay && index == 0)
        {
            player.coin -= price;
            player.hasweapon[index + 2] = true;
            weaponpay = true;
        }
    }

    IEnumerator talk()
    {
        talktext.text = talkdata[0];
        yield return new WaitForSeconds(2f);
        talktext.text = talkdata[1];
    }
    IEnumerator buyinfo()
    {
        talktext.text = talkdata[2];
        yield return new WaitForSeconds(2f);
        talktext.text = talkdata[1];
    }
    IEnumerator readyitem()
    {
        talktext.text = talkdata[3];
        yield return new WaitForSeconds(2f);
        talktext.text = talkdata[1];
    }
}

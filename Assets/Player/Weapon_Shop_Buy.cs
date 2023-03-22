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

    public void Buy(int index)
    {
        int price = weaponprice[index];
        if (price > player.coin)
        {
            StopCoroutine(talk());
            StartCoroutine(talk());
            return;
        }
        player.coin -= price;
        player.hasweapon[index+1] = true;
    }

    IEnumerator talk()
    {
        talktext.text = talkdata[0];
        yield return new WaitForSeconds(2f);
        talktext.text = talkdata[1];
    }
}

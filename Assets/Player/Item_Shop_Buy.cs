using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.UI;

public class Item_Shop_Buy : MonoBehaviour
{
    public Player player;

    public GameObject itempanel;
    public Text talktext;
    public int[] itemprice;
    public string[] talkdata;

    public void Buy(int index)
    {
        int gap;
        int price = itemprice[index];
        if (price > player.coin)
        {
            StopCoroutine(talk());
            StartCoroutine(talk());
            return;
        }
        if(player.health!=player.maxhealth && index == 0)
        {
            player.health += 20;
            player.coin -= price;
            if(player.health >= player.maxhealth)
            {
                gap = player.health - player.maxhasgre;
                player.health -= gap;
                StartCoroutine(maxhealth());
            }
        }
        if (player.ammo!=player.maxammo && index == 1)
        {
            player.ammo += 30;
            player.coin -= price;
            if (player.ammo >= player.maxammo)
            {
                gap = player.ammo - player.maxammo;
                player.ammo -= gap;
                StartCoroutine(maxammo());
            }
        }
        if(player.hasgre != player.maxhasgre && index==2)
        {
            player.grendes[player.hasgre].SetActive(true);
            player.hasgre ++;
            player.coin -= price;
            if (player.health >= player.maxhealth)
            {
                gap = player.hasgre - player.maxhasgre;
                player.hasgre -= gap;
                StartCoroutine(maxhasgre());
            }
        }
    }

    IEnumerator talk()
    {
        talktext.text = talkdata[0];
        yield return new WaitForSeconds(2f);
        talktext.text = talkdata[1];
    }
    IEnumerator maxhealth()
    {
        talktext.text = talkdata[2];
        yield return new WaitForSeconds(2f);
        talktext.text = talkdata[1];
    }
    IEnumerator maxammo()
    {
        talktext.text = talkdata[3];
        yield return new WaitForSeconds(2f);
        talktext.text = talkdata[1];
    }
    IEnumerator maxhasgre()
    {
        talktext.text = talkdata[4];
        yield return new WaitForSeconds(2f);
        talktext.text = talkdata[1];
    }
}

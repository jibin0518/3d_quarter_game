using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class Item_Shop_Buy : MonoBehaviour
{
    public Player player;

    public GameObject itempanel;
    public Text talktext;
    public int[] itemprice;
    public string[] talkdata;

    public void Buy(int index)
    {
        int price = itemprice[index];
        if (price > player.coin)
        {
            StopCoroutine(talk());
            StartCoroutine(talk());
            return;
        }
        player.coin -= price;
        if(player.health!=player.maxhealth && index == 0)
        {
            player.health += 20;
        }
        if(player.ammo!=player.maxammo && index == 1)
        {
            player.ammo += 30;
        }
        if(player.hasgre != player.maxhasgre && index==2)
        {
            player.grendes[player.hasgre].SetActive(true);
            player.hasgre ++;
        }
    }

    IEnumerator talk()
    {
        talktext.text = talkdata[0];
        yield return new WaitForSeconds(2f);
        talktext.text = talkdata[1];
    }
}

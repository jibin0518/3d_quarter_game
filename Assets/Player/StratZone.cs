using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StratZone : MonoBehaviour
{
    public GameManager manger;

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            manger.StageStart();
        }
    }
}

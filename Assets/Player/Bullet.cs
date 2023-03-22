using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage;
    public bool ismel;
    public bool isrock;

    void OnCollisionEnter(Collision collision)
    {
        if(!isrock & collision.gameObject.tag == "ground")
        {
            Destroy(gameObject, 1.5f);
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (!ismel && other.gameObject.tag == "wall")
        {
            Destroy(gameObject);
        }
    }

}

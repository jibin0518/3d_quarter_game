using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int damage;

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "ground")
        {
            Destroy(gameObject, 1.5f);
        }
        else if (collision.gameObject.tag == "wall")
        {
            Destroy(gameObject);
        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "wall" || other.gameObject.tag == "ground")
        {
            Destroy(gameObject);
        }
    }

}

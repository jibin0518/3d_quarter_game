using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Rock : Bullet
{
    Rigidbody rigid;
    float angular = 2;
    float scaleval = 0.1f;
    bool isshot;
    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        StartCoroutine(gainpowertime());
        StartCoroutine(gainpower());
    }

    // Update is called once per frame
    IEnumerator gainpowertime()
    {
        yield return new WaitForSeconds(2.2f);
        isshot = true;
    }
    IEnumerator gainpower() {
        while (!isshot)
        {
            angular += 0.02f;
            scaleval += 0.005f;
            transform.localScale = Vector3.one * scaleval;
            rigid.AddTorque(transform.right * angular, ForceMode.Acceleration);
            yield return null;
        }
    }
}

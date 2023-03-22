using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public enum Type {mel, ran };
    public Type type;
    public int damage;
    public float rate;
    public int maxammo;
    public int curammo;

    public BoxCollider melarea;
    public TrailRenderer effect;
    public Transform bulletpos;
    public GameObject bullet;
    public Transform bulletcasepos;
    public GameObject bulletcase;

    public void use()
    {
        if(type == Type.mel)
        {
            StopCoroutine("Swing");
            StartCoroutine("Swing");
        }
        else if(type == Type.ran && curammo>0)
        {
            curammo--;
            StartCoroutine("Shot");
        }
    }
    IEnumerator Swing()
    {
        //1
        yield return new WaitForSeconds(0.1f);
        melarea.enabled = true;
        effect.enabled = true;
        //2
        yield return new WaitForSeconds(0.3f);
        melarea.enabled = false;
        //3
        yield return new WaitForSeconds(0.3f);
        effect.enabled = false;
    }
    IEnumerator Shot()
    {
        GameObject instantbullet = Instantiate(bullet, bulletpos.position, bulletpos.rotation);
        Rigidbody bulletrigid = instantbullet.GetComponent<Rigidbody>();
        bulletrigid.velocity = bulletpos.forward * 50;

        yield return null;

        GameObject instantcase = Instantiate(bulletcase, bulletcasepos.position, bulletcasepos.rotation);
        Rigidbody caserigid = instantcase.GetComponent<Rigidbody>();
        Vector3 casevec = bulletcasepos.forward * Random.Range(-0.3f, -0.2f) + Vector3.up * Random.Range(0.2f, 0.3f);
        caserigid.AddForce(casevec, ForceMode.Impulse);
        caserigid.AddForce(Vector3.up * 0.8f, ForceMode.Impulse);
    }
}

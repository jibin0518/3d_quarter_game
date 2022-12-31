using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class Enemy : MonoBehaviour
{
    // Start is called before the first frame update
    public int maxhealth;
    public int curhealth;
    public Transform target;
    public bool ischase;

    bool attackmel;

    Rigidbody rigid;
    BoxCollider BoxCollider;
    Material mat;
    NavMeshAgent nav;
    Animator anim;

    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        BoxCollider = GetComponent<BoxCollider>();
        mat = GetComponentInChildren<MeshRenderer>().material;
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();

        Invoke("chasestart", 2);
    }

    void chasestart()
    {
        ischase = true;
        anim.SetBool("isWalk", true);
    }

    void Update()
    {
        if (ischase)
        {
            nav.SetDestination(target.position);
        }
    }
    void freezevel()
    {
        if (ischase)
        {
            rigid.velocity = Vector3.zero;
            rigid.angularVelocity = Vector3.zero;
        }
    }

    void FixedUpdate()
    {
        freezevel();
    }
    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "mel")
        {
            Weapon weapon = other.GetComponent<Weapon>();
            curhealth -= weapon.damage;
            Vector3 reactvect = transform.position - other.transform.position;
            StartCoroutine(OnDamage(reactvect,false));
            //---------------------------------------------
            reactvect = reactvect.normalized;
            reactvect += Vector3.up;

            rigid.AddForce(reactvect * 1.3f, ForceMode.Impulse);
        }
        else if(other.tag == "bullet")
        {
            Bullet bullet = other.GetComponent<Bullet>();
            curhealth -= bullet.damage;
            Vector3 reactvect = transform.position - other.transform.position;
            Destroy(other.gameObject);
            StartCoroutine(OnDamage(reactvect,false));
        }
    }

    public void hitbyGre(Vector3 explorepos)
    {
        curhealth -= 100;
        Vector3 reactvect = transform.position - explorepos;
        StartCoroutine(OnDamage(reactvect,true));
    }

    IEnumerator OnDamage(Vector3 reactvect, bool isGre)
    {
        mat.color = Color.red;
        yield return new WaitForSeconds(0.1f);

        if (curhealth > 0)
        {
            mat.color = Color.white;
        }
        else
        {
            mat.color = Color.gray;
            gameObject.layer = 12;
            ischase = false;
            nav.enabled = false;
            anim.SetTrigger("doDie");

            if (isGre)
            {
                reactvect = reactvect.normalized;
                reactvect += Vector3.up * 3;

                rigid.AddForce(reactvect * 0.8f, ForceMode.Impulse);
            }
            else
            {
                reactvect = reactvect.normalized;
                reactvect += Vector3.up;

                rigid.freezeRotation = false;
                rigid.AddForce(reactvect * 0.8f, ForceMode.Impulse);
                rigid.AddTorque(reactvect * 15, ForceMode.Impulse);

                Destroy(gameObject, 2.5f);
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class Enemy : MonoBehaviour
{
    public enum Type {A,B,C };
    public Type enemytype;
    public int maxhealth;//초대체력
    public int curhealth;//현제체력

    public Transform target;//따라갈 목표
    public BoxCollider melarea;//공격범위
    public GameObject bullet;

    public bool ischase;
    public bool isattack;

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
        if (nav.enabled)
        {
            nav.SetDestination(target.position);
            nav.isStopped = !ischase;
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

    void targeting()
    {
        float targetRad = 0;
        float targetRange = 0;

        switch (enemytype)
        {
            case Type.A:
                targetRad = 0.5f;
                targetRange = 0.05f;
                break;
            case Type.B:
                targetRad = 0.5f;
                targetRange = 1.5f;
                break;
            case Type.C:
                targetRad = 0.5f;
                targetRange = 2f;
                break;
        }

        RaycastHit[] rayhit = Physics.SphereCastAll(transform.position, targetRad, transform.forward, targetRange, LayerMask.GetMask("player"));
        if (rayhit.Length > 0 && !isattack)
        {
            StartCoroutine((attack()));
        }
    }

    IEnumerator attack()
    {
        ischase = false;
        isattack = true;
        anim.SetBool("isAttack", true);

        switch (enemytype)
        {
            case Type.A:
                yield return new WaitForSeconds(0.1f);
                melarea.enabled = true;

                yield return new WaitForSeconds(1f);
                melarea.enabled = false;

                yield return new WaitForSeconds(1f);
                break;
            case Type.B:
                yield return new WaitForSeconds(0.2f);
                rigid.AddForce(transform.forward * 6, ForceMode.Impulse);
                melarea.enabled = true;

                yield return new WaitForSeconds(0.5f);
                rigid.velocity = Vector3.zero;
                melarea.enabled = false;

                yield return new WaitForSeconds(2f);
                break;
            case Type.C:
                yield return new WaitForSeconds(0.5f);
                GameObject instantbullet = Instantiate(bullet, transform.position, transform.rotation);
                Rigidbody rigidbullet = instantbullet.GetComponent<Rigidbody>();
                rigidbullet.velocity = transform.forward * 6;

                yield return new WaitForSeconds(2f);
                break;
        }

        ischase = true;
        isattack = false;
        anim.SetBool("isAttack", false);
    }

    void FixedUpdate()
    {
        targeting();
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

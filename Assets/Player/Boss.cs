using Microsoft.Win32.SafeHandles;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Boss : Enemy
{
    public GameObject missile;
    public Transform missileA;
    public Transform missileB;

    Vector3 lookvec;
    Vector3 taunvec;

    public bool islook;

    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        BoxCollider = GetComponent<BoxCollider>();
        meshs = GetComponentsInChildren<MeshRenderer>();
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();

        nav.isStopped = true;

        StartCoroutine(think());
    }

    // Update is called once per frame
    void Update()
    {
        if (isdead)
        {
            StopAllCoroutines();
            return;
        }
        if (islook)
        {
            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical");
            lookvec = new Vector3(h, 0, v) * 1.5f;
            transform.LookAt(target.position + lookvec);
        }
        else
        {
            nav.SetDestination(taunvec);
        }
    }

    IEnumerator think()
    {
        yield return new WaitForSeconds(0.1f);
        int ranAction = Random.Range(0, 5);
        switch (ranAction)
        {
            case 0:
            case 1:
                StartCoroutine(missileshot());
                break;
            case 2:
            case 3:
                StartCoroutine(rockshot());
                break;
            case 4:
                StartCoroutine(taunt());
                break;

        }
    }
    IEnumerator missileshot()
    {
        anim.SetTrigger("doShot");
        yield return new WaitForSeconds(0.3f);
        GameObject instantmissileA = Instantiate(missile,missileA.position,missileA.rotation);
        BossMissile bossmissileA = instantmissileA.GetComponent<BossMissile>();
        bossmissileA.target = target;

        yield return new WaitForSeconds(0.2f);
        GameObject instantmissileB = Instantiate(missile, missileB.position, missileB.rotation);
        BossMissile bossmissileB = instantmissileB.GetComponent<BossMissile>();
        bossmissileB.target = target;

        yield return new WaitForSeconds(2f);
        StartCoroutine(think());
    }
    IEnumerator rockshot()
    {
        islook = false;
        anim.SetTrigger("doBigshot");
        Instantiate(bullet, transform.position, transform.rotation);
        yield return new WaitForSeconds(3f);
        islook = true;
        StartCoroutine(think());
    }
    IEnumerator taunt()
    {
        taunvec = target.position + lookvec;
        islook = false;
        nav.isStopped = false;
        BoxCollider.enabled = false;
        anim.SetTrigger("doTaunt");

        yield return new WaitForSeconds(1.5f);
        melarea.enabled = true;

        yield return new WaitForSeconds(0.5f);
        melarea.enabled = false;

        yield return new WaitForSeconds(1f);
        islook = true;
        nav.isStopped = true;
        BoxCollider.enabled = true;
        StartCoroutine(think());
    }
}

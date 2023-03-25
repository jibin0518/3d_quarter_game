using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

public class Item : MonoBehaviour
{
    public enum Type {am,co,gre, hea,wea };
    public Transform target;
    public Type type;
    public int val;

    Rigidbody rigid;
    SphereCollider sphereCollider;
    NavMeshAgent nav;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        sphereCollider = GetComponent<SphereCollider>();
        nav = GetComponent<NavMeshAgent>();
    }
    void Update()
    {
        transform.Rotate(Vector3.up * 50 * Time.deltaTime);
        if(type == Type.co && nav.enabled)
        {
            Player player = target.GetComponent<Player>();
            targeting();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "ground")
        {
            rigid.isKinematic = true;
            sphereCollider.enabled = false;
        }
    }
    void targeting()
    {
        
        nav.SetDestination(target.position);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SubsystemsImplementation;

public class Grenade : MonoBehaviour
{
    public GameObject meshObj;
    public GameObject effectObj;
    public Rigidbody rigid;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(explore());
    }

    // Update is called once per frame
    IEnumerator explore()
    {
        yield return new WaitForSeconds(3);
        rigid.velocity = Vector3.zero;
        rigid.angularVelocity = Vector3.zero;
        meshObj.SetActive(false);
        effectObj.SetActive(true);

        RaycastHit[] rayhits = Physics.SphereCastAll(transform.position, 2, Vector3.up, 0, LayerMask.GetMask("enemy"));
        foreach (RaycastHit hitObj in rayhits)
        {
            hitObj.transform.GetComponent<Enemy>().hitbyGre(transform.position);
        }
        Destroy(gameObject, 3);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using Unity.VisualScripting.ReorderableList;
using UnityEditor;
using UnityEngine;
using UnityEngine.SubsystemsImplementation;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    
    public GameObject[] weapons;//����.�迭
    public bool[] hasweapon;//������ �ִ� ����.�迭
    public GameObject[] grendes;//�������ִ� ����ź
    public Camera followca;// ���콺 ����
    public GameObject GreObj;//����ź ��ü
    public GameManager manger;

    public Shop shopmanger;

    public GameObject Itemshop;
    public GameObject Weaponshop;

    public int ammo;//�Ѿ�
    public int coin;//��
    public int health;//ü��
    public int hasgre;//����ź
    public int score;

    public RectTransform playerhealthbar;

    public int maxammo;//�ִ��Ѿ�
    public int maxcoin;//�ִ뵷
    public int maxhealth;//�ִ�ü��
    public int maxhasgre;//�ִ����ź

    public string[] text;
    public Text noticetext;

    public bool openshop=false;

    float haxis;//�¿�
    float vaxis;//����
    
    bool gdown;//����ź ��ô
    bool rundown;//�������Է�
    bool space;//�����Է�
    bool idown;//������(����) �Ա��Է�
    bool cooltm;//��Ÿ��
    bool isjp;//�����Է�
    bool isdg;//�����̵��Է�

    bool sdown1;//1�� �׸�
    bool sdown2;//2�� �׸�
    bool sdown3;//3�� �׸�
    bool isswap;//���� ��ü

    bool rdown;//�������Է�
    bool isreload;//������
    bool reloadely;//��������Ÿ��

    bool isdamge;//���ؾִϸ��̼�

    bool isborder;
    bool isdead;

    bool fdown;//����
    bool isfry=true;//�����غ�

    int equipweapon = -2;//���⸦ ��ü�ϱ� ���� ����
    int slide;//�����̵�

    float fdel;//���ݵ�����
    float runspd;//�ȱ� �޸��� ����
    float spd;//�ӵ�

    Vector3 movevec;//�̵�����
    Vector3 dgvec;//���ϱ����

    Rigidbody rigid;//����
    Animator anim;//�ִϸ��̼�

    GameObject nearob;//���� ������
    public Weapon nowweapon;//���ݹ���
    MeshRenderer[] meshs;


    private void Start()
    {
        slide = 1;
    }
    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
        meshs = GetComponentsInChildren<MeshRenderer>();
        PlayerPrefs.SetInt("MaxScore",112500);
    }
     
    void Update()
    {
        getin();
        move();
        turn();
        //jump();
        dodge();
        inter();
        swap();
        attack();
        reload();
        grenade();
    }

    void LateUpdate()
    {
        playerhealthbar.localScale = new Vector3((float)health / maxhealth, 1, 1);
    }

    //�Է�
    void getin()
    {
        haxis = Input.GetAxisRaw("Horizontal");
        vaxis = Input.GetAxisRaw("Vertical");
        rundown = Input.GetButton("Run");
        space = Input.GetButtonDown("Jump");
        idown = Input.GetButtonDown("Interation");
        sdown1 = Input.GetButtonDown("swap1");
        sdown2 = Input.GetButtonDown("swap2");
        sdown3 = Input.GetButtonDown("swap3");
        fdown = Input.GetButton("Fire1");
        rdown = Input.GetButtonDown("Reload");
        gdown = Input.GetButtonDown("Fire2");
    }

    //�̵�
    void move()
    {
        movevec = new Vector3(haxis, 0, vaxis).normalized;

        if (isdg || isdead)
        {
            movevec = dgvec;
        }

        if (manger.escboo)
        {
            movevec = Vector3.zero;
        }

        spd = (isreload || isswap || fdown) ? 2f : 5f;
        runspd = (rundown || space) ? 0.8f : 0.3f;

        //�޸���
        if (!isborder && !isdead){
            transform.position += movevec * spd * runspd * slide * Time.deltaTime;
        }

        anim.SetBool("isWolk", movevec != Vector3.zero);
        anim.SetBool("isRun", rundown);

        
    }

    //���� ����
    void turn()
    {
        transform.LookAt(transform.position + movevec);

        if (fdown && !isdead && !manger.escboo)
        {
            Ray ray = followca.ScreenPointToRay(Input.mousePosition);
            RaycastHit rayhit;
            if (Physics.Raycast(ray, out rayhit, 100))
            {
                Vector3 nextvect = rayhit.point - transform.position;
                nextvect.y = 0;
                transform.LookAt(transform.position + nextvect);
            }
        }
    }

    //����(�۵�����)
    void jump()
    {
        if (space && !isjp && movevec == Vector3.zero && !isdead)
        {
            rigid.AddForce(Vector3.up * 3.5f, ForceMode.Impulse);
            anim.SetBool("isJump", true);
            anim.SetTrigger("doJump");
            isjp = true;
        }
    }

    //����ź
    void grenade()
    {
        if(hasgre == 0)
        {
            return;
        }
        if(gdown && !isreload && !isswap && !isdead && !manger.escboo)
        {
            Ray ray = followca.ScreenPointToRay(Input.mousePosition);
            RaycastHit rayhit;
            if (Physics.Raycast(ray, out rayhit, 100))
            {
                Vector3 nextvect = rayhit.point - transform.position;
                nextvect.y = 3.5f;

                GameObject instantgre = Instantiate(GreObj, transform.position, transform.rotation);
                Rigidbody rigidgre = instantgre.GetComponent<Rigidbody>();
                rigidgre.AddForce(nextvect, ForceMode.Impulse);
                rigidgre.AddTorque(Vector3.back * 5, ForceMode.Impulse);

                hasgre--;
                grendes[hasgre].SetActive(false);
            }
        }
    }

    //�����̵�
    void dodge()
    {
        if (space && movevec != Vector3.zero && !isjp && !isdg && !isswap && !isdead && !manger.escboo /*&& !cooltm*/)
        {
            dgvec = movevec;
            slide = 2;
            anim.SetTrigger("doDodge");
            isdg = true;
            //cooltm = true; ��Ÿ��

            Invoke("Dodgeout", 0.4f);
            //Invoke("Cooltime", 3); ��Ÿ��
        }
    }

    //�����̵� ������ �������� ���� �����
    void Dodgeout()
    {
        slide = 1;
        isdg = false;
    }

    //�����̵� ��Ÿ��
    /*
    void Cooltime()
    {
        cooltm = false;
    }
    */

    //���� ������ �ٲٱ�
    void swap()
    {
        int weaponidex = -1;
        if (sdown1) weaponidex = 0;
        if (sdown2) weaponidex = 1;
        if (sdown3) weaponidex = 2;
        if (sdown1 || sdown2 || sdown3 && !isdg && !manger.escboo)
        {
            if (nowweapon != null)
            {
                nowweapon.gameObject.SetActive(false);
            }
            if (hasweapon[weaponidex] == false)
            {
                nowweapon = null;
            }
            if (hasweapon[weaponidex] == true)
            {
                nowweapon = weapons[weaponidex].GetComponent<Weapon>();
                weapons[weaponidex].gameObject.SetActive(true);

                if (weaponidex != equipweapon)
                {
                    anim.SetTrigger("doswap");
                }
                equipweapon = weaponidex;
                isswap = true;
                Invoke("swapout", 0.4f);
            }
            
        }
    }

    //���� ������ �ٲٱ� ������
    void swapout()
    {
        isswap = false;
    }

    //������ �Ա�
    void inter()
    {
        if (idown && nearob != null && !isdg && !isdead && !manger.escboo)
        {
            if (nearob.tag == "weapon")
            {
                Item item = nearob.GetComponent<Item>();
                int weaponindex = item.val;
                hasweapon[weaponindex] = true;

                Destroy(nearob);
            }
        }
    }

    //����
    void attack()
    {
        if(nowweapon == null)
        {
            return;
        }
        fdel += Time.deltaTime;
        isfry = nowweapon.rate < fdel;

        if(fdown && isfry && !isdg && !isswap && !reloadely && !isdead && !manger.escboo && !openshop)
        {
            nowweapon.use();
            anim.SetTrigger(nowweapon.type == Weapon.Type.mel ? "doswing" : "doshot");
            fdel = 0;
            isfry = false;
        }
        if (nowweapon.curammo == 0 && nowweapon.type!=Weapon.Type.mel)
        {
            StartCoroutine(noticearm());
        }
    }

    IEnumerator noticearm()
    {
        noticetext.text = text[1];
        yield return null;
        if (nowweapon.curammo > 0)
        {
            noticetext.text = text[0];
        }
    }

    //�����Է� 
    void reload()
    {
        if(nowweapon != null && nowweapon.type == Weapon.Type.ran && ammo > 0 && !isdg && rdown && !isswap && isfry && nowweapon.curammo != nowweapon.maxammo && !reloadely && !isdead && !manger.escboo)
        {
            if (isreload == false)
            {
                anim.SetTrigger("doReload");
                isreload = true;
                reloadely = true;
            }

            Invoke("reloadout", 1.4f);
            Invoke("reloaddrely", 1.5f);
        }
    }

    //����
    void reloadout()
    {
        int reammo = nowweapon.maxammo - nowweapon.curammo;
        if (ammo > nowweapon.maxammo || nowweapon.maxammo < nowweapon.curammo + ammo)
        {
            nowweapon.curammo = nowweapon.maxammo;
            ammo -= reammo;
        }
        else if(nowweapon.maxammo > nowweapon.curammo + ammo)
        {
            nowweapon.curammo += ammo;
            ammo = 0;
        }
        isreload=false;
    }

    void reloaddrely()
    {
        reloadely = false;
    }

    //����(�۵�����)
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "ground")
        {
            anim.SetBool("isJump", false);
            isjp = false;
        }
    }

    //������ ++;
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "item")
        {
            Item item = other.GetComponent<Item>();
            switch (item.type)
            {
                case Item.Type.am:
                    if (ammo < maxammo)
                    {
                        ammo += item.val;
                    }
                    break;
                case Item.Type.co:
                    if (coin < maxcoin)
                    {
                        coin += item.val;
                    }
                    break;
                case Item.Type.hea:
                    if (health < maxhealth)
                    {
                        health += item.val;
                    }
                    break;
                case Item.Type.gre:
                    if (hasgre < maxhasgre)
                    {
                        grendes[hasgre].SetActive(true);
                        hasgre += item.val;
                    }
                    break;

            }
            Destroy(other.gameObject);
        }
        else if(other.tag == "enemybullet")
        {
            if (!isdamge)
            {
                Bullet enemybullet = other.GetComponent<Bullet>();
                health -= enemybullet.damage;

                bool isbossatk = other.name == "Boss_Mel_area";

                StartCoroutine(ondamage(isbossatk));
            }
            if (other.GetComponent<Rigidbody>() != null)
            {
                Destroy(other.gameObject);
            }
        }
    }

    IEnumerator ondamage(bool isbossatk)
    {
        isdamge = true;
        foreach(MeshRenderer mesh in meshs)
        {
            mesh.material.color = Color.yellow;
        }
        if (isbossatk)
        {
            rigid.AddForce(transform.forward * -3.5f, ForceMode.Impulse);
        }

        if (health <= 0 && !isdead)
        {
            ondie();
        }

        yield return new WaitForSeconds(1f);

        isdamge = false;
        foreach(MeshRenderer mesh in meshs)
        {
            mesh.material.color = Color.white;
        }
        if (isbossatk)
        {
            rigid.velocity = Vector3.zero;
        }
    }

    void ondie()
    {
        anim.SetTrigger("doDie");
        isdead = true;
        manger.GameOver();
    }

    //���� ����
    void Stoptowall()
    {
        Debug.DrawRay(transform.position, transform.forward * 1f, Color.red);
        isborder = Physics.Raycast(transform.position, transform.forward , 0.5f, LayerMask.GetMask("wall"));
    }
    void FixedUpdate()
    {
        Stoptowall();
        //���� ��Ҵ��� ��ȣ
        /*
        if (isborder)
        {
            Debug.Log("!!!");
        }
        */
    }

    //�ָ鿡 ���Ⱑ ������
    void OnTriggerStay(Collider other)
    {
        if (other.tag == "weapon")
        {
            nearob = other.gameObject;
        }

        //Debug.Log(nearob.name);
    }

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "weapon")
        {
            nearob = null;
        }
    }
}

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
    
    public GameObject[] weapons;//무기.배열
    public bool[] hasweapon;//가지고 있는 무기.배열
    public GameObject[] grendes;//가지고있는 수류탄
    public Camera followca;// 마우스 따라감
    public GameObject GreObj;//수류탄 객체
    public GameManager manger;

    public Shop shopmanger;

    public GameObject Itemshop;
    public GameObject Weaponshop;

    public int ammo;//총알
    public int coin;//돈
    public int health;//체력
    public int hasgre;//수류탄
    public int score;

    public RectTransform playerhealthbar;

    public int maxammo;//최대총알
    public int maxcoin;//최대돈
    public int maxhealth;//최대체력
    public int maxhasgre;//최대수류탄

    public string[] text;
    public Text noticetext;

    public bool openshop=false;

    float haxis;//좌우
    float vaxis;//상하
    
    bool gdown;//수류탄 투척
    bool rundown;//딜리기입력
    bool space;//점프입력
    bool idown;//아이템(무기) 먹기입력
    bool cooltm;//쿨타임
    bool isjp;//점프입력
    bool isdg;//슬라이드입력

    bool sdown1;//1번 그릇
    bool sdown2;//2번 그릇
    bool sdown3;//3번 그릇
    bool isswap;//무기 교체

    bool rdown;//재장전입력
    bool isreload;//재장전
    bool reloadely;//재장전쿨타임

    bool isdamge;//피해애니메이션

    bool isborder;
    bool isdead;

    bool fdown;//공격
    bool isfry=true;//공격준비

    int equipweapon = -2;//무기를 교체하기 위한 변수
    int slide;//슬라이드

    float fdel;//공격딜레이
    float runspd;//걷기 달리기 구분
    float spd;//속도

    Vector3 movevec;//이동방향
    Vector3 dgvec;//피하기방향

    Rigidbody rigid;//물리
    Animator anim;//애니메이션

    GameObject nearob;//무변 아이템
    public Weapon nowweapon;//지금무기
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

    //입력
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

    //이동
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

        //달리기
        if (!isborder && !isdead){
            transform.position += movevec * spd * runspd * slide * Time.deltaTime;
        }

        anim.SetBool("isWolk", movevec != Vector3.zero);
        anim.SetBool("isRun", rundown);

        
    }

    //보는 방향
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

    //점프(작동중지)
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

    //수류탄
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

    //슬라이드
    void dodge()
    {
        if (space && movevec != Vector3.zero && !isjp && !isdg && !isswap && !isdead && !manger.escboo /*&& !cooltm*/)
        {
            dgvec = movevec;
            slide = 2;
            anim.SetTrigger("doDodge");
            isdg = true;
            //cooltm = true; 쿨타임

            Invoke("Dodgeout", 0.4f);
            //Invoke("Cooltime", 3); 쿨타임
        }
    }

    //슬라이드 일정한 방향으로 가게 만들기
    void Dodgeout()
    {
        slide = 1;
        isdg = false;
    }

    //슬라이드 쿨타임
    /*
    void Cooltime()
    {
        cooltm = false;
    }
    */

    //무기 아이템 바꾸기
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

    //무기 아이템 바꾸기 딜레이
    void swapout()
    {
        isswap = false;
    }

    //아이템 먹기
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

    //공격
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

    //장전입력 
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

    //장전
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

    //점프(작동중지)
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "ground")
        {
            anim.SetBool("isJump", false);
            isjp = false;
        }
    }

    //아이템 ++;
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

    //별뚫 방지
    void Stoptowall()
    {
        Debug.DrawRay(transform.position, transform.forward * 1f, Color.red);
        isborder = Physics.Raycast(transform.position, transform.forward , 0.5f, LayerMask.GetMask("wall"));
    }
    void FixedUpdate()
    {
        Stoptowall();
        //벽에 닿았는지 신호
        /*
        if (isborder)
        {
            Debug.Log("!!!");
        }
        */
    }

    //주면에 무기가 있을떄
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AllMobs : MonoBehaviour
{
    enum Team
    {
        Blue = 0,
        Red = 1
    }
    [Header("Unit ScriptableObjects")]
    [SerializeField] public Units unit;

    [Space(5)]
    [Header("General characteristics")]
    [SerializeField] Team team = Team.Blue;
    [SerializeField] int hp = 10;

    [Space(5)]
    [Header("Combat characteristecs")]
    [SerializeField] protected int forceDamage = 1;
    [SerializeField] protected float rangeAttack = 3f;
    [SerializeField] protected float timeReload = 1f;

    [Space(5)]
    [Header("Animation controller")]
    [SerializeField] protected Animator animator;


    protected Transform target = null;
    private GameObject[] targets = null;

    private NavMeshAgent agent = null;

    private void Start()
    {
        if(unit == null) { Debug.LogError("Not completed unit in mobs"); unit = new Units();}

        StarAnimSettings();
        tag = GameManager.instance.SetTagForMob((int)team); //������������� ��� ���� � ����������� �� ���� �� ���� �� ������
        GameManager.instance.startGameFite+= StartGame; //������������� �� ������� ������ ���
        agent = GetComponent<NavMeshAgent>();

        SpawnManager.instance.isOn = true;
    }

    virtual public void StarAnimSettings()
    {
        animator.enabled = false; //��������!!!
    }

    virtual public void StartGame() //����� ����������� ��� ������ �����
    {
        SetTargets();
        InvokeRepeating("SearchTarget",0,0.5f);

        animator.enabled = true;
    }

    private void Update()
    {
        if (!GameManager.instance.isPlay || target == null) {return; }

        Move();
        ActivationAttack();
    }

    virtual public void Death() //��������� ��� ������ ����
    {
        Destroy(gameObject);
    }

    public void Damage(int damage) //����� ���������� �� ��������� ����� �� ������ �����
    {
        hp-=damage;
        if (hp <= 0) { Death();}
    }

    virtual public void SearchTarget()
    {
        if(targets == null) { return; }

        float minDistance = Mathf.Infinity;

        foreach (var enemu in targets)
        {
            if(enemu == null) { continue; }

            var dist = Vector3.Distance(transform.position, enemu.transform.position);
            if (dist < minDistance)
            {
                minDistance = dist;
                target = enemu.transform;
            }
        } //����� ��������� ����������

        if(target == null) { GameManager.instance.GameOver(transform.tag);}

    }

    virtual public void Attack()
    {
        if(target == null) {  return; }
        var buf = target.GetComponent<AllMobs>();
        if(buf == null) { Debug.LogError("Not AllMobs on taget obj!!!"); return;}
        buf.Damage(forceDamage);
    }
    virtual public void ActivationAttack()
    {      
        if (agent.remainingDistance <= rangeAttack && agent.remainingDistance > 0)
        {
            animator.SetBool("attack", true);
        }
        else
        {
            animator.SetBool("attack", false);
        }
    }
    
    virtual public void Move()
    {
        if(target == null || agent == null) { Debug.LogError("No target or agent!!!"); return;}

        
        Vector3 dir = target.position - transform.position;
        //look 
        Quaternion look = Quaternion.LookRotation(dir);
        Vector3 rot = look.eulerAngles;
        transform.eulerAngles = new Vector3(0, rot.y, 0);
        //move
        agent.SetDestination(target.position);
    }

    private void SetTargets()
    {
        if(team == Team.Blue)
        {
            targets = GameManager.instance.teamRed;
        }
        else
        {
            targets = GameManager.instance.teamBlue;
        }
    }


    private void OnDestroy()
    {
        GameManager.instance.startGameFite -= StartGame;
    }
}

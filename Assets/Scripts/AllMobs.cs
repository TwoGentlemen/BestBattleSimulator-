using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(BoxCollider))]
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

    
    [HideInInspector] protected Animator animator;


    protected Transform target = null;
    private GameObject[] targets = null;

    protected NavMeshAgent agent = null;

  
    private void AddComponent()
    {
        if (unit == null) { Debug.LogError("Not completed unit in mobs"); unit = new Units(); }

        agent = GetComponent<NavMeshAgent>();
        if(agent == null) { Debug.LogError("Not comleted agentNavMesh!!!"); throw new System.Exception();}

        animator = GetComponentInChildren<Animator>();
        if(animator == null) { Debug.LogError("Not comleted animator in children!!!"); throw new System.Exception();}
    }

    private void Start()
    {

        AddComponent();
        StarAnimSettings();
        
        tag = GameManager.instance.SetTagForMob((int)team); //Устанавливаем тэг мобу в зависимости от того за кого он играет
        gameObject.layer = 2;
        GameManager.instance.startGameFite+= StartGame; //Подписываемся на событие начала боя
        SpawnManager.instance.isOn = true;

        rangeAttack = agent.stoppingDistance+2;
    }

    virtual public void StarAnimSettings()
    {
        animator.enabled = false; //ВРЕМЕННО!!!
    }

    virtual public void StartGame() //Метод срабатывает при начале битвы
    {
        SetTargets();
        InvokeRepeating("SearchTarget",0,0.5f);

        animator.enabled = true;
    }

    private void Update()
    {
        if (!GameManager.instance.isPlay || target == null) {return; }

        Move();
        
    }

    virtual public void Death() //Поведение при смерти моба
    {
        Destroy(gameObject);
    }

    public void Damage(int damage) //Метод отвечающий за получение урона от других мобов
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
        } //Поиск ближащего противника

        if(target == null) { GameManager.instance.GameOver(transform.tag);}

    }

    virtual public void Attack()
    {
        if(target == null) {  return; }
        var buf = target.GetComponent<AllMobs>();
        if(buf == null) { Debug.LogError("Not AllMobs on taget obj!!!"); return;}
        buf.Damage(forceDamage);
    }
    
    virtual public void Move()
    {
        if(target == null || agent == null) { Debug.LogError("No target or agent!!!"); return;}

        if (Vector3.Distance(target.position,transform.position) <= rangeAttack)
        {
            animator.SetBool("attack", true);
            animator.SetBool("walk", false);
        }
        else
        {
            //move
            animator.SetBool("attack", false);
            animator.SetBool("walk", true);
            agent.SetDestination(target.position);
        }

        Vector3 dir = target.position - transform.position;
        //look 
        Quaternion look = Quaternion.LookRotation(dir);
        Vector3 rot = look.eulerAngles;
        transform.eulerAngles = new Vector3(0, rot.y, 0);
       
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

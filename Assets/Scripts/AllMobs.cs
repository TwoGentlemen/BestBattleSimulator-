using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(BoxCollider))]
public class AllMobs : MyHP
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
    [SerializeField] private Team team = Team.Blue;

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
        GameManager.instance.gameOver+=GameOver; //Подписываемся на событие конца боя
        SpawnManager.instance.isOn = true;

        rangeAttack = agent.stoppingDistance+2;
    }
    public void SetMaterial(bool isBlue)
    {
        if (isBlue)
        {
            team = Team.Blue;
            GetComponentInChildren<Renderer>().material.color = Color.blue;
        }
        else
        {
            team = Team.Red;
            GetComponentInChildren<Renderer>().material.color = Color.red;
        }
        
    }
    virtual public void StarAnimSettings()
    {
        animator.enabled = false; //ВРЕМЕННО!!!
    }

    virtual public void GameOver()//Метод срабатывания при окончании игры
    {
        animator.enabled = false;
        agent.enabled = false;
        
    }
    virtual public void StartGame() //Метод срабатывает при начале битвы
    {
        SetTargets();
        InvokeRepeating("SearchTarget",0,0.5f);

        animator.enabled = true;
        if (target != null)
        {
            agent.SetDestination(target.position);
        }
    }

    private void Update()
    {
        if (!GameManager.instance.isPlay || target == null) {return; }

        Move();
        
    }

    override public void Death() //Поведение при смерти моба
    {
        Destroy(gameObject);
    }


    virtual public void SearchTarget()
    {
        if(targets == null || !GameManager.instance.isPlay) { return; }

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
        if(Vector3.Distance(target.position,transform.position) > rangeAttack) {  return; }

        var buf = target.GetComponent<MyHP>();
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
            agent.isStopped = true;
        }
        else
        {
            //move
            agent.isStopped = false;
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
        GameManager.instance.gameOver-= GameOver;
    }
}

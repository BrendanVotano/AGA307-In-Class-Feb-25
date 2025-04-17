using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class Enemy : GameBehaviour
{
    [Header("Basics")]
    public EnemyType myType;
    [Space(10)]
    public EnemyState myState;
    public PatrolType myPatrol;
    [Header("Stats")]
    public float moveDistance = 1000f;
    public float stoppingDistance = 0.3f;

    [Header("Health Bar")]
    public HealthBar healthBar;

    [Header("AI")]
    [SerializeField] private float detectDistance = 10f;
    [SerializeField] private float detectTime = 5f;
    private float currentDetectTime;
    [SerializeField] private float attackDistance = 3f;
 
    private float mySpeed;
    public int myHealth;
    private int myMaxHealth;
    private int myDamage;
    private int myScore;
    public int MyScore => myScore;
    private Transform moveToPos;    //Needed for all movement
    private Transform startPos;     //Needed for PingPong movement
    private Transform endPos;       //Needed for PingPong movement
    private bool reverse;           //Needed for PingPong movement
    private int patrolPoint;        //Needed for Linear movement;

    public Animator anim;
    public NavMeshAgent agent;

    private void ChangeSpeed(float _speed) => agent.speed = _speed;

    public void Initialize(Transform _startPos, string _name)
    {
        //anim = GetComponent<Animator>();

        switch(myType)
        {
            case EnemyType.OneHanded:
                mySpeed = 3;
                myHealth = 100;
                myDamage = 100;
                myScore = 100;
                myPatrol = PatrolType.Linear;
                break;
            case EnemyType.TwoHanded:
                mySpeed = 1;
                myHealth = 200;
                myDamage = 200;
                myScore = 50;
                myPatrol = PatrolType.PingPong;
                break;
            case EnemyType.Archer:
                mySpeed = 4;
                myHealth = 50;
                myDamage = 75;
                myScore = 200;
                myPatrol = PatrolType.Random;
                break;
            default:
                mySpeed = 2;
                myHealth = 100;
                myDamage = 10;
                myScore = 100;
                break;
        }
        myMaxHealth = myHealth;

        startPos = _startPos;
        endPos = _EM.GetRandomSpawnPoint;
        moveToPos = endPos;

        healthBar.SetName(_name);
        healthBar.UpdateHealthBar(myHealth, myMaxHealth);

        SetupAI();
    }

    private void SetupAI()
    {
        myState = EnemyState.Patrol;
        switch (myPatrol)
        {
            case PatrolType.Linear:
                moveToPos = _EM.GetSpecificSpawnPoint(patrolPoint);
                patrolPoint = patrolPoint != _EM.spawnPoints.Length - 1 ? patrolPoint + 1 : 0;
                break;

            case PatrolType.PingPong:
                moveToPos = reverse ? startPos : endPos;
                reverse = !reverse;
                break;

            case PatrolType.Random:
                moveToPos = _EM.GetRandomSpawnPoint;
                break;
        }
        agent.SetDestination(moveToPos.position);
        currentDetectTime = detectTime;
        ChangeSpeed(mySpeed);
    }

    private void Update()
    {
        if (myState == EnemyState.Die)
            return;

        //Set the animator float parameter to the agents speed
        anim.SetFloat("Speed", agent.speed);

        //Get the distance between us and the player
        float distToPlayer = Vector3.Distance(transform.position, _PLAYER.transform.position);
        if (distToPlayer < detectDistance && myState != EnemyState.Attack)
        {
            if (myState != EnemyState.Chase)
                myState = EnemyState.Detect;
        }

        switch (myState)
        {
            case EnemyState.Patrol:
                //Get the distance between us and the destination
                float distToDestination = Vector3.Distance(transform.position, moveToPos.position);
                //If the distance is close enough to endPos, run SetupAI() again
                if (distToDestination < 1)
                    SetupAI();
                break;

            case EnemyState.Detect:
                ChangeSpeed(0);
                agent.SetDestination(transform.position);
                currentDetectTime -= Time.deltaTime;
                if(distToPlayer <= detectDistance)
                {
                    myState = EnemyState.Chase;
                    currentDetectTime = detectTime;
                }
                if(currentDetectTime <= 0)
                {
                    SetupAI();
                }
                break;

            case EnemyState.Chase:
                agent.SetDestination(_PLAYER.transform.position);
                ChangeSpeed(mySpeed * 1.5f);
                if (distToPlayer > detectDistance)
                    myState = EnemyState.Detect;
                if (distToPlayer < attackDistance)
                    StartCoroutine(Attack());
                break;
        }

    }

    private IEnumerator Attack()
    {
        myState = EnemyState.Attack;
        ChangeSpeed(0);
        PlayAnimation("Attack", 3);
        yield return new WaitForSeconds(1);
        myState = EnemyState.Chase;
    }


    private IEnumerator Move()
    {
        switch(myPatrol)
        {
            case PatrolType.Linear:
                moveToPos = _EM.GetSpecificSpawnPoint(patrolPoint);
                patrolPoint = patrolPoint != _EM.spawnPoints.Length - 1 ? patrolPoint + 1 : 0;
                break;

            case PatrolType.PingPong:
                moveToPos = reverse ? startPos : endPos;
                reverse = !reverse;
                break;

            case PatrolType.Random:
                moveToPos = _EM.GetRandomSpawnPoint;
                break;
        }


        transform.LookAt(moveToPos);

        while(Vector3.Distance(transform.position, moveToPos.position) > stoppingDistance)
        {
            transform.position = Vector3.MoveTowards(transform.position, moveToPos.position, mySpeed * Time.deltaTime);
            yield return null;
        }

        yield return new WaitForSeconds(1);
        StartCoroutine(Move());
    }

    public void Hit(int _damage)
    {
        myHealth -= _damage;
        _GM.AddScore(myScore);
        healthBar.UpdateHealthBar(myHealth, myMaxHealth);

        if (myHealth <= 0)
            myHealth = 0;

        if (myHealth == 0)
            Die();
        else
            PlayAnimation("Hit", 3);
    }

    public void Die()
    {
        myState = EnemyState.Die;
        ChangeSpeed(0);
        agent.SetDestination(transform.position);
        GetComponent<Collider>().enabled = false;
        PlayAnimation("Die", 3);
        StopAllCoroutines();
    }

    private void PlayAnimation(string _animationName, int _animationCount)
    {
        int rnd = Random.Range(1, _animationCount+1);
        anim.SetTrigger(_animationName + rnd);
    }


    /*
    private IEnumerator Move()
    {
        for(int i = 0; i < moveDistance; i++)
        {
            transform.Translate(Vector3.forward * Time.deltaTime);
            yield return null;
        }
        transform.Rotate(Vector3.up * 180);
        yield return new WaitForSeconds(Random.Range(1f, 3f));
        StartCoroutine(Move());
    }
    */
}

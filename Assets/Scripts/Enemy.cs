using UnityEngine;
using System.Collections;

public class Enemy : GameBehaviour
{
    [Header("Basics")]
    public EnemyType myType;
    [Space(10)]
    public PatrolType myPatrolType;
    [Header("Stats")]
    public float moveDistance = 1000f;
    public float stoppingDistance = 0.3f;

    [Header("Health Bar")]
    public HealthBar healthBar;
 
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


    public void Initialize(Transform _startPos, string _name)
    {
        //anim = GetComponent<Animator>();

        switch(myType)
        {
            case EnemyType.OneHanded:
                mySpeed = 10;
                myHealth = 100;
                myDamage = 100;
                myScore = 100;
                myPatrolType = PatrolType.Linear;
                break;
            case EnemyType.TwoHanded:
                mySpeed = 5;
                myHealth = 200;
                myDamage = 200;
                myScore = 50;
                myPatrolType = PatrolType.PingPong;
                break;
            case EnemyType.Archer:
                mySpeed = 20;
                myHealth = 50;
                myDamage = 75;
                myScore = 200;
                myPatrolType = PatrolType.Random;
                break;
            default:
                mySpeed = 100;
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

        //StartCoroutine(Move());
    }

    private IEnumerator Move()
    {
        switch(myPatrolType)
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

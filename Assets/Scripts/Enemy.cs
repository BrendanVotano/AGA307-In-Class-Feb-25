using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    public EnemyType myType;
    public PatrolType myPatrolType;
    public float moveDistance = 1000f;
    public float stoppingDistance = 0.3f;
    
    private float mySpeed;
    private int myHealth;
    private int myDamage;
    private Transform moveToPos;    //Needed for all movement
    private Transform startPos;     //Needed for PingPong movement
    private Transform endPos;       //Needed for PingPong movement
    private bool reverse;           //Needed for PingPong movement
    private int patrolPoint;        //Needed for Linear movement;
    private EnemyManager _EM;

    public void Initialize(EnemyManager _em, Transform _startPos)
    {
        _EM = _em;

        switch(myType)
        {
            case EnemyType.OneHanded:
                mySpeed = 10;
                myHealth = 100;
                myDamage = 100;
                myPatrolType = PatrolType.Linear;
                break;
            case EnemyType.TwoHanded:
                mySpeed = 5;
                myHealth = 200;
                myDamage = 200;
                myPatrolType = PatrolType.PingPong;
                break;
            case EnemyType.Archer:
                mySpeed = 20;
                myHealth = 50;
                myDamage = 75;
                myPatrolType = PatrolType.Random;
                break;
            default:
                mySpeed = 100;
                myHealth = 100;
                myDamage = 10;
                break;
        }

        startPos = _startPos;
        endPos = _EM.GetRandomSpawnPoint;
        moveToPos = endPos;

        StartCoroutine(Move());
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

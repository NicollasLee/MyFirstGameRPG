using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SlimeA : MonoBehaviour
{
    private GameManager _GameManager;
    private Animator anim;

    private bool isDie;

    public ParticleSystem hitEffect;
    public int HP;
    public enemyState state;

    public const float idleWaitTime = 5f;

    //IA
    private NavMeshAgent agent;
    private int idWayPoint;
    private Vector3 destination;

    // Start is called before the first frame update
    void Start()
    {
        _GameManager = FindObjectOfType(typeof(GameManager)) as GameManager;

        anim = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();

        ChangeState(state);
    }

    // Update is called once per frame
    void Update()
    {
        StateManager();
    }


    IEnumerator Died()
    {
        isDie = true;
        yield return new WaitForSeconds(2.5f);
        Destroy(this.gameObject);
    }

    #region MEUS METODOS

    void GetHit(int amount)
    {
        if (isDie)
        {
            return;
        }

        HP -= amount;

        if (HP > 0)
        {
            anim.SetTrigger("GetHit");
            hitEffect.Emit(10);
        }
        else
        {
            anim.SetTrigger("Die");
            StartCoroutine("Died");
        }

    }

    void StateManager()
    {
        switch (state)
        {
            case enemyState.IDLE:
                break;
            case enemyState.ALERT:
                break;
            case enemyState.EXPLORE:
                break;
            case enemyState.PATROL:
                break;
            case enemyState.FOLLOW:
                break;
            case enemyState.FURY:
                break;
            default:
                break;
        }
    }

    void ChangeState(enemyState newState)
    {
        StopAllCoroutines();
        state = newState;

        print(newState);
        switch (state)
        {
            case enemyState.IDLE:
                destination = transform.position;
                agent.destination = destination;

                StartCoroutine("IDLE");

                break;
            case enemyState.ALERT:

                break;
            //case enemyState.EXPLORE:
            //    break;
            case enemyState.PATROL:

                idWayPoint = Random.Range(0, _GameManager.slimeWayPoints.Length);
                destination = _GameManager.slimeWayPoints[idWayPoint].position;
                agent.destination = destination;


                StartCoroutine("PATROL");
                break;
                //case enemyState.FOLLOW:
                //    break;
                //case enemyState.FURY:
                //    break;
                //default:
                //    break;
        }
    }

    IEnumerator IDLE()
    {
        yield return new WaitForSeconds(idleWaitTime);
        StayStill(50); //50% de chance de ficar parado ou não
    }

    IEnumerable PATROL()
    {
        yield return new WaitUntil(() => agent.remainingDistance <= 0);
        

        StayStill(30); // 70% de chance de ficar em patrulha
    }

    void StayStill(int yes)
    {
        if (Rand() <= yes)
        {
            ChangeState(enemyState.IDLE);
        }
        else
        {
            ChangeState(enemyState.PATROL);
        }
    }


    //Sorteio randomico de 50%
    int Rand()
    {
        int rand = Random.Range(0, 100);
        return rand;
    }
    #endregion
}

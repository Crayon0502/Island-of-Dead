using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private Slider HpBar;

    private float enemyMaxHP = 10;
    public float enemyCurruntHP = 0;

    private NavMeshAgent agent;
    private Animator animator;

    private GameObject targetPlayer;
    private float targetDealay = 0.5f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        targetPlayer = GameObject.FindWithTag("Player");
        InitEnemyHP();
    }

    void Update() 
    {
        HpBar.value = enemyCurruntHP / enemyMaxHP;

        if(enemyCurruntHP <= 0)
        {
            StartCoroutine(EnemyDie());
            return;
        }

        if (targetPlayer != null)
        {
            float maxDealay = 0.5f;
            targetDealay += Time.deltaTime;

            if (targetDealay < maxDealay)
                return;

            agent.destination = targetPlayer.transform.position;
            transform.LookAt(targetPlayer.transform.position);

            bool isRange = Vector3.Distance(transform.position, targetPlayer.transform.position) <= agent.stoppingDistance;
            if (isRange)
            {
                animator.SetTrigger("Attack");
            }
            else
            {
                animator.SetFloat("MoveSpeed", agent.velocity.magnitude);

            }

            targetDealay = 0;
        }

    }

    private void InitEnemyHP()
    {
        enemyCurruntHP = enemyMaxHP;
    }

    IEnumerator EnemyDie()
    {
        agent.speed = 0;
        animator.SetTrigger("Dead");

        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }
}
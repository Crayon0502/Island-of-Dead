using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private Slider HpBar;
    public float zombieAtkDamage = 7f;

    [SerializeField]
    private SphereCollider meleeArea;

    [SerializeField]
    private float enemyMaxHP = 10;
    public float enemyCurruntHP = 0;

    [SerializeField]
    private AudioClip[] zombieAtkSound;
    private AudioSource atkSound;

    [SerializeField]
    private float trackingRange = 10f;

    [SerializeField]
    private float wanderingTime = 2f; // �����ϰ� �ȴ� �ð�
    private float currentWanderingTime = 0f;

    public GameObject hitEffectPrefab;

    private NavMeshAgent agent;
    private Animator animator;

    private GameObject targetPlayer;
    private float targetDealay = 0.5f;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        atkSound = GetComponent<AudioSource>();
        targetPlayer = GameObject.FindWithTag("Player");
        InitEnemyHP();
    }

    void Update()
    {
        HpBar.value = enemyCurruntHP / enemyMaxHP;

        if (enemyCurruntHP <= 0)
        {
            StartCoroutine(EnemyDie());
            return;
        }

        if (targetPlayer != null && Vector3.Distance(transform.position, targetPlayer.transform.position) <= trackingRange)
        {
            Chase(); // �÷��̾� ����
        }
        else
        {
            Wander(); // ��ȸ
        }

    }

    private void Chase()
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

    private void Wander()
    {
        currentWanderingTime += Time.deltaTime;

        if (currentWanderingTime >= wanderingTime)
        {
            // �����ϰ� ���� ����
            Vector3 randomDirection = Random.insideUnitSphere * 10f;
            randomDirection += transform.position;
            NavMeshHit hit;
            NavMesh.SamplePosition(randomDirection, out hit, 10f, NavMesh.AllAreas);
            Vector3 finalPosition = hit.position;

            // �̵� ��ǥ ����
            agent.destination = finalPosition;

            // �ִϸ��̼� �� Ÿ�̸� �ʱ�ȭ
            animator.SetFloat("MoveSpeed", agent.velocity.magnitude);
            currentWanderingTime = 0f;
        }
        else
        {
            // �̵� ���� ��� �ִϸ��̼� ���
            animator.SetFloat("MoveSpeed", agent.velocity.magnitude);
        }
    }

    private void InitEnemyHP()
    {
        enemyCurruntHP = enemyMaxHP;
    }

    public void ZombieAttack(int zombieAttack)
    {
        if (zombieAttack == 1)
        {
            meleeArea.enabled = true;
            ZombieAtkSound();
        }
        else
        {
            meleeArea.enabled = false;
        }
    }

    private void ZombieAtkSound()
    {
        atkSound.clip = zombieAtkSound[Random.Range(0, 3)];
        atkSound.Play();
    }

    IEnumerator EnemyDie()
    {
        agent.speed = 0;
        animator.SetTrigger("Dead");

        yield return new WaitForSeconds(3.5f);
        Destroy(gameObject);
    }

    public void PlayHitEffect(Vector3 hitPoint)
    {
        if (hitEffectPrefab != null)
        {
            Instantiate(hitEffectPrefab, hitPoint, Quaternion.identity);
        }
    }
}

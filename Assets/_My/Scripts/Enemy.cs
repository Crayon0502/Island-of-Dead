using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private Slider HpBar;
    public float zombieAtkDamage = 9f;

    [SerializeField]
    private SphereCollider meleeArea;

    [SerializeField]
    private float enemyMaxHP = 10;
    public float enemyCurruntHP = 0;

    [SerializeField]
    private AudioClip[] zombieAtkSound;
    private AudioSource atkSound;

    [SerializeField]
    private AudioClip[] zombieHitSound;
    private AudioSource hitSound;
    private AudioSource deadSound;

    [SerializeField]
    private float trackingRange = 10f;

    [SerializeField]
    private float wanderingTime = 2f;
    private float currentWanderingTime = 0f;

    public GameObject hitEffectPrefab;

    private NavMeshAgent agent;
    private Animator animator;
    private bool hasDied = false;

    private GameObject targetPlayer;
    private float targetDealay = 0.5f;
    private CapsuleCollider enemyCollider;

    private GameManger gm;
    private QManager qm;
    void Start()
    {
        qm = FindObjectOfType<QManager>();
        gm = FindObjectOfType<GameManger>();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        atkSound = GetComponent<AudioSource>();
        deadSound = GetComponent<AudioSource>();
        hitSound = GetComponent<AudioSource>();
        enemyCollider = GetComponent<CapsuleCollider>();
        targetPlayer = GameObject.FindWithTag("Player");
        InitEnemyHP();
    }

    void Update()
    {
        HpBar.value = enemyCurruntHP / enemyMaxHP;

        if (enemyCurruntHP <= 0 && !hasDied)
        {
            StartCoroutine(EnemyDie());
            return;
        }

        if (!hasDied) // 사망한 경우에는 더 이상 업데이트를 수행하지 않도록
        {
            if (targetPlayer != null && Vector3.Distance(transform.position, targetPlayer.transform.position) <= trackingRange)
            {
                Chase(); // 플레이어 추적
            }
            else
            {
                Wander(); // 배회
            }
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
            // 랜덤하게 방향 설정
            Vector3 randomDirection = Random.insideUnitSphere * 10f;
            randomDirection += transform.position;
            NavMeshHit hit;
            NavMesh.SamplePosition(randomDirection, out hit, 10f, NavMesh.AllAreas);
            Vector3 finalPosition = hit.position;

            // 이동 목표 설정
            agent.destination = finalPosition;

            // 애니메이션 및 타이머 초기화
            animator.SetFloat("MoveSpeed", agent.velocity.magnitude);
            currentWanderingTime = 0f;
        }
        else
        {
            // 이동 중인 경우 애니메이션 재생
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

        hasDied = true;

        agent.speed = 0;
        animator.SetTrigger("Dead");
        enemyCollider.enabled = false;
        EnemyCountDecrease();

        yield return new WaitForSeconds(3.5f);

        gameObject.SetActive(false);
        InitEnemyHP();
        agent.speed = 2f;
        enemyCollider.enabled = true;
        hasDied = false;
    }

    private void EnemyCountDecrease()
    {
        gm.spawnedEnemies -= 1;
        qm.zombieCount += 1;
    }

    public void PlayHitEffect(Vector3 hitPoint)
    {
        if (hitEffectPrefab != null)
        {
            Instantiate(hitEffectPrefab, hitPoint, Quaternion.identity);
        }
    }

    private void HitSound()
    {
        hitSound.clip = zombieHitSound[Random.Range(0, 3)];
        hitSound.Play();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("HandAtk"))
        {
            enemyCurruntHP -= 0.7f;
            HitSound();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Spawner : MonoBehaviour
{
    private GameManger gm;
    private QManager qm;


    public GameObject hitEffectPrefab;
    public GameObject explosion;
    public GameObject fire;

    [SerializeField]
    private Slider spawnerHpBar;

    [SerializeField]
    private float spawnerMaxHP = 20;
    public float spawnerCurruntHP = 20;

    [SerializeField]
    private AudioClip[] spawnerHitSound;
    private AudioSource hitSound;

    [SerializeField]
    private AudioClip spawnerBoomSound;
    private AudioSource boomSound;

    private bool hasDied = false;

    private void Start()
    {
        explosion.SetActive(false);
        fire.SetActive(false);

        qm = FindObjectOfType<QManager>();
        gm = FindObjectOfType<GameManger>();
        hitSound = GetComponent<AudioSource>();
        boomSound = GetComponent<AudioSource>();
    }

    private void Update()
    {
       spawnerHpBar.value = spawnerCurruntHP / spawnerMaxHP;

        if(spawnerCurruntHP <= 0 && !hasDied)
        {
            StartCoroutine(SpawnerDie());
        }
    }

    private IEnumerator SpawnerDie()
    {
        hasDied = true;  // �÷��׸� true�� �����Ͽ� ü���� 0�� �Ǿ����� ��Ÿ���ϴ�.

        gm.spawnerCount -= 1;
        qm.spawnerCount += 1;

        explosion.SetActive(true);
        BoomSound();
        fire.SetActive(true);

        yield return null;  // ���� �����ӱ��� ���

       
        yield return new WaitForSeconds(0.1f);
    }

    public void HitSound()
    {
        hitSound.clip = spawnerHitSound[Random.Range(0, spawnerHitSound.Length)];
        hitSound.Play();
    }

    private void BoomSound()
    {
        boomSound.clip = spawnerBoomSound;
        boomSound.Play();
    }

    public void PlayHitEffect(Vector3 hitPoint)
    {
        if (hitEffectPrefab != null)
        {
            Instantiate(hitEffectPrefab, hitPoint, Quaternion.identity);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("HandAtk"))
        {
            spawnerCurruntHP -= 0.3f;
            HitSound();
        }
    }
}
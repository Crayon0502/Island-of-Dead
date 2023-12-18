using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Spawner : MonoBehaviour
{
    private GameManger gm;
    private QManager qm;


    [SerializeField]
    private Slider spawnerHpBar;

    [SerializeField]
    private float spawnerMaxHP = 20;
    public float spawnerCurruntHP = 20;

    //[SerializeField]
    //private AudioClip[] spawnerHitSound;
    //private AudioSource hitSound;

    private bool hasDied = false;

    private void Start()
    {
        qm = FindObjectOfType<QManager>();
        gm = FindObjectOfType<GameManger>();
        //hitSound = GetComponent<AudioSource>();
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
        yield return null;  // ���� �����ӱ��� ���

       
        yield return new WaitForSeconds(0.1f);
    }

    //private void HandHitSound()
    //{
    //    hitSound.clip = spawnerHitSound[Random.Range(0, spawnerHitSound.Length)];
    //    hitSound.Play();
    //}

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("HandAtk"))
        {
            spawnerCurruntHP -= 0.5f;
            //HandHitSound();
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManger : MonoBehaviour
{
    public static GameManger instance;

    [Header("Bullet")]
    [SerializeField]
    private Transform bulletPoint;
    [SerializeField]
    private GameObject bulletObj;
    [SerializeField]
    private float maxShootDelay = 0.2f;
    [SerializeField]
    private float curruntShootDelay = 0.2f;
    [SerializeField]
    private Text bulletText;
    private int maxBullet = 30;
    private int curruntBullet = 0;

    [Header("Weapon FX")]
    [SerializeField]
    private GameObject weaponFlashFX;
    [SerializeField]
    private Transform bulletCasePoint;
    [SerializeField]
    private GameObject bulletCaseFX;
    [SerializeField]
    private Transform weaponClipPoint;
    [SerializeField]
    private GameObject weaponClipFX;

    [Header("Enemy")]
    [SerializeField]
    private GameObject[] spawnPoint;
    [SerializeField]
    private int spawnedEnemies = 0;
    [SerializeField]
    private int maxEnemies = 50;

    [Header("UI")]
    public Image bloodScreen;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;

        curruntShootDelay = 0;

        InitBullet();

        CountExistingZombies();
        StartCoroutine(EnemySpawn());
    }

    // Update is called once per frame
    void Update()
    {
        bulletText.text = curruntBullet + " / " + maxBullet;
    }

    public void Shooting(Vector3 targetPosition, Enemy enemy, AudioSource weaponSound, AudioClip shootingSound)
    {
        curruntShootDelay += Time.deltaTime;

        if (curruntShootDelay < maxShootDelay || curruntBullet <= 0)
            return;

        curruntBullet -= 1;
        curruntShootDelay = 0;

        weaponSound.clip = shootingSound;
        weaponSound.Play();

        Vector3 aim = (targetPosition - bulletPoint.position).normalized;

        GameObject flashFX = PoolManager.instance.ActivateObj(1);
        SetObjPosition(flashFX, bulletPoint);
        flashFX.transform.rotation = Quaternion.LookRotation(aim, Vector3.up);

        GameObject caseFX = PoolManager.instance.ActivateObj(2);
        SetObjPosition(caseFX, bulletCasePoint);

        GameObject prefabToSpawn = PoolManager.instance.ActivateObj(0);
        SetObjPosition(prefabToSpawn, bulletPoint);
        prefabToSpawn.transform.rotation = Quaternion.LookRotation(aim, Vector3.up);
    }

    public void ReloadClip()
    {
        GameObject clipFX = PoolManager.instance.ActivateObj(3);
        SetObjPosition(clipFX, weaponClipPoint);
        InitBullet();
    }

    private void InitBullet()
    {
        curruntBullet = maxBullet;
    }

    private void SetObjPosition(GameObject obj, Transform targetTransform)
    {
        obj.transform.position = targetTransform.position;
    }

    public IEnumerator ShowBloodScreen()
    {
        bloodScreen.color = new Color(0.255f, 0, 0, UnityEngine.Random.Range(0.9f, 1f));
        yield return new WaitForSeconds(0.15f);
        bloodScreen.color = Color.clear;
    }

    private void CountExistingZombies()
    {
        GameObject[] existingZombies = GameObject.FindGameObjectsWithTag("Enemy"); // 태그는 적절히 변경
        spawnedEnemies += existingZombies.Length;
    }

    IEnumerator EnemySpawn()
    {
        while (spawnedEnemies < maxEnemies)
        {
            GameObject enemy = PoolManager.instance.ActivateObj(Random.Range(4, 6));
            SetObjPosition(enemy, spawnPoint[Random.Range(0, spawnPoint.Length)].transform);
            spawnedEnemies++;

            yield return new WaitForSeconds(1.5f);
        }
    }
}
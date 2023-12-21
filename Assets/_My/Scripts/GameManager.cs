using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
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
    public int maxBullet = 0;
    public int curruntBullet = 0;

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
    public int spawnedEnemies = 0;
    [SerializeField]
    private int maxEnemies = 50;

    [Header("UI")]
    [SerializeField]
    private GameObject canvas;
    public Image bloodScreen;
    [SerializeField]
    private GameObject supportUI;
    private bool spUIOn = true;

    [Header("Spawner")]
    public int spawnerCount = 3;

    [Header("BGM")]
    [SerializeField]
    private AudioClip bgmSound;
    private AudioSource bgmSource;

    private PlayerManager pm;
    private PlayableDirector cut;
    public bool isReady = true;

    private Inventory inventory;
    public int tempCount;
    private bool isSpawning = false;

    // Start is called before the first frame update
    void Start()
    {
        canvas.SetActive(false);

        cut = GetComponent<PlayableDirector>();
        cut.Play();

        instance = this;

        pm = FindObjectOfType<PlayerManager>();
        inventory = FindObjectOfType<Inventory>();

        curruntShootDelay = 0;

        CountExistingZombies();
    }

    // Update is called once per frame
    void Update()
    {
        bulletText.text = curruntBullet + " / " + maxBullet;
        StartCoroutine(EnemySpawn());

        if(Input.GetKeyDown(KeyCode.C))
        {
            SupportUIOnOff();
        }
    }

    private void SupportUIOnOff()
    {
        spUIOn = !spUIOn;

        if (spUIOn)
            supportUI.SetActive(true);
        else 
            supportUI.SetActive(false);
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

        int plusBullet = Mathf.Min(30 - curruntBullet, maxBullet);
        curruntBullet += plusBullet;
        maxBullet -= plusBullet;
    }

    private void SetObjPosition(GameObject obj, Transform targetTransform)
    {
        obj.transform.position = targetTransform.position;
    }

    public IEnumerator ShowBloodScreen()
    {
        bloodScreen.color = new Color(0.255f, 0, 0, Random.Range(0.9f, 1f));
        yield return new WaitForSeconds(0.15f);
        bloodScreen.color = Color.clear;
    }

    private void CountExistingZombies()
    {
        GameObject[] existingZombies = GameObject.FindGameObjectsWithTag("Enemy");
        spawnedEnemies += existingZombies.Length;
    }

    IEnumerator EnemySpawn()
    {
        while (spawnedEnemies < maxEnemies && spawnerCount != 0)
        {

            GameObject enemy = PoolManager.instance.ActivateObj(Random.Range(4, 6));
            SetObjPosition(enemy, spawnPoint[Random.Range(0, spawnPoint.Length)].transform);
            spawnedEnemies++;

            yield return new WaitForSeconds(1.5f);
        }
    }

    private void PlayBGMSound()
    {
        bgmSource = GetComponent<AudioSource>();
        bgmSource.clip = bgmSound;
        bgmSource.loop = true;
        bgmSource.Play();
    }

    public void StartGame()
    {
        isReady = false;
        PlayBGMSound();
        canvas.SetActive(true);
    }
}
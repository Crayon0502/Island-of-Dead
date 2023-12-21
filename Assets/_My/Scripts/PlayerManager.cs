using Cinemachine;
using System.Collections;
using StarterAssets;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.UI;
using UnityEngine.Playables;

public class PlayerManager : MonoBehaviour
{
    private StarterAssetsInputs input;
    private ThirdPersonController controller;
    private Animator anim;
    public MultiAimConstraint multiAimConstraint;

    [Header("PlayerState")]
    [SerializeField]
    private Slider hpBar;
    [SerializeField]
    private Slider infectionBar;
    [SerializeField]
    private float playerMaxHP = 100;
    public float playerCurrentHP = 0;
    private float playerMaxInfection = 100;
    public float playerCurrentInfection = 0;
    private bool isGun = false;
    public bool isDie = false;
    public bool infection = false;



    
    [Header("Aim")]
    [SerializeField]
    private CinemachineVirtualCamera aimCam;
    [SerializeField]
    private GameObject aimImage;
    [SerializeField]
    private GameObject aimObj;
    [SerializeField]
    private float aimObjDis = 10f;
    [SerializeField]
    private LayerMask targetLayer;

    [Header("IK")]
    [SerializeField]
    private Rig aimRig;

    [Header("Weapon")]
    [SerializeField]
    private GameObject gunHand;
    [SerializeField]
    private GameObject gunBack;
    [SerializeField]
    private SphereCollider handMeleeArea_L;
    [SerializeField]
    private SphereCollider handMeleeArea_R;

    [Header("Weapon Sound Effect")]
    [SerializeField]
    private AudioClip shootingSound;
    [SerializeField]
    private AudioClip[] reloadSound;
    private AudioSource weaponSound;
    [SerializeField]
    private AudioClip[] handAtkSoundC;
    private AudioSource handAtkSoundS;

    [Header("UI")]
    [SerializeField]
    private GameObject dieBase;
    [SerializeField]
    private Text playerDieText;
    [SerializeField]
    private GameObject diePanel;
    [SerializeField]
    private Image dieBloodScreen;
    [SerializeField]
    private GameObject restartButton;

    [SerializeField]
    private GameObject endBase;
    [SerializeField]
    private Image endImage;
    [SerializeField]
    private Text endText;
    [SerializeField]
    private GameObject endRestartButton;
    [SerializeField]
    private GameObject endFireFX;
    [SerializeField]
    private GameObject inGameUI;
    [SerializeField]
    private GameObject endVC;

    private GameManger gameManager;
    private Enemy enemy;
    private float infectionSpeed = 1f;
    private bool waterInfection = false;

    private PlayableDirector cut;
    public bool isEnd = false;

    void Start()
    {
        endVC.SetActive(false);

        cut = GetComponent<PlayableDirector>();
        input = GetComponent<StarterAssetsInputs>();
        controller = GetComponent<ThirdPersonController>();
        anim = GetComponent<Animator>();
        weaponSound = GetComponent<AudioSource>();
        handAtkSoundS = GetComponent<AudioSource>();
        multiAimConstraint = GetComponent<MultiAimConstraint>();
        gameManager = FindObjectOfType<GameManger>();

        InitPlayerHP();
        isGun = false;
    }

    private void Update()
    {
        if(GameManger.instance.isReady)
        {
            AimControll(false);
            SetRigWeight(0);
            return;
        }

        if (!isDie)
        {
            if (!Inventory.inventoruActivated)
            {
                Swap();
                AimCheck();
                InfectionIncrease();
            }
        }

        if (playerCurrentHP > 100)
            playerCurrentHP = 100;
        if (playerCurrentInfection < 0)
            playerCurrentInfection = 0;

        if (playerCurrentHP <= 0 || playerCurrentInfection >= 100)
        {
            StartCoroutine(PlayerDied());
        }

        hpBar.value = playerCurrentHP / playerMaxHP;
        infectionBar.value = playerCurrentInfection / playerMaxInfection;
    }

    public void End()
    {
        inGameUI.SetActive(false);
        endVC.SetActive(true);
        isEnd = true;
        endFireFX.SetActive(true);
        this.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
        cut.Play();
    }

    public void EndScreen()
    {
        StartCoroutine(EndGame());
    }

     IEnumerator EndGame()
    {
        endBase.SetActive(true);

        // 블러드 스크린을 서서히 나타나게 함
        while (endImage.color.a < 1.0f)
        {
            endImage.color += new Color(0.85f, 0.85f, 0.85f, Time.deltaTime * 0.5f);
            yield return null;
        }

        while (endText.color.a < 1.0f)
        {
            endText.color += new Color(0.69f, 0.69f, 0.69f, Time.deltaTime * 0.5f);
            yield return null;
        }

        yield return new WaitForSeconds(1f);

        endRestartButton.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    IEnumerator PlayerDied()
    {
        dieBase.SetActive(true);
        isDie = true;
        // 판넬을 서서히 나타나게 함
        while (diePanel.GetComponent<Image>().color.a < 1.0f)
        {
            Color panelColor = diePanel.GetComponent<Image>().color;
            panelColor += new Color(0, 0, 0, Time.deltaTime * 0.02f);
            diePanel.GetComponent<Image>().color = panelColor;
            yield return null;
        }

        // 블러드 스크린을 서서히 나타나게 함
        while (dieBloodScreen.color.a < 1.0f)
        {
            dieBloodScreen.color += new Color(0, 0, 0, Time.deltaTime * 0.01f);
            yield return null;
        }

        while (playerDieText.color.a < 1.0f)
        {
            playerDieText.color += new Color(0f, 0, 0, Time.deltaTime * 0.01f); 
            yield return null;
        }



        yield return new WaitForSeconds(2f);

        restartButton.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void InitPlayerHP()
    {
        playerCurrentHP = playerMaxHP;
    }

    private void Swap()
    {
        if(input.gun)
            isGun = true;  
        if(input.hand)
            isGun = false;


        if (isGun)
        {
            anim.SetBool("isGun", true);
            input.gun = false;
            gunHand.SetActive(true);
            gunBack.SetActive(false);
        }
        else
        {
            anim.SetBool("isGun", false);
            input.hand = false;
            gunHand.SetActive(false);
            gunBack.SetActive(true);
        }
    }

    private void AimCheck()
    {
        if (input.reload && isGun && gameManager.maxBullet > 0 && gameManager.curruntBullet != 30)
        {
            input.reload = false;

            if (controller.isReload)
                return;

            SetRigWeight(0);
            AimControll(false);
            anim.SetLayerWeight(1, 1);
            anim.SetTrigger("Reload");
            controller.isReload = true;
        }

        if (controller.isReload)
            return;

        if (isGun)
        {
            if (input.aim)
            {
                AimControll(true);

                SetRigWeight(1);
                anim.SetLayerWeight(1, 1);

                Vector3 targetPosition = Vector3.zero;
                Transform camTransform = Camera.main.transform;
                RaycastHit hit;

                if (Physics.Raycast(camTransform.position, camTransform.forward, out hit, Mathf.Infinity, targetLayer))
                {
                    targetPosition = hit.point;
                    aimObj.transform.position = hit.point;

                    enemy = hit.collider.gameObject.GetComponent<Enemy>();
                    //float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
                }
                else
                {
                    targetPosition = camTransform.position + camTransform.forward * aimObjDis;
                    aimObj.transform.position = camTransform.position + camTransform.forward * aimObjDis;
                }

                Vector3 targetAim = targetPosition;
                targetAim.y = transform.position.y;
                Vector3 aimDir = (targetAim - transform.position).normalized;
                transform.forward = Vector3.Lerp(transform.forward, aimDir, Time.deltaTime * 50f);

                if (input.shoot)
                {
                    anim.SetBool("Shoot", true);
                    GameManger.instance.Shooting(targetPosition, enemy, weaponSound, shootingSound);
                }
                else
                {
                    anim.SetBool("Shoot", false);
                }
            }
            else
            {
                AimControll(false);
                SetRigWeight(0);
                anim.SetLayerWeight(1, 0);
                anim.SetBool("Shoot", false);
            }
        }
        else
        {
            SetRigWeight(0);

            if (input.punching)
            {
                anim.SetTrigger("Punching");
                input.punching = false;
                controller.isPunching = true;
            }
            else
            {

            }
        }
    }

    private void AimControll(bool isCheck)
    {
        aimCam.gameObject.SetActive(isCheck);
        aimImage.SetActive(isCheck);
        controller.isAimMove = isCheck;
    }

    public void Reload()
    {
        controller.isReload = false;
        SetRigWeight(0);
        anim.SetLayerWeight(1, 0);
    }

    private void SetRigWeight(float weight)
    {
        aimRig.weight = weight;
    }

    public void ReloadWeaponClip()
    {
        GameManger.instance.ReloadClip();
        PlayWeaponSound(reloadSound[0]);
    }

    public void ReloadInsertClip(int num)
    {
        PlayWeaponSound(reloadSound[num]);
    }

    private void PlayWeaponSound(AudioClip sound)
    {
        weaponSound.clip = sound;
        weaponSound.Play();
    }

    private void InfectionIncrease()
    {
        if (infection)
            playerCurrentInfection += Time.deltaTime * 1f;

        if (waterInfection)
            playerCurrentInfection += Time.deltaTime * infectionSpeed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("EnemyAtk"))
        {
            playerCurrentHP -= other.gameObject.GetComponentInParent<Enemy>().zombieAtkDamage;
            StartCoroutine(gameManager.ShowBloodScreen());
            playerCurrentInfection += 2;
            infection = true;
        }

        if (other.CompareTag("Water"))
        {
            waterInfection = true;
            infectionSpeed = 4f;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Water"))
        {
            waterInfection = false;
        }
    }

    public void PunchingAnimationEnd()
    {
        controller.isPunching = false;
    }

    public void HandAttack(int handAttack)
    {
        if (handAttack == 1)
        {
            handMeleeArea_L.enabled = true;
            HandAtkSound(0);
        }
        else if (handAttack == 2)
        {
            handMeleeArea_R.enabled = true;
            HandAtkSound(1);
        }
        else
        {
            handMeleeArea_L.enabled = false;
            handMeleeArea_R.enabled = false;
        }
    }

    private void HandAtkSound (int soundNum)
    {
        handAtkSoundS.clip = handAtkSoundC[soundNum];
        handAtkSoundS.Play();
    }
}
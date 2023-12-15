using Cinemachine;
using StarterAssets;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.UI;

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
    private Slider pollutionBar;
    [SerializeField]
    private float playerMaxHP = 100;
    public float playerCurrentHP = 0;
    private float playerMaxInfection = 100;
    public float playerCurrentInfection = 0;
    private bool isGun = false;

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

    private GameManger gameManager;
    private Enemy enemy;
    private float infectionSpeed = 1f;
    private bool infection = false;
    private bool waterInfection = false;

    void Start()
    {
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
        if(!Inventory.inventoruActivated)
        {
            Swap();
            AimCheck();
            InfectionIncrease();
        }
        
        hpBar.value = playerCurrentHP / playerMaxHP;
        pollutionBar.value = playerCurrentInfection / playerMaxInfection;
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
        if (input.reload && isGun)
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
            playerCurrentInfection += Time.deltaTime * 0.5f;

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
            infectionSpeed = 0.6f;
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
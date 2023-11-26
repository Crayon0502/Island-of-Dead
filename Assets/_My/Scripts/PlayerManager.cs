using Cinemachine;
using StarterAssets;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private StarterAssetsInputs input;
    private ThirdPersonController controller;
    private Animator anim;

    [Header("Aim")]
    [SerializeField]
    private CinemachineVirtualCamera aimCam;
    [SerializeField]
    private GameObject aimImage;
    //[SerializeField]
    //private GameObject aimObj;
    [SerializeField]
    private float aimObjDis = 10f;
    [SerializeField]
    private LayerMask targetLayer;

    private void Start()
    {
        input = GetComponent<StarterAssetsInputs>();
        controller = GetComponent<ThirdPersonController>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        AimCheck();
    }

    private void AimCheck()
    {
        if(input.reload)
        {
            input.reload = false;


            if (controller.isReload)
                return;

            AimControll(false);
            anim.SetLayerWeight(1, 1);
            anim.SetTrigger("Reload");
            controller.isReload = true;
        }

        if (controller.isReload)
            return;

        if (input.aim)
        {
            AimControll(true);

            anim.SetLayerWeight(1, 1);

            Vector3 targetPosition = Vector3.zero;
            Transform camTransform = Camera.main.transform;
            RaycastHit hit;

            if (Physics.Raycast(camTransform.position, camTransform.forward, out hit, Mathf.Infinity, targetLayer))
            {
                targetPosition = hit.point;
                //aimObj.transform.position = hit.point;
            }
            else
            {
                targetPosition = camTransform.position + camTransform.forward * aimObjDis;
                //aimObj.transform.position = camTransform.position + camTransform.forward * aimObjDis;
            }

            Vector3 targetAim = targetPosition;
            targetAim.y = transform.position.y;
            Vector3 aimDir = (targetAim - transform.position).normalized;
            transform.forward = Vector3.Lerp(transform.forward, new Vector3(aimDir.x, transform.forward.y, aimDir.z), Time.deltaTime * 180f);

            if(input.shoot)
            {
                anim.SetBool("Shoot", true);
            }
            else
            {
                anim.SetBool("Shoot", false);
            }
        }
        else
        {
            AimControll(false);

            anim.SetLayerWeight(1, 0);
            anim.SetBool("Shoot", false);
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
        Debug.Log("dddd");
        controller.isReload = false;
        anim.SetLayerWeight(1, 0);
    }
}

using Cinemachine;
using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{

    private StarterAssetsInputs input;

    [Header("Aim")]
    [SerializeField]
    private CinemachineVirtualCamera aimCam;


    void Start()
    {
        input = GetComponent<StarterAssetsInputs>();
    }

    void Update()
    {
        if(input.aim)
        {
            aimCam.gameObject.SetActive(true);
        }
        else
        {
            aimCam.gameObject.SetActive(false);
        }
    }
}

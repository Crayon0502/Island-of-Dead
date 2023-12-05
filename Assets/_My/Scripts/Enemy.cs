using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private Slider HpBar;

    private float enemyMaxHP = 10;
    public float enemyCurruntHP = 0;

    void Start()
    {
        InitEnemyHP();
    }

    void Update()
    {
        HpBar.value = enemyCurruntHP / enemyMaxHP;
    }

    private void InitEnemyHP()
    {
        enemyCurruntHP = enemyMaxHP;
    }
}
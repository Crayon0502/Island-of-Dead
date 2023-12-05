using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    private int enemyMaxHP = 10;
    public int enemyCurruntHP = 0;

    void Start()
    {
        InitEnemyHP();
    }

    void Update()
    {

    }

    private void InitEnemyHP()
    {
        enemyCurruntHP = enemyMaxHP;
    }
}

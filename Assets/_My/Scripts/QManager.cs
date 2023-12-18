using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QManager : MonoBehaviour
{
    public bool talkStart = false;
    private int talkIndexs = 0;

    public GameObject talkBase;
    public Text talkText;
    public string[] talk;

    public GameObject QuestBase;
    public Text q1;
    public Text q2;
    public Text q3;

    void Start()
    {
        talkBase.SetActive(false);
        QuestBase.SetActive(false);
    }

    void Update()
    {
        if (talkStart)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                if (talkIndexs < talk.Length - 1)
                {
                    talkIndexs++;
                    TalkTextChange(talkIndexs);
                }
                else
                {
                    talkBase.SetActive(false);
                    talkStart = false;
                }
            }
        }
    }

    public void TalkStart()
    {
        talkIndexs = 0;
        talkBase.SetActive(true);
        TalkTextChange(talkIndexs);
        talkStart = true; // 대화 시작 상태로 변경
    }

    public void TalkTextChange(int talkIndex)
    {
        talkText.text = talk[talkIndex];
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QManager : MonoBehaviour
{
    public int zombieCount = 0;
    public int spawnerCount = 0;

    public bool talkStart = false;
    private bool isQuestStart = false;
    private int talkIndexs = 0;

    public GameObject talkBase;
    public Text talkText;
    public string[] talk;

    public GameObject QuestBase;
    public Text q1;
    public Text q2;
    public Text q3;

    private ActionController ac;

    void Start()
    {
        ac = FindObjectOfType<ActionController>();
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

                    QuestStart();
                }
            }
        }

        if(isQuestStart)
        {
            q1.text = "진행도 : " + zombieCount + " / 20";
            q2.text = "진행도 : " + spawnerCount + " / 3";
            if(ac.isHasKey)
                q3.text = "진행도 : 1 / 1";
        }

    }

    private void QuestStart()
    {
        isQuestStart = true;
        QuestBase.SetActive(true);
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Reel : MonoBehaviour
{
    /// <summary>
    /// 滾輪的類型
    /// </summary>
    public enum REELTYPE
    {
        NONE = 0,//獎項停著的狀態
        START = 1,//開始旋轉的狀態
        OVER = 2,//結束盤面下來
    }

    [SerializeField]
    public REELTYPE Type;

    /// <summary>
    /// 得獎滾輪
    /// </summary>
    [SerializeField]
    public SpriteRenderer[] Awards;

    [SerializeField]
    public Vector3[] pos;

    [SerializeField]
    public float height;

    [SerializeField]
    public bool running;

    private void Awake()
    {
        for (int i = 0; i < Awards.Length; i++)
        {
            Awards[i].transform.localPosition = pos[i];
        }
    }

    public void ChangeAwards(int[] r)
    {
        for (int i = 0; i < Awards.Length; i++)
        {
            Awards[i].sprite = MainManage.Main.Symbol[r[i]];
        }
    }

    public void StartGame_Normal(float time, float overtime, float delaytime)
    {
        running = true;
        Type = REELTYPE.START;
        StartCoroutine(Start_Normal(time, overtime, delaytime));
    }

    IEnumerator Start_Normal(float time, float overtime, float delaytime)
    {
        float value_time = 0;
        float value_overtime = 0;

        Vector3[] s = new Vector3[Awards.Length];
        Vector3[] e = new Vector3[Awards.Length];

        //盤面移到上面
        for (int i = 0; i < Awards.Length; i++)
        {
            s[i] = pos[i] + new Vector3(0, height, 0);
            e[i] = pos[i];
            Awards[i].transform.localPosition = s[i];
        }

        yield return new WaitForSeconds(delaytime);

        while (true)
        {
            switch (Type)
            {
                case REELTYPE.NONE:
                    {
                        running = false;
                        yield break;
                    }
                case REELTYPE.START:
                    {
                        while (value_time <= time)
                        {
                            value_time += Time.deltaTime;
                            yield return null;
                        }
                        Type = REELTYPE.OVER;
                    }
                    break;
                case REELTYPE.OVER:
                    {
                        while (value_overtime <= overtime)
                        {
                            value_overtime += Time.deltaTime;
                            float t = value_overtime / overtime;
                            for (int i = 0; i < Awards.Length; i++)
                            {
                                Awards[i].transform.localPosition = Vector3.Lerp(s[i], e[i], t);
                            }

                            yield return null;
                        }

                        Type = REELTYPE.NONE;
                    }
                    break;
            }
        }
    }

    public void StartGame_Auto(float time, float overtime, float delaytime)
    {
        running = true;
        Type = REELTYPE.START;
        StartCoroutine(Start_Auto(time, overtime, delaytime));
    }

    IEnumerator Start_Auto(float time, float overtime, float delaytime)
    {
        float value_time = 0;
        float value_overtime = 0;

        Vector3[] s = new Vector3[Awards.Length];
        Vector3[] e = new Vector3[Awards.Length];

        //盤面移到上面
        for (int i = 0; i < Awards.Length; i++)
        {
            s[i] = pos[i] + new Vector3(0, height, 0);
            e[i] = pos[i];
            Awards[i].transform.localPosition = s[i];
        }

        yield return new WaitForSeconds(delaytime);

        while (true)
        {
            switch (Type)
            {
                case REELTYPE.NONE:
                    {
                        running = false;
                        yield break;
                    }
                case REELTYPE.START:
                    {
                        while (value_time <= time)
                        {
                            value_time += Time.deltaTime;
                            yield return null;
                        }
                        Type = REELTYPE.OVER;
                    }
                    break;
                case REELTYPE.OVER:
                    {
                        while (value_overtime <= overtime)
                        {
                            value_overtime += Time.deltaTime;
                            float t = value_overtime / overtime;
                            for (int i = 0; i < Awards.Length; i++)
                            {
                                Awards[i].transform.localPosition = Vector3.Lerp(s[i], e[i], t);
                            }

                            yield return null;
                        }

                        Type = REELTYPE.NONE;
                    }
                    break;
            }
        }

    }
}

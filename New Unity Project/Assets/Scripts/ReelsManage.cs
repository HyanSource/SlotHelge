using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ReelsManage : MonoBehaviour
{
    //https://felgo.com/doc/felgo-demos-flaskofrum-example/

    enum GAMETYPE
    {
        NORMAL = 0,
        AUTO = 1,
        NORMALQUICK = 2,
        AUTOQUICK = 3,
    }
    /// <summary>
    /// 滾輪物件
    /// </summary>
    [SerializeField]
    Reel[] AllReel;
    /// <summary>
    /// 是否停止
    /// </summary>
    [SerializeField]
    public bool Stop
    {
        get
        {
            for (int i = 0; i < AllReel.Length; i++)
            {
                if (AllReel[i].Type != Reel.REELTYPE.NONE)
                {
                    return false;
                }
            }
            return true;
        }
    }

    /// <summary>
    /// 開始遊戲
    /// </summary>
    public void StartGame(int[] r, int[] l,int odds)
    {
        GAMETYPE Type = MainManage.Main.Auto ? GAMETYPE.AUTO : GAMETYPE.NORMAL;

        MainManage.Main.Lines_Obj.CloseLine();

        switch (Type)
        {
            case GAMETYPE.NORMAL:
                {
                    for (int i = 0; i < AllReel.Length; i++)
                    {
                        AllReel[i].ChangeAwards(new int[3] { r[i * 3], r[i * 3 + 1], r[i * 3 + 2] });
                        AllReel[i].StartGame_Normal(0, 0.2f, i * 0.05f);
                    }
                }
                break;
            case GAMETYPE.AUTO:
                {
                    for (int i = 0; i < AllReel.Length; i++)
                    {
                        AllReel[i].ChangeAwards(new int[3] { r[i * 3], r[i * 3 + 1], r[i * 3 + 2] });
                        AllReel[i].StartGame_Auto(0, 0.2f, i * 0.05f);
                    }
                }
                break;
            case GAMETYPE.NORMALQUICK:
                {

                }
                break;
            case GAMETYPE.AUTOQUICK:
                {

                }
                break;
        }

        StartCoroutine(OverGame(l,odds));
    }
    /// <summary>
    /// 轉輪停止後的執行方法
    /// </summary>
    IEnumerator OverGame(int[] l, int odds)
    {
        while (!Stop)
        {
            yield return null;
        }

        MainManage.Main.Lines_Obj.OpenLine(l);
        MainManage.Main.TextWinMoney_Obj.text = odds.ToString();

        MainManage.Main.Play_Obj.enabled = true;

        if (MainManage.Main.Auto)
        {
            MainManage.Main.PlayGame();
        }
    }


}

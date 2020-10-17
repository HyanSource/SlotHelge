using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public class MainManage : MonoBehaviour
{
    public static MainManage Main;

    /// <summary>
    /// 儲存需要的圖片
    /// </summary>
    [SerializeField]
    public Sprite[] Symbol;
    /// <summary>
    /// 持有金錢
    /// </summary>
    [SerializeField]
    public int Money;
    /// <summary>
    /// 最高下注
    /// </summary>
    [SerializeField]
    public int MaxBet;
    /// <summary>
    /// 下注金額
    /// </summary>
    [SerializeField]
    public int Bet;
    /// <summary>
    /// 自動下注
    /// </summary>
    [SerializeField]
    public bool Auto;
    /// <summary>
    /// 下注金額物件
    /// </summary>
    [SerializeField]
    public TextMesh TextBet_Obj;
    /// <summary>
    /// 身上金額物件
    /// </summary>
    [SerializeField]
    public TextMesh TextMoney_Obj;
    /// <summary>
    /// 獲得金額物件
    /// </summary>
    [SerializeField]
    public TextMesh TextWinMoney_Obj;
    /// <summary>
    /// 滾輪
    /// </summary>
    [SerializeField]
    public ReelsManage Reels_Obj;
    /// <summary>
    /// 得分線
    /// </summary>
    [SerializeField]
    public LineManage Lines_Obj;

    [SerializeField]
    public BoxCollider2D Play_Obj;

    [SerializeField]
    public BoxCollider2D Auto_Obj;

    [SerializeField]
    public InputField HostText;
    [SerializeField]
    public InputField PortText;
    [SerializeField]
    public SpriteRenderer Login_Back;

    public void LoginCLick()
    {
        SM = new SocketManage(HostText.text, int.Parse(PortText.text));
        SM.Init();

        StartCoroutine(StartLogin());
    }

    [SerializeField]
    public SocketManage SM;

    private void Awake()
    {
        Main = this;
        //SM = new SocketManage();
        //SM.Init();
    }

    private void Update()
    {
        if (SM == null || !SM.OnLine)
        {
            return;
        }
        //取得訊息
        SocketManage.MessagePack t = SM.GetData();
        if (t != null)
        {
            switch (t.MsgID)
            {
                case 10://登入更新金錢
                    {
                        Pb.Money m = HyanProto.UnMarshal<Pb.Money>(t.Data);
                        TextMoney_Obj.text = m.Money_.ToString();
                    }
                    break;
                case 100://盤面
                    {
                        Pb.Result result = HyanProto.UnMarshal<Pb.Result>(t.Data);
                        TextMoney_Obj.text = result.Money.Money_.ToString();
                        //TextWinMoney_Obj.text = result.WinMoney.ToString();
                        Reels_Obj.StartGame(result.Table.ToArray(), result.Paylinesnum.ToArray(), result.WinMoney);

                    }
                    break;
                default:
                    break;
            }
        }
    }
    /// <summary>
    /// 遊玩事件
    /// </summary>
    public void PlayGame()
    {
        if (Play_Obj.enabled)
        {
            Play_Obj.enabled = false;
            Pb.Play p = new Pb.Play() { Bet = Bet };
            MainManage.Main.SM.SendMsg(100, p);
        }

    }

    /// <summary>
    /// 初始化
    /// </summary>
    public void init_()
    {
        TextWinMoney_Obj.text = "0";
    }

    IEnumerator StartLogin()
    {
        while (!SM.OnLine)
        {
            yield return null;
        }
        Login_Back.gameObject.SetActive(false);
    }

    /// <summary>
    /// 應用程式關閉時會做的事情
    /// </summary>
    private void OnApplicationQuit()
    {
        SM.close();
    }
}

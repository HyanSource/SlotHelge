using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BetControll : MonoBehaviour
{
    /// <summary>
    /// 下注金額的文字
    /// </summary>
    [SerializeField]
    public TextMesh TextBet;
    /// <summary>
    /// 下注間隔
    /// </summary>
    [SerializeField]
    public int Raise;

    private void Awake()
    {
        TextBet = this.GetComponent<TextMesh>();
    }

    public void Start()
    {
        //預設押注
        MainManage.Main.Bet = Raise;
        ChangeBet();
    }

    /// <summary>
    /// 減少下注
    /// </summary>
    public void Deincrease()
    {
        MainManage.Main.Bet -= (MainManage.Main.Bet - Raise) > 0 ? Raise : 0;
        ChangeBet();
    }
    /// <summary>
    /// 增加下注
    /// </summary>
    public void Increase()
    {
        MainManage.Main.Bet += (MainManage.Main.Bet + Raise) > MainManage.Main.MaxBet ? 0 : Raise;
        ChangeBet();
    }
    /// <summary>
    /// 最高下注
    /// </summary>
    public void Max()
    {
        MainManage.Main.Bet = MainManage.Main.MaxBet;
        ChangeBet();

    }

    private void ChangeBet()
    {
        Debug.Log(MainManage.Main.Bet);
        TextBet.text = MainManage.Main.Bet.ToString();
    }
}

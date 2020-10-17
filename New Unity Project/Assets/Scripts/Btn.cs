using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Btn : MonoBehaviour
{
    /// <summary>
    /// 按鈕的所有類型
    /// </summary>
    enum BTN_TYPE
    {
        //一般按鈕
        NORMAL = 0,
        //切換按鈕
        SWITCH = 1,
    }

    /// <summary>
    /// 按鈕類型
    /// </summary>
    [SerializeField]
    BTN_TYPE Type;

    /// <summary>
    /// 使用到的圖片
    /// </summary>
    [SerializeField]
    public Sprite[] Images;
    /// <summary>
    /// 
    /// </summary>
    [SerializeField]
    SpriteRenderer s;

    [SerializeField]
    UnityEvent e;

    /// <summary>
    /// 按下
    /// </summary>
    bool press = false;

    private void Awake()
    {
        s = this.GetComponent<SpriteRenderer>();
    }

    /// <summary>
    /// 按下
    /// </summary>
    private void OnMouseDown()
    {
        switch (Type)
        {
            case BTN_TYPE.NORMAL:
                {
                    s.sprite = Images[1];
                }
                break;
            case BTN_TYPE.SWITCH:
                {

                }
                break;
        }

        press = true;
    }
    /// <summary>
    /// 彈起
    /// </summary>
    private void OnMouseUp()
    {
        switch (Type)
        {
            case BTN_TYPE.NORMAL:
                {
                    s.sprite = Images[0];
                    e.Invoke();
                }
                break;
            case BTN_TYPE.SWITCH:
                {
                    MainManage.Main.Auto = !MainManage.Main.Auto;
                    s.sprite = Images[MainManage.Main.Auto ? 1 : 0];
                    e.Invoke();
                }
                break;
        }

        press = false;
    }

}

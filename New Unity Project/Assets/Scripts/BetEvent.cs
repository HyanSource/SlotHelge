using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Google.Protobuf;

public class BetEvent : MonoBehaviour
{
    /// <summary>
    /// 綁定需要的事件
    /// </summary>
    [SerializeField]
    public UnityEvent e;

    /// <summary>
    /// 按下
    /// </summary>
    private void OnMouseDown()
    {
        if (e != null)
        {
            e.Invoke();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineManage : MonoBehaviour
{
    [SerializeField]
    public SpriteRenderer[] Lines;

    public void CloseLine()
    {
        for (int i = 0; i < Lines.Length; i++)
        {
            Lines[i].gameObject.SetActive(false);
        }
    }

    public void OpenLine(int[] l)
    {
        for (int i = 0; i < l.Length; i++)
        {
            Lines[l[i]].gameObject.SetActive(true);
        }
    }

}

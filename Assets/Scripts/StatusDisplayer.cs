using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusDisplayer : MonoBehaviour
{
    [SerializeField]
    private Spinner p1 = null;
    [SerializeField]
    private Spinner p2 = null;

    private void Start()
    {
        StopCalculating(p1);
        StopCalculating(p2);
    }



    public void ShowCalculating(sbyte p)
    {
        if (p == 1)
        {
            ShowCalculating(p1);
        }
        else
        {
            ShowCalculating(p2);
        }
    }

    public void StopCalculating(sbyte p)
    {
        if (p == 1)
        {
            StopCalculating(p1);
        }
        else
        {
            StopCalculating(p2);
        }
    }


    public void ShowCalculating(Spinner p)
    {
        p.Show();
        p.StartEffect();
    }

    public void StopCalculating(Spinner p)
    {
        p.Hide();
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusDisplayer : MonoBehaviour
{
    [SerializeField]
    Spinner p1 = null;
    [SerializeField]
    Spinner p2 = null;

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

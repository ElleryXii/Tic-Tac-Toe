using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Spinner : MonoBehaviour
{

    [SerializeField]
    RectTransform holder;
    [SerializeField]
    Image spinnerImage;
    [SerializeField]
    Text textProgress;
    [SerializeField]
    [Range(0, 1)]
    float progress = 0f;
    [SerializeField]
    bool spining = true;


    void Start()
    {
        StartCoroutine("Spin");
    }

    IEnumerator Load()
    {
        while (progress < 1f)
        {
            progress += 0.01f;
            spinnerImage.fillAmount = progress;
            textProgress.text = Mathf.Floor(progress * 100).ToString();
            yield return new WaitForSeconds(.1f);
            //yield return 1;
        }
    }


    public void StopEffect()
    {
        spining = false;
    }

    public void StartEffect()
    {
        spining = true;
        StartCoroutine("Sping");
    }


    IEnumerator Spin()
    {
        while (spining)
        {
            if (spinnerImage.fillAmount >= 1f)
            {
                spinnerImage.fillClockwise = false;
            }
            if (spinnerImage.fillAmount <= 0f)
            {
                spinnerImage.fillClockwise = true;

            }
            float amount = spinnerImage.fillClockwise ? 0.01f : -0.01f;
            spinnerImage.fillAmount += amount;
            yield return new WaitForSeconds(.01f);
        }
    }


}

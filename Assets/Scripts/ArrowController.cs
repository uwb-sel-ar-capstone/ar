using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowController : MonoBehaviour
{
    public GameObject baseArrow;
    private bool isBaseArrowOn;

    public GameObject binArrow;
    private bool isBinArrowOn;

    // Start is called before the first frame update
    void Start()
    {
        isBaseArrowOn = true;
        baseArrow.SetActive(true);

        isBinArrowOn = true;
    }

    public void interactBin()
    {
        if (isBinArrowOn)
        {
            isBinArrowOn = false;
            binArrow.SetActive(false);
        } else
        {
            isBinArrowOn = true;
            binArrow.SetActive(true);
        }

        if (isBaseArrowOn)
        {
            isBaseArrowOn = false;
            baseArrow.SetActive(false);
        } else
        {
            isBaseArrowOn = true;
            baseArrow.SetActive(true);
        }     
    }
}

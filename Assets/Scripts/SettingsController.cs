using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class SettingsController : MonoBehaviour
{
    public GameObject ColorMenu;
    private bool isScreenActive;

    public GameObject MoveHandle;
    public GameObject GrabHandle;
    public GameObject Cube;
    public GameObject Cube1;
    public GameObject Cube2;
    public GameObject Cube3;
    public GameObject binHandle;
    public GameObject binCube;
    public GameObject binCube1;
    public GameObject binCube2;
    public GameObject binCube3;

    public GameObject WIButton;
    public GameObject StepButton;
    public GameObject ColorButton;
    public GameObject MoveButton;
    public GameObject IPButton;

    public GameObject WIMenu;
    private bool isWIActive;

    private bool isMoveActive;

    public GameObject StepMenu;

    public CanvasGroup myUIGroup;
    public Slider opacitySlider;

    private bool isIPActive;
    public GameObject KeypadMenu;

    public Button WIButton1;
    public Button WIButton2;
    public Button WIButton3;

    void Start()
    {
        WIButton1.onClick.AddListener(delegate { SetStep(); });
        WIButton2.onClick.AddListener(delegate { SetStep(); });
        WIButton3.onClick.AddListener(delegate { SetStep(); });

        opacitySlider.onValueChanged.AddListener(delegate { changeUIOpacity(); });

        isScreenActive = false;
        ColorMenu.SetActive(false);

        Color WIcol = WIButton.GetComponent<Image>().color;
        WIcol.a = 0.35f;
        WIButton.GetComponent<Image>().color = WIcol;
        isWIActive = true;
        WIMenu.SetActive(true);

        isMoveActive = false;
        MoveHandle.SetActive(false);
        GrabHandle.SetActive(false);
        Cube.SetActive(false);
        Cube1.SetActive(false);
        Cube2.SetActive(false);
        Cube3.SetActive(false);
        binHandle.SetActive(false);
        binCube.SetActive(false);
        binCube1.SetActive(false);
        binCube2.SetActive(false);
        binCube3.SetActive(false);

        StepMenu.SetActive(false);

        isIPActive = false;
        KeypadMenu.SetActive(false);
    }

    void changeUIOpacity()
    {
        myUIGroup.alpha = opacitySlider.value;
    }

    public void SetScreen()
    {
        Color WIcol = WIButton.GetComponent<Image>().color;
        Color Stepcol = StepButton.GetComponent<Image>().color;
        Color Colorcol = ColorButton.GetComponent<Image>().color;

        WIcol.a = 1f;

        WIButton.GetComponent<Image>().color = WIcol;

        isWIActive = false;
        StepMenu.SetActive(false);
        WIMenu.SetActive(false);

        if (isScreenActive)
        {
            ColorMenu.SetActive(false);
            isScreenActive = false;
            StepMenu.SetActive(true);
            Colorcol.a = 1f;
            ColorButton.GetComponent<Image>().color = Colorcol;
            Stepcol.a = 0.35f;
            StepButton.GetComponent<Image>().color = Stepcol;
        }
        else
        {
            ColorMenu.SetActive(true);
            isScreenActive= true;
            Colorcol.a = 0.35f;
            ColorButton.GetComponent<Image>().color = Colorcol;
            Stepcol.a = 1f;
            StepButton.GetComponent<Image>().color = Stepcol;
        }
    }

    public void SetStep()
    {
        Color WIcol = WIButton.GetComponent<Image>().color;
        Color Stepcol = StepButton.GetComponent<Image>().color;
        Color Colorcol = ColorButton.GetComponent<Image>().color;

        WIcol.a = 1f;
        Stepcol.a = 0.35f;
        Colorcol.a = 1f;

        WIButton.GetComponent<Image>().color = WIcol;
        StepButton.GetComponent<Image>().color = Stepcol;
        ColorButton.GetComponent<Image>().color = Colorcol;

        isScreenActive = false;
        isWIActive = false;
        ColorMenu.SetActive(false);
        WIMenu.SetActive(false);

        StepMenu.SetActive(true);
    }

    public void SetWI()
    {
        Color WIcol = WIButton.GetComponent<Image>().color;
        Color Stepcol = StepButton.GetComponent<Image>().color;
        Color Colorcol = ColorButton.GetComponent<Image>().color;

        Colorcol.a = 1f;

        ColorButton.GetComponent<Image>().color = Colorcol;

        isScreenActive = false;
        ColorMenu.SetActive(false);
        StepMenu.SetActive(false);

        if (isWIActive)
        {
            WIMenu.SetActive(false);
            isWIActive = false;
            StepMenu.SetActive(true);
            WIcol.a = 1f;
            WIButton.GetComponent<Image>().color = WIcol;
            Stepcol.a = 0.35f;
            StepButton.GetComponent<Image>().color = Stepcol;
        }
        else
        {
            WIMenu.SetActive(true);
            isWIActive = true;
            WIcol.a = 0.35f;
            WIButton.GetComponent<Image>().color = WIcol;
            Stepcol.a = 1f;
            StepButton.GetComponent<Image>().color = Stepcol;
        }
    }

    public void SetMove()
    {
        Color Movecol = MoveButton.GetComponent<Image>().color;

        if (isMoveActive)
        {
            isMoveActive = false;
            Movecol.a = 1f;
            MoveButton.GetComponent<Image>().color = Movecol;
        }
        else
        {
            isMoveActive = true;
            Movecol.a = 0.35f; 
            MoveButton.GetComponent<Image>().color = Movecol;
        }

        MoveHandle.SetActive(isMoveActive);
        GrabHandle.SetActive(isMoveActive);
        Cube.SetActive(isMoveActive);
        Cube1.SetActive(isMoveActive);
        Cube2.SetActive(isMoveActive);
        Cube3.SetActive(isMoveActive);
        binHandle.SetActive(isMoveActive);
        binCube.SetActive(isMoveActive);
        binCube1.SetActive(isMoveActive);
        binCube2.SetActive(isMoveActive);
        binCube3.SetActive(isMoveActive);
    }

    public void SetIP()
    {
        Color IPcol = IPButton.GetComponent<Image>().color;

        if (isIPActive)
        {
            isIPActive = false;
            KeypadMenu.SetActive(false);
            IPcol.a = 1f;
            IPButton.GetComponent<Image>().color = IPcol;
        }
        else
        {
            isIPActive = true;
            KeypadMenu.SetActive(true);
            IPcol.a = 0.35f;
            IPButton.GetComponent<Image>().color = IPcol;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using System;
using static System.Net.WebRequestMethods;
using System.Text.RegularExpressions;

public class ARController : MonoBehaviour
{
    public float xDimen;
    public float zDimen;
    public float yDimen;

    public Button nextButton; // Button to go to next step in WI
    public Button prevButton; // Button to go to previous step in WI

    public Slider progressSlider; // Slider object to visualize step progress

    public TMP_Text wiText;
    public TMP_Text itemText;
    public TMP_Text stepText; // Text to display the step instruction
    public TMP_Text progressText; // Text to display the step progress (Ex: Step 1/5)

    public GameObject image; // Work instruction image

    public GameObject breadboard; // Breadboard gameobject includes border
    public GameObject marker; // Top left indicator and marker reference 

    // Material colors for the AR images
    public Material red;
    public Material orange;
    public Material yellow;
    public Material green;
    public Material blue;
    public Material purple;
    public Material transparent;

    // Buttons to change color of AR images
    public Button redButton;
    public Button orangeButton;
    public Button yellowButton;
    public Button greenButton;
    public Button blueButton;
    public Button purpleButton;

    public GameObject cube;
    public GameObject cube1;
    public GameObject cube2;
    public GameObject cube3;
    public GameObject handle;
    public GameObject benchmark;

    public Material color; // Default material color when app is loaded
    string imageData;

    public GameObject label;
    public GameObject baseArrow;
    public GameObject binScreen;
    public Button binButton;
    private bool displayBaseArrows = true;
    public GameObject binArrow;
    public GameObject Shaft;
    public GameObject Cone;

    public string IP = "";

    public Button zero;
    public Button one;
    public Button two;
    public Button three;
    public Button four;
    public Button five;
    public Button six;
    public Button seven;
    public Button eight;
    public Button nine;
    public Button Dot;
    public Button Back;

    public Button Refresh;

    public TMP_InputField input;

    private int pageNumbers;
    private int wiPageIndex;

    public GameObject WICard1;
    public GameObject WICard1Image;
    public TMP_Text WICard1Title;
    public Button WIButton1;
    private string WIID1 = "";

    public GameObject WICard2;
    public GameObject WICard2Image;
    public TMP_Text WICard2Title;
    public Button WIButton2;
    private string WIID2 = "";

    public GameObject WICard3;
    public GameObject WICard3Image;
    public TMP_Text WICard3Title;
    public Button WIButton3;
    private string WIID3 = "";

    public Button nextWI;
    public Button prevWI;
    public TMP_Text wiProgressText;

    private int wiIndex;
    bool prevInstrReverse;

    List<Segment> segments = new List<Segment>(); // List to hold component positions (list because number of positions variable)
    List<float> positions = new List<float>();
    List<stepsList> steps = new List<stepsList>();

    public int i; // index for steps and components arrays

    void Start()
    {
        Refresh.onClick.AddListener(delegate { startWIListCall(); });

        nextWI.onClick.AddListener(delegate { nextWIMenu(); });
        prevWI.onClick.AddListener(delegate { prevWIMenu(); });

        WIButton1.onClick.AddListener(delegate { startWICall(WIID1); });
        WIButton2.onClick.AddListener(delegate { startWICall(WIID2); });
        WIButton3.onClick.AddListener(delegate { startWICall(WIID3); });

        WICard1.transform.localScale = new Vector3(0, 0, 0);
        WICard2.transform.localScale = new Vector3(0, 0, 0);
        WICard3.transform.localScale = new Vector3(0, 0, 0);
        nextWI.transform.localScale = new Vector3(0, 0, 0);
        prevWI.transform.localScale = new Vector3(0, 0, 0);
        wiProgressText.transform.localScale = new Vector3(0, 0, 0);

        marker.transform.localScale = new Vector3(0, 0, 0); // Make marker reference invisible (scale = 0 on Vector3)
        label.transform.localScale = new Vector3(0, 0, 0);
        binScreen.transform.localScale = new Vector3(0, 0, 0);
        baseArrow.transform.localScale = new Vector3(0, 0, 0);
        binArrow.transform.localScale = new Vector3(0, 0, 0);

        image.SetActive(false);

        // Listener calls for the next and previous step buttons
        nextButton.onClick.AddListener(delegate { nextStep(steps); });
        prevButton.onClick.AddListener(delegate { prevStep(steps); });

        // Listener calls for the color changing buttons 
        redButton.onClick.AddListener(delegate { changeColor(red); });
        orangeButton.onClick.AddListener(delegate { changeColor(orange); });
        yellowButton.onClick.AddListener(delegate { changeColor(yellow); });
        greenButton.onClick.AddListener(delegate { changeColor(green); });
        blueButton.onClick.AddListener(delegate { changeColor(blue); });
        purpleButton.onClick.AddListener(delegate { changeColor(purple); });

        binButton.onClick.AddListener(updateBaseArrows);

        zero.onClick.AddListener(delegate { updateIP('0'); });
        one.onClick.AddListener(delegate { updateIP('1'); });
        two.onClick.AddListener(delegate { updateIP('2'); });
        three.onClick.AddListener(delegate { updateIP('3'); });
        four.onClick.AddListener(delegate { updateIP('4'); });
        five.onClick.AddListener(delegate { updateIP('5'); });
        six.onClick.AddListener(delegate { updateIP('6'); });
        seven.onClick.AddListener(delegate { updateIP('7'); });
        eight.onClick.AddListener(delegate { updateIP('8'); });
        nine.onClick.AddListener(delegate { updateIP('9'); });
        Dot.onClick.AddListener(delegate { updateIP('.'); });
        Back.onClick.AddListener(delegate { backIP(); });
    }

    void createWIListMenu()
    {
        WICard1.transform.localScale = new Vector3(0, 0, 0);
        WICard2.transform.localScale = new Vector3(0, 0, 0);
        WICard3.transform.localScale = new Vector3(0, 0, 0);
        nextWI.transform.localScale = new Vector3(0, 0, 0);
        prevWI.transform.localScale = new Vector3(0, 0, 0);
        wiProgressText.transform.localScale = new Vector3(0, 0, 0);

        wiPageIndex = 1;

        if (wis.getLength() % 3 == 0)
        {
            pageNumbers = wis.getLength() / 3;
        } 
        else
        {
            pageNumbers = (wis.getLength() / 3) + 1;
        }

        if (pageNumbers > 1)
        {
            nextWI.transform.localScale = new Vector3(1.102532f, 1.498985f, 1.008817f);
            prevWI.transform.localScale = new Vector3(1.102532f, 1.498985f, 1.008817f);
            wiProgressText.transform.localScale = new Vector3(1.102532f, 1.498985f, 1.008817f);

            wiProgressText.text = "" + wiPageIndex + " / " + pageNumbers;
        }

        if (wis.getLength() == 1)
        {
            WICard1.transform.localScale = new Vector3(0.25f, 0.7f, 1);

            loadImage(wis.getWI(0).image.imageData, WICard1Image);

            WICard1Title.text = wis.getWI(0).name;

            WIID1 = wis.getWI(0)._id;

            wiIndex = 1;
        }
        else if (wis.getLength() == 2)
        {
            WICard1.transform.localScale = new Vector3(0.25f, 0.7f, 1);
            WICard2.transform.localScale = new Vector3(0.25f, 0.7f, 1);

            loadImage(wis.getWI(0).image.imageData, WICard1Image);
            loadImage(wis.getWI(1).image.imageData, WICard2Image);

            WICard1Title.text = wis.getWI(0).name;
            WICard2Title.text = wis.getWI(1).name;

            WIID1 = wis.getWI(0)._id;
            WIID2 = wis.getWI(1)._id;

            wiIndex = 2;
        }
        else if (wis.getLength() >= 3)
        {
            WICard1.transform.localScale = new Vector3(0.25f, 0.7f, 1);
            WICard2.transform.localScale = new Vector3(0.25f, 0.7f, 1);
            WICard3.transform.localScale = new Vector3(0.25f, 0.7f, 1);

            loadImage(wis.getWI(0).image.imageData, WICard1Image);
            loadImage(wis.getWI(1).image.imageData, WICard2Image);
            loadImage(wis.getWI(2).image.imageData, WICard3Image);

            WICard1Title.text = wis.getWI(0).name;
            WICard2Title.text = wis.getWI(1).name;
            WICard3Title.text = wis.getWI(2).name;

            WIID1 = wis.getWI(0)._id;
            WIID2 = wis.getWI(1)._id;
            WIID3 = wis.getWI(2)._id;

            wiIndex = 2;
        }
        
    }

    void nextWIMenu()
    {
        if (wiPageIndex + 1 <= pageNumbers)
        {
            wiPageIndex++;

            if (prevInstrReverse)
            {
                if (wiIndex % 3 == 0)
                {
                    wiIndex += 2;
                }
                else if (wiIndex % 3 == 1)
                {
                    wiIndex += 1;
                }
            }

            prevInstrReverse = false;

            if (wiPageIndex != pageNumbers)
            {
                WICard1.transform.localScale = new Vector3(0.25f, 0.7f, 1);
                WICard2.transform.localScale = new Vector3(0.25f, 0.7f, 1);
                WICard3.transform.localScale = new Vector3(0.25f, 0.7f, 1);

                loadImage(wis.getWI(++wiIndex).image.imageData, WICard1Image);
                WICard1Title.text = wis.getWI(wiIndex).name;
                WIID1 = wis.getWI(wiIndex)._id;
                loadImage(wis.getWI(++wiIndex).image.imageData, WICard2Image);
                WICard2Title.text = wis.getWI(wiIndex).name;
                WIID2 = wis.getWI(wiIndex)._id;
                loadImage(wis.getWI(++wiIndex).image.imageData, WICard3Image);
                WICard3Title.text = wis.getWI(wiIndex).name;
                WIID3 = wis.getWI(wiIndex)._id;
            }
            else
            {
                if (wis.getLength() - (wiIndex + 1) == 1)
                {
                    WICard1.transform.localScale = new Vector3(0.25f, 0.7f, 1);
                    WICard2.transform.localScale = new Vector3(0, 0, 0);
                    WICard3.transform.localScale = new Vector3(0, 0, 0);

                    loadImage(wis.getWI(++wiIndex).image.imageData, WICard1Image);
                    WICard1Title.text = wis.getWI(wiIndex).name;
                    WIID1 = wis.getWI(wiIndex)._id;
                }
                else if (wis.getLength() - (wiIndex + 1) == 2)
                {
                    WICard1.transform.localScale = new Vector3(0.25f, 0.7f, 1);
                    WICard2.transform.localScale = new Vector3(0.25f, 0.7f, 1);
                    WICard3.transform.localScale = new Vector3(0, 0, 0);

                    loadImage(wis.getWI(++wiIndex).image.imageData, WICard1Image);
                    WICard1Title.text = wis.getWI(wiIndex).name;
                    WIID1 = wis.getWI(wiIndex)._id;
                    loadImage(wis.getWI(++wiIndex).image.imageData, WICard2Image);
                    WICard2Title.text = wis.getWI(wiIndex).name;
                    WIID2 = wis.getWI(wiIndex)._id;
                }
                else if (wis.getLength() - (wiIndex + 1) == 3)
                {
                    WICard1.transform.localScale = new Vector3(0.25f, 0.7f, 1);
                    WICard2.transform.localScale = new Vector3(0.25f, 0.7f, 1);
                    WICard3.transform.localScale = new Vector3(0.25f, 0.7f, 1);

                    loadImage(wis.getWI(++wiIndex).image.imageData, WICard1Image);
                    WICard1Title.text = wis.getWI(wiIndex).name;
                    WIID1 = wis.getWI(wiIndex)._id;
                    loadImage(wis.getWI(++wiIndex).image.imageData, WICard2Image);
                    WICard2Title.text = wis.getWI(wiIndex).name;
                    WIID2 = wis.getWI(wiIndex)._id;
                    loadImage(wis.getWI(++wiIndex).image.imageData, WICard3Image);
                    WICard3Title.text = wis.getWI(wiIndex).name;
                    WIID3 = wis.getWI(wiIndex)._id;
                }
            }
        }
        wiProgressText.text = "" + wiPageIndex + " / " + pageNumbers;
    }

    void prevWIMenu()
    {
        if (wiPageIndex - 1 >= 1)
        {
            wiPageIndex--;

            if (!prevInstrReverse)
            {
                if (wiIndex % 3 == 1)
                {
                    wiIndex -= 1;
                }
                else if (wiIndex % 3 == 2)
                {
                    wiIndex -= 2;
                }
            }

            prevInstrReverse = true;

            WICard1.transform.localScale = new Vector3(0.25f, 0.7f, 1);
            WICard2.transform.localScale = new Vector3(0.25f, 0.7f, 1);
            WICard3.transform.localScale = new Vector3(0.25f, 0.7f, 1);

            loadImage(wis.getWI(--wiIndex).image.imageData, WICard3Image);
            WICard3Title.text = wis.getWI(wiIndex).name;
            WIID3 = wis.getWI(wiIndex)._id;
            loadImage(wis.getWI(--wiIndex).image.imageData, WICard2Image);
            WICard2Title.text = wis.getWI(wiIndex).name;
            WIID2 = wis.getWI(wiIndex)._id;
            loadImage(wis.getWI(--wiIndex).image.imageData, WICard1Image);
            WICard1Title.text = wis.getWI(wiIndex).name;
            WIID1 = wis.getWI(wiIndex)._id;
        }
        wiProgressText.text = "" + wiPageIndex + " / " + pageNumbers;
    }

    void updateIP(char item)
    {
        IP = IP + "" + item;
        input.text = IP;
    }

    void backIP()
    {
        if (IP.Length > 0) 
        {
            IP = IP.Remove(IP.Length - 1, 1);
        }
        input.text = IP;
    }

    void updateBaseArrows()
    {
        displayBaseArrows = true;
        binArrow.transform.localScale = new Vector3(0, 0, 0);
    }

    void setup() {
        i = 0;
        createBoundary();
        image.SetActive(true);
        progressSlider.value = 0.0f; // Set slider default progress to 0
        label.transform.localScale = new Vector3(0, 0, 0);
        wiText.text = wi.getWIName();
        steps = wi.getSteps();
        
        if (steps[i].image != null )
        {
            imageData = steps[i].getStepImageData();
            loadImage(imageData, image);
        } else
        {
            loadImage("EMPTY", image);
        }
        
        populateItemsList();
        transAllBins();
        currBin = bins[0].getBinName();

        currBin.GetComponent<Renderer>().material = color;

        stepText.text = ConvertHtmlToRichText(RemovePTags(steps[i].getStepText()));

        progressText.text = "" + (i + 1) + " / " + steps.Count;

        displayBaseArrows = false;

        positions = steps[i].getPositions();
        for (int j = 0; j < positions.Count; j += 4)
        {
            segments.Add(new Segment(xDimen, zDimen, yDimen, positions[j], positions[j + 1], positions[j + 2], positions[j + 3], marker, breadboard, baseArrow, displayBaseArrows));
        }
        label.transform.localScale = new Vector3(0.0005f, 0.0005f, 0.0005f);
        label.transform.position = new Vector3(currBin.transform.position.x - 0.023f, currBin.transform.position.y + 0.02f, currBin.transform.position.z - 0.002f);
        binScreen.transform.localScale = new Vector3(1f, 1f, 1f);
        binScreen.transform.position = new Vector3(currBin.transform.position.x, currBin.transform.position.y, currBin.transform.position.z - 0.049600003f);

        binArrow.transform.localScale = new Vector3(2, 2, 2);

        itemText.text = steps[i].getStepItemText();
    }

    public void loadImage(string imageData, GameObject imagePopulate)
    {
        if (!imageData.Equals("EMPTY"))
        {
            byte[] imageBytes = Convert.FromBase64String(imageData);
            Texture2D tex = new Texture2D(2, 2);
            tex.LoadImage(imageBytes);
            Sprite sprite = Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
            imagePopulate.GetComponent<Image>().sprite = sprite;
        } else
        {
            imagePopulate.GetComponent<Image>().sprite = null;
        }
    }

    void createBoundary()
    {
        xDimen = wi.getXDimension() * 0.01020408f;
        zDimen = wi.getZDimension() * 0.01020408f;
        yDimen = wi.getYDimension() * 0.01020408f;

        cube.transform.localScale = new Vector3(xDimen, 0.001333333f, 0.004f);
        cube1.transform.localScale = new Vector3(xDimen, 0.001333333f, 0.004f);
        cube2.transform.localScale = new Vector3(0.004f, 0.001333333f, zDimen + 0.004f);
        cube3.transform.localScale = new Vector3(0.004f, 0.001333333f, zDimen + 0.004f);

        cube.transform.position = new Vector3(benchmark.transform.position.x + xDimen / 2, benchmark.transform.position.y - 0f, benchmark.transform.position.z - 0f);
        cube1.transform.position = new Vector3(benchmark.transform.position.x + xDimen / 2, benchmark.transform.position.y - 0f, benchmark.transform.position.z + zDimen);
        cube2.transform.position = new Vector3(benchmark.transform.position.x + 0f, benchmark.transform.position.y - 0f, benchmark.transform.position.z + zDimen / 2);
        cube3.transform.position = new Vector3(benchmark.transform.position.x + xDimen, benchmark.transform.position.y - 0f, benchmark.transform.position.z + zDimen / 2);

        handle.transform.localScale = new Vector3(xDimen / 1.5f, 0.003f, 0.003f);
        handle.transform.position = new Vector3(benchmark.transform.position.x + xDimen / 2, benchmark.transform.position.y - 0f, benchmark.transform.position.z -0.009500001f);
    }

    void nextStep(List<stepsList> steps)
    {
        if (i < steps.Count - 1)
        {
            currBin.GetComponent<Renderer>().material = transparent;

            clearSegments();

            stepText.text = ConvertHtmlToRichText(RemovePTags(steps[++i].getStepText()));

            if (steps[i].image != null)
            {
                imageData = steps[i].getStepImageData();
                loadImage(imageData, image);
            } else
            {
                loadImage("EMPTY", image);
            }

            currBin = getPartsBin(steps[i].getStepItemText());
            currBin.GetComponent<Renderer>().material = color;

            positions = steps[i].getPositions();
            progressText.text = "" + (i + 1) + " / " + steps.Count;
            progressSlider.value = i / (steps.Count - 1 * 1.0f);
            label.transform.localScale = new Vector3(0.0005f, 0.0005f, 0.0005f);
            label.transform.position = new Vector3(currBin.transform.position.x - 0.023f, currBin.transform.position.y + 0.02f, currBin.transform.position.z - 0.002f);
            binScreen.transform.localScale = new Vector3(1f, 1f, 1f);
            binScreen.transform.position = new Vector3(currBin.transform.position.x, currBin.transform.position.y, currBin.transform.position.z - 0.049600003f);

            binArrow.transform.localScale = new Vector3(2, 2, 2);

            itemText.text = steps[i].getStepItemText();

            displayBaseArrows = false;

            for (int j = 0; j < positions.Count; j += 4)
            {
                segments.Add(new Segment(xDimen, zDimen, yDimen, positions[j], positions[j + 1], positions[j + 2], positions[j + 3], marker, breadboard, baseArrow, displayBaseArrows));
            }
        }
    }

    void prevStep(List<stepsList> steps)
    {
        if (i > 0)
        {
            currBin.GetComponent<Renderer>().material = transparent;

            clearSegments();

            stepText.text = ConvertHtmlToRichText(RemovePTags(steps[--i].getStepText()));

            if (steps[i].image != null)
            {
                imageData = steps[i].getStepImageData();
                loadImage(imageData, image);
            } else
            {
                loadImage("EMPTY", image);
            }

            currBin = getPartsBin(steps[i].getStepItemText());
            currBin.GetComponent<Renderer>().material = color;

            positions = steps[i].getPositions();
            progressText.text = "" + (i + 1) + " / " + steps.Count;
            progressSlider.value = i / (steps.Count - 1 * 1.0f);
            label.transform.localScale = new Vector3(0.0005f, 0.0005f, 0.0005f);
            label.transform.position = new Vector3(currBin.transform.position.x - 0.023f, currBin.transform.position.y + 0.02f, currBin.transform.position.z - 0.002f);
            binScreen.transform.localScale = new Vector3(1f, 1f, 1f);
            binScreen.transform.position = new Vector3(currBin.transform.position.x, currBin.transform.position.y, currBin.transform.position.z - 0.049600003f);

            binArrow.transform.localScale = new Vector3(2, 2, 2);

            itemText.text = steps[i].getStepItemText();

            displayBaseArrows = false;

            for (int j = 0; j < positions.Count; j += 4)
            {
                segments.Add(new Segment(xDimen, zDimen, yDimen, positions[j], positions[j + 1], positions[j + 2], positions[j + 3], marker, breadboard, baseArrow, displayBaseArrows));
            }
        }
    }

    void clearSegments()
    {
        foreach (Segment seg in segments)
        {
            seg.destroy();
        }
        segments.Clear();
    }

    void changeColor(Material newColor)
    {
        color = newColor;
    }

    void Update()
    {
        float phi = Time.time / 2.5f * 2 * Mathf.PI; // 2.5 is the duration, change to reach desired rate
        float amplitude = Mathf.Cos(phi) * 0.5F;
        color.SetColor("_EmissionColor", new Color(amplitude, amplitude, amplitude));
        color.EnableKeyword("_EMISSION");

        if (currBin != null)
        {
            currBin.GetComponent<Renderer>().material = color;
        }

        foreach (Segment seg in segments)
        {
            seg.updatePosition(breadboard, displayBaseArrows);
            seg.updateColor(color);
        }

        Shaft.GetComponent<Renderer>().material = color;
        Cone.GetComponent<Renderer>().material = color;

        binArrow.transform.position += new Vector3(0, 0, Mathf.Sin(Time.time * 3f)) * 0.00045f;
    }

    public workInstruction wi = null;
    public workInstructions wis = null;

    public void startWICall(string wiID)
    {
        clearSegments();
        StartCoroutine(getWICall(wiID));
    }

    public void startWIListCall()
    {
        StartCoroutine(getWIListCall());
    }

    IEnumerator getWICall(string wiID)
    {
        UnityWebRequest infoRequest = UnityWebRequest.Get("http://" + IP + ":5000/api/v1/workinstructions/" + wiID + "?populate=true&imageData=true");
        yield return infoRequest.SendWebRequest();

        if (infoRequest.isNetworkError || infoRequest.isHttpError)
        {
            UnityEngine.Debug.LogError(infoRequest.error);
            // TODO: add setup failure here
        }
        else
        {
            wi = JsonConvert.DeserializeObject<workInstruction>(infoRequest.downloadHandler.text);
            setup();
        }
    }

    IEnumerator getWIListCall()
    {
        UnityWebRequest infoRequest = UnityWebRequest.Get("http://" + IP + ":5000/api/v1/workinstructions?imageData=true&populate=true");
        yield return infoRequest.SendWebRequest();

        if (infoRequest.isNetworkError || infoRequest.isHttpError)
        {
            UnityEngine.Debug.LogError(infoRequest.error);
            // TODO: add setup failure here
        }
        else
        {
            wis = JsonConvert.DeserializeObject<workInstructions>(infoRequest.downloadHandler.text);
            createWIListMenu();
        }
    }

    List<string> items = new List<string>();
    List<Bin> bins = new List<Bin>();
    public GameObject currBin;

    public void populateItemsList()
    {
        items.Clear();
        bins.Clear();

        for (int i = 0; i < steps.Count; i++)
        {
            if (!items.Contains(steps[i].getStepItemText()))
            {
                items.Add(steps[i].getStepItemText());
            }
        }

        populatePartsBin();
    }

    public void populatePartsBin()
    {
        for (int i = 0; i < items.Count; i++)
        {
            bins.Add(new Bin("B" + i, items[i]));
        }
    }

    public GameObject getPartsBin(string currItem)
    {
        for (int i = 0; i < bins.Count; i++)
        {
            if (bins[i].getBinItemName().Equals(currItem))
            {
                return bins[i].getBinName();
            }
        }
        return null;
    }

    public void transAllBins()
    {
        for (int i = 0; i < 24; i++)
        {
            GameObject.Find("B" + i).GetComponent<Renderer>().material = transparent;
        }
    }

    public class Bin
    {
        public string item;
        public GameObject name;

        public Bin(string name, string item)
        {
            this.name = GameObject.Find(name);
            this.item = item;   
        }

        public GameObject getBinName()
        {
            return name;
        }

        public string getBinItemName()
        {
            return item;
        }
    }

    [Serializable]
    public class workInstructions
    {
        public List<wiData> wis;

        public int getLength()
        {
            return wis.Count;
        }

        public wiData getWI(int i)
        {
            return wis[i];
        }
    }

    [Serializable]
    public class workInstruction
    {
        public wiData wi;

        public string getWIName()
        {
            return wi.name;
        }

        public string getWIID()
        {
            return wi._id;
        }

        public float getXDimension()
        {
            return wi.dimensions.xLengthCM;
        }

        public float getZDimension()
        {
            return wi.dimensions.zLengthCM;
        }

        public float getYDimension()
        {
            return wi.dimensions.yLengthCM;
        }

        public List<stepsList> getSteps()
        {
            return wi.steps;
        }

        public string getImageData()
        {
            return wi.image.imageData;
        }
    }

    [Serializable]
    public class wiData
    {
        public dimensionsData dimensions;
        public string _id;
        public string name;
        public List<stepsList> steps;
        public imageInfo image;
    }

    [Serializable]
    public class dimensionsData
    {
        public float xLengthCM;
        public float zLengthCM;
        public float yLengthCM;
    }

    [Serializable]
    public class stepsList
    {
        public string _id;
        public string text;
        public itemData item;
        public imageInfo image;
        public List<positionsList> positions;

        public string getStepText()
        {
            return text;
        }

        public string getStepItemText()
        {
            return item.getItemName();
        }

        public string getStepImageData()
        {
            return image.imageData;
        }

        public List<float> getPositions()
        {
            List<float> floatPositions = new List<float>();

            foreach (positionsList list in positions)
            {
                floatPositions.Add(list.getxStart());
                floatPositions.Add(list.getxEnd());
                floatPositions.Add(list.getzStart());
                floatPositions.Add(list.getzEnd());
            }

            return floatPositions;
        }
       
    }

    [Serializable]
    public class itemData
    {
        public string _id;
        public string name;

        public String getItemName()
        {
            return name;
        }
    }

    [Serializable]
    public class positionsList
    {
        public float xStart;
        public float zStart;
        public float xEnd;
        public float zEnd;

        public float getxStart()
        {
            return xStart;
        }

        public float getxEnd()
        {
            return xEnd;
        }

        public float getzStart()
        {
            return zStart;
        }

        public float getzEnd()
        {
            return zEnd;
        }
    }

    [Serializable]
    public class imageInfo
    {
        public string _id;
        public string imageName;
        public string imageData;
        public string mimeType;
        public string encoding;

        public string getImageData()
        {
            return imageData;
        }
    }

    public TextMeshProUGUI targetText; // The Text component to apply the rich text to

    // Dictionary to map HTML tags to their corresponding Unity Rich Text tags
    private readonly Dictionary<string, string> tagMap = new Dictionary<string, string>()
    {
        {"<b>", "<b>"},
        {"<em>", "<i>"},
        {"</em>", "</i>"},
        {"<strong>", "<b>"},
        {"</strong>", "</b>"},
        {"</b>", "</b>"},
        {"<i>", "<i>"},
        {"</i>", "</i>"},
        {"<u>", "<u>"},
        {"</u>", "</u>"},
        {"<sub>", "<sub>"},
        {"</sub>", "</sub>"},
        {"<sup>", "<sup>"},
        {"</sup>", "</sup>"},
        {"<br>", "\n"},
        {"<p>", "\n"},
        {"</p>", "\n"},
        {"<font color=", "<color="},
        {"<font size=", "<size="},
        {"<font face=", "<font="},
        {"<color=", "<color="},
        {"<size=", "<size="},
        {"<a href=", "<link="},
        {"</a>", "</link>"},
        {"<span style=\"color: ", "<color="}, // added entry
        {"</span>", "</color>"} // added entry
    };

    // Regex pattern to match HTML tags
    private readonly string tagPattern = "<.*?>";

    // Converts HTML to Unity Rich Text
    public string ConvertHtmlToRichText(string html)
    {
        string richText = html;

        // Replace HTML tags with their corresponding Unity Rich Text tags
        foreach (KeyValuePair<string, string> entry in tagMap)
        {
            richText = richText.Replace(entry.Key, entry.Value);
        }

        return richText;
    }

    public string RemovePTags(string input)
    {
        if (input.StartsWith("<p>"))
        {
            input = input.Substring(3);
        }

        if (input.EndsWith("</p>"))
        {
            input = input.Substring(0, input.Length - 4);
        }

        return input;
    }
}

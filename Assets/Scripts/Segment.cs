using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Segment
{
    // XYZ dimensions of the component to display AR images onto 
    public float xDimen;
    public float zDimen;
    public float yDimen;
    //public float yDimen = 0.01020408f;

    // Gameobjects for each corner of the segment
    private GameObject xSeg1;
    private GameObject xSeg2;
    private GameObject zSeg1;
    private GameObject zSeg2;

    private GameObject arrow;
    private GameObject shaft;
    private GameObject cone;

    // Floats for the 4 marker corner positions
    private float xStart;
    private float xEnd;
    private float zStart;
    private float zEnd;


    // Constructor creates a new segment based on the given percentages in the X and Z plane
    public Segment(float x, float z, float y, float x1, float x2, float z1, float z2, GameObject marker, GameObject breadboard, GameObject baseArrow, bool displayBaseArrows)
    {
        xDimen = x;
        zDimen = z;
        yDimen = y;

        // Create empty gameobjects for each side using the marker reference
        xSeg1 = GameObject.Instantiate(marker);
        xSeg2 = GameObject.Instantiate(marker);
        zSeg1 = GameObject.Instantiate(marker);
        zSeg2 = GameObject.Instantiate(marker);

        arrow = GameObject.Instantiate(baseArrow);
        shaft = arrow.transform.Find("Shaft").gameObject;
        cone = arrow.transform.Find("Cone").gameObject;

        // Calculate the start and ending position for the X and Z planes
        xStart = xDimen * x1;
        xEnd = xDimen * x2;
        zStart = zDimen * z1;
        zEnd = zDimen * z2;

        updatePosition(breadboard, displayBaseArrows); // Creates and places the segment in the correct place
    }

    // Destructor destroys all 4 segment sides in the segment 
    public void destroy()
    {
        GameObject.Destroy(xSeg1);
        GameObject.Destroy(xSeg2);
        GameObject.Destroy(zSeg1);
        GameObject.Destroy(zSeg2);
        GameObject.Destroy(arrow);
    }

    // Update position gets the breadboard's position and updates the segment's position based on the value (used when user is moving the border)
    public void updatePosition(GameObject breadboard, bool displayBaseArrows)
    {
        // Position the segment sides relative to the breadboard's position
        xSeg1.transform.position = new Vector3(breadboard.transform.position.x + xStart + (xEnd - xStart), yDimen, breadboard.transform.position.z + (zDimen - zStart));
        xSeg1.transform.position += new Vector3((xEnd - xStart) / 2 + (-1 * (xEnd - xStart)), 0, 0);
        xSeg2.transform.position = new Vector3(breadboard.transform.position.x + xStart + (xEnd - xStart), yDimen, breadboard.transform.position.z + (zDimen - zEnd));
        xSeg2.transform.position += new Vector3((xEnd - xStart) / 2 + (-1 * (xEnd - xStart)), 0, 0);
        zSeg1.transform.position = new Vector3(breadboard.transform.position.x + xStart, yDimen, breadboard.transform.position.z + (zDimen - zStart));
        zSeg1.transform.position += new Vector3((xEnd - xStart) + (-1 * (xEnd - xStart)), 0, ((zEnd - zStart) / 2) * -1);
        zSeg2.transform.position = new Vector3(breadboard.transform.position.x + xEnd, yDimen, breadboard.transform.position.z + (zDimen - zStart));
        zSeg2.transform.position += new Vector3((xEnd - xStart) + (-1 * (xEnd - xStart)), 0, ((zEnd - zStart) / 2) * -1);

        if (displayBaseArrows)
        {
            arrow.transform.position = new Vector3(breadboard.transform.position.x + xStart + (xEnd - xStart) / 2, 0.04f + yDimen, breadboard.transform.position.z + ((zDimen - zStart) + ((zDimen - zEnd))) / 2);
            arrow.transform.localScale = new Vector3(1f, 1f, 1f);
            arrow.transform.position += new Vector3(0, Mathf.Sin(Time.time * 3f), 0) * 0.005f;
        }


        // Scale the segment sides using the given start and end values (connects the 4 sides to create a rectangular area) 
        float scale;
        
        if (xEnd - xStart > zEnd - zStart)
        {
            scale = (zEnd - zStart) / 25;
        } else
        {
            scale = (xEnd - xStart) / 25;
        }
        
        xSeg1.transform.localScale = new Vector3((xEnd - xStart) + scale + 0.0005f, 0.0005f, scale + 0.0005f);
        xSeg2.transform.localScale = new Vector3((xEnd - xStart) + scale + 0.0005f, 0.0005f, scale + 0.0005f);
        zSeg1.transform.localScale = new Vector3(scale + 0.0005f, 0.0005f, (zEnd - zStart) + scale + 0.0005f);
        zSeg2.transform.localScale = new Vector3(scale + 0.0005f, 0.0005f, (zEnd - zStart) + scale + 0.0005f);
    }

    // Update color changes the color of the segment sides (called in Placer using the color buttons)
    public void updateColor(Material color)
    {
        xSeg1.GetComponent<MeshRenderer>().material = color;
        xSeg2.GetComponent<MeshRenderer>().material = color;
        zSeg1.GetComponent<MeshRenderer>().material = color;
        zSeg2.GetComponent<MeshRenderer>().material = color;
        shaft.GetComponent<Renderer>().material = color;
        cone.GetComponent<Renderer>().material = color;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIText : MonoBehaviour
{
    /// <summary>
    /// The text to display
    /// </summary>
    public string DisplayText;
    /// <summary>
    /// First Tank object that can be used to have its name displayed in color
    /// </summary>
    public Tank t1;
    /// <summary>
    /// Second Tank object that can be used to have its name displayed in color
    /// </summary>
    public Tank t2;

    /// <summary>
    /// The text to display on the main text object, same as outlineText if no color is used
    /// </summary>
    private string mainString;
    /// <summary>
    /// The text to display on the outline text objects
    /// </summary>
    private string outlineString;
    /// <summary>
    /// Used to store the index of the mainString where color tags begin. This saves performance when adjusting the alpha values when fading
    /// </summary>
    private List<int> colorStartIndices;
    private List<Text> outline;
    private Text mainText;

    private void Start()
    {
        outline = new List<Text>();
        colorStartIndices = new List<int>();

        for (int i = 0; i < transform.childCount - 1; i++)
        {
            outline.Add(transform.GetChild(i).GetComponent<Text>());
        }
        mainText = transform.GetChild(transform.childCount - 1).GetComponent<Text>();

        //Set the text to display, which will stay the same if color is not used
        outlineString = DisplayText;
        mainString = DisplayText;

        //Fill appropriate format fields with colored tank names if tank objects have been assigned
        if(t1 != null)
        {
            if(t2 != null)
            {
                outlineString = string.Format(DisplayText, t1.PlayerName, t2.PlayerName);
                mainString = string.Format(DisplayText, StringToColorText(t1.color, t1.PlayerName), StringToColorText(t2.color, t2.PlayerName));
            }
            else
            {
                outlineString = string.Format(DisplayText, t1.PlayerName);
                mainString = string.Format(DisplayText, StringToColorText(t1.color, t1.PlayerName));
            }
            CalculateColorIndices();
        }

        foreach(Text t in outline)
            t.text = outlineString;
        mainText.text = mainString;
    }

    public void UpdateTextAlpha(float a)
    {
        Color c;

        for (int i = 0; i < transform.childCount; i++)
        {
            c = transform.GetChild(i).GetComponent<Text>().color;
            c.a = a;
            transform.GetChild(i).GetComponent<Text>().color = c;
        }

        //Now change the alpha value in the color tag to fade the text properly
        if (colorStartIndices.Count == 0)
            return;

        string alphaHex = ((byte)(a * 255)).ToString("X2").ToLower();
        char[] updatedString = mainString.ToCharArray();

        foreach (int i in colorStartIndices)
        {
            updatedString[i + 7] = alphaHex[0];
            updatedString[i + 8] = alphaHex[1];
        }

        mainText.text = new string(updatedString);
    }

    private void CalculateColorIndices()
    {
        for(int i = 0; i < mainString.Length; i++)
        {
            if (mainString[i] == '#')
                colorStartIndices.Add(i);
        }
    }

    private string StringToColorText(Color32 c, string text)
    {
        string result = "<color=#";

        result += c.r.ToString("X2").ToLower();
        result += c.g.ToString("X2").ToLower();
        result += c.b.ToString("X2").ToLower();
        result += c.a.ToString("X2").ToLower();
        result += ">" + text + "</color>";

        return result;
    }
}

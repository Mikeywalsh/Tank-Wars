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

    private List<Text> outline;
    private Text mainText;

    void Start()
    {
        outline = new List<Text>();
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
        }

        foreach(Text t in outline)
            t.text = outlineString;
        mainText.text = mainString;
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// A singleton which controls the kill feed located at the top right of the screen ingame
/// </summary>
public class KillFeedController : MonoBehaviour {

    public static KillFeedController controller;

    public float startFadeTime;
    public float fadeDuration;

    private bool startedFade;
    private List<KillFeedContainer> displayedKills;

	void Start () {
        displayedKills = new List<KillFeedContainer>();
        controller = this;
        Add("{0} IS THE KING", GameObject.Find("Player").transform.FindChild("Tank").GetComponent<Tank>());
        Add("{0} SUCKS", GameObject.Find("Simple AI Player").transform.FindChild("Tank").GetComponent<Tank>());
        Add("{0} IS THE BEST", GameObject.Find("Player").transform.FindChild("Tank").GetComponent<Tank>());
    }
	
	void Update () {
		foreach(KillFeedContainer c in displayedKills)
        {
            c.ActiveTime += Time.deltaTime;

            if(c.ActiveTime >= startFadeTime && !c.StartedFade)
            {
                iTween.ValueTo(c.TextObject, iTween.Hash("from", 1, "to", 0, "time", fadeDuration, "onupdate", "UpdateTextAlpha"));
                c.StartedFade = true;
            }

            if (c.ActiveTime >= startFadeTime + fadeDuration)
            {
                Destroy(c.TextObject);
                //displayedKills.Remove(c);
            }
        }
	}

    public static void Add(string killText, Tank t)
    {
        GameObject u = Instantiate(Resources.Load("Color UI Text"), controller.transform) as GameObject;
        Vector2 textPosition = u.GetComponent<RectTransform>().anchoredPosition;
        textPosition -= new Vector2(0, 30 * controller.displayedKills.Count);

        u.GetComponent<RectTransform>().anchoredPosition = textPosition;
        u.GetComponent<UIText>().t1 = t;
        u.GetComponent<UIText>().DisplayText = killText;
        controller.displayedKills.Add(new KillFeedContainer(0, u));
    }



}

/// <summary>
/// Class used to contain information about an element in the kill feed
/// </summary>
public class KillFeedContainer
{
    public float ActiveTime;
    public bool StartedFade;
    public GameObject TextObject;

    public KillFeedContainer(float activeTime, GameObject textObject)
    {
        ActiveTime = activeTime;
        TextObject = textObject;
        StartedFade = false;
    }
}

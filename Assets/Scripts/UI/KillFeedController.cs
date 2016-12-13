using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// A singleton which controls the kill feed located at the top right of the screen ingame
/// </summary>
public class KillFeedController : MonoBehaviour {

    public static KillFeedController controller;

    public float addWaitTime;
    public float startFadeTime;
    public float fadeDuration;

    private bool startedFade;
    private float lastAddedTime;
    private List<KillFeedQueueContainer> queuedKills;
    private List<KillFeedContainer> displayedKills;

	void Start () {
        queuedKills = new List<KillFeedQueueContainer>();
        displayedKills = new List<KillFeedContainer>();

        controller = this;
        //AddToQueue("{0} IS THE KING", GameObject.Find("Player").transform.FindChild("Tank").GetComponent<Tank>());
        //AddToQueue("{0} SUCKS", GameObject.Find("Simple AI Player").transform.FindChild("Tank").GetComponent<Tank>());
        //AddToQueue("{0} IS THE BEST", GameObject.Find("Player").transform.FindChild("Tank").GetComponent<Tank>());
    }
	
	void Update () {
        if(Time.time - lastAddedTime > addWaitTime)
        {
            if (QueuedKills.Count > 0)
            {
                Add(QueuedKills[0]);
                QueuedKills.RemoveAt(0);
                lastAddedTime = Time.time;
            }
        }

		for(int i = 0; i < DisplayedKills.Count; i++)
        {
            DisplayedKills[i].ActiveTime += Time.deltaTime;

            if(DisplayedKills[i].ActiveTime >= startFadeTime && !DisplayedKills[i].StartedFade)
            {
                iTween.ValueTo(DisplayedKills[i].TextObject, iTween.Hash("from", 1, "to", 0, "time", fadeDuration, "onupdate", "UpdateTextAlpha"));
                DisplayedKills[i].StartedFade = true;
            }

            if (DisplayedKills[i].ActiveTime >= startFadeTime + fadeDuration)
            {
                Destroy(DisplayedKills[i].TextObject);
                displayedKills.RemoveAt(0);
            }
        }
	}

    public static void AddToQueue(string killText, Tank t)
    {
        QueuedKills.Add(new KillFeedQueueContainer(killText, t));
    }

    public static void AddToQueue(string killText, Tank t1, Tank t2)
    {
        QueuedKills.Add(new KillFeedQueueContainer(killText, t1, t2));
    }

    public static void Add(KillFeedQueueContainer nextKill)
    {
        GameObject u = Instantiate(Resources.Load("Color UI Text"), controller.transform) as GameObject;
        Vector2 textPosition = u.GetComponent<RectTransform>().anchoredPosition;

        //Move all existing killcam text objects out of the way when spawning a new one
        for(int i = 0; i < DisplayedKills.Count; i++)
        {
            float yPos = DisplayedKills[i].TextObject.GetComponent<RectTransform>().anchoredPosition.y;
            iTween.ValueTo(DisplayedKills[i].TextObject, iTween.Hash("from", yPos, "to", yPos - 30, "time", .2f, "onupdate", "UpdateAnchoredPosition"));
        }

        //Populate the created text object with the required information
        u.GetComponent<RectTransform>().anchoredPosition = textPosition;
        u.GetComponent<UIText>().t1 = nextKill.Tank1;
        if(nextKill.Tank2 != null)
            u.GetComponent<UIText>().t2 = nextKill.Tank2;
        u.GetComponent<UIText>().DisplayText = nextKill.KillText;
        u.GetComponent<UIText>().DisplayDelay = .2f;
        DisplayedKills.Add(new KillFeedContainer(0, u));
    }

    private static List<KillFeedContainer> DisplayedKills
    {
        get { return controller.displayedKills; }
    }

    private static List<KillFeedQueueContainer> QueuedKills
    {
        get { return controller.queuedKills; }
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

/// <summary>
/// Struct used to contain information about an element queued to be displayed in the kill feed
/// </summary>
public struct KillFeedQueueContainer
{
    public string KillText;
    public Tank Tank1;
    public Tank Tank2;

    public KillFeedQueueContainer(string killText, Tank t1)
    {
        KillText = killText;
        Tank1 = t1;
        Tank2 = null;
    }

    public KillFeedQueueContainer(string killText, Tank t1, Tank t2)
    {
        KillText = killText;
        Tank1 = t1;
        Tank2 = t2;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

sealed public class DisposableText : MonoBehaviour {

    public string text;
    public Color color;
    public float activeTime;
    private float x;

    void Start() {
        GetComponent<FloatingText>().Initialise(text, color, 60);
        iTween.MoveBy(gameObject, iTween.Hash("y", 1, "time", activeTime, "easetype", "linear", "oncomplete", "DestroyText"));
        iTween.ValueTo(gameObject, iTween.Hash("from", 1, "to", 0, "time", activeTime, "onupdate", "UpdateTextAlpha"));        
    }

    void UpdateTextAlpha(float a)
    {
        Color c = GetComponent<TextMesh>().color;
        c.a = a;
        GetComponent<TextMesh>().color = c;

        for (int i = 0; i < transform.childCount; i++)
        {
            c = transform.GetChild(i).GetComponent<TextMesh>().color;
            c.a = a;
            transform.GetChild(i).GetComponent<TextMesh>().color = c;
        }
    }

    private void DestroyText()
    {
        Destroy(gameObject);
    }
}

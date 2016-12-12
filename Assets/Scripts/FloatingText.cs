using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Used to set text of a TextMesh and allow them to be sorted in layers in 2D games as if they were sprites
/// </summary>
public class FloatingText : MonoBehaviour {

	public void Initialise(string text, Color textColor, int sortingOrder)
    {
        //Set the actual text
        GetComponent<TextMesh>().color = textColor;

        //Set the text mesh to be in the default sorting layer at order 50
        GetComponent<MeshRenderer>().sortingLayerName = "Default";
        GetComponent<MeshRenderer>().sortingOrder = sortingOrder;
        GetComponent<TextMesh>().text = text;

        //Now do the same for any children meshes
        for(int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<MeshRenderer>().sortingLayerName = "Default";
            transform.GetChild(i).GetComponent<MeshRenderer>().sortingOrder = sortingOrder - 1;
            transform.GetChild(i).GetComponent<TextMesh>().text = text;
        }
    }
}

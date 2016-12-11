using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Allows TextMeshes to be sorted in layers in 2D games as if they were sprites
/// </summary>
public class TextMeshLayerControl : MonoBehaviour {

	private void Awake()
    {
        //Set the text to be in the default sorting layer at order 50
        GetComponent<MeshRenderer>().sortingLayerName = "Default";
        GetComponent<MeshRenderer>().sortingOrder = 50;

        //Now do the same for any children meshes
        for(int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<MeshRenderer>().sortingLayerName = "Default";
            transform.GetChild(i).GetComponent<MeshRenderer>().sortingOrder = 50;
        }
    }
}

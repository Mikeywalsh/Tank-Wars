using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour {

    private float zoom;
    private float vertExtent;
    private float horzExtent;
    private float mapMinX;
    private float mapMinY;
    private float mapMaxX;
    private float mapMaxY;

	void Start () {
        zoom = GetComponent<Camera>().orthographicSize;

        //Ensure that camera stays in the bounds of the current map
        vertExtent = GetComponent<Camera>().orthographicSize;
        horzExtent = vertExtent * (Screen.width / Screen.height);
        mapMinX = horzExtent - (LevelController.MapWidth / 2.0f);
        mapMaxX = (LevelController.MapWidth / 2.0f) - horzExtent;
        mapMinY = vertExtent - (LevelController.MapHeight / 2.0f);
        mapMaxY = (LevelController.MapHeight / 2.0f) - vertExtent;
    }
	
	void Update () {
        //Find the midpoint of all active tanks
        float targetMinX = float.MaxValue;
        float targetMaxX = float.MinValue;
        float targetMinY = float.MaxValue;
        float targetMaxY = float.MinValue;

        foreach(Tank t in Tank.activeTanks)
        {
            if (t.body.position.x < targetMinX)
                targetMinX = t.body.position.x;
            if (t.body.position.x > targetMaxX)
                targetMaxX = t.body.position.x;
            if (t.body.position.y < targetMinY)
                targetMinY = t.body.position.y;
            if (t.body.position.y > targetMaxY)
                targetMaxY = t.body.position.y;
        }

        Vector2 followTarget = new Vector2((targetMinX + targetMaxX) / 2, (targetMinY + targetMaxY) / 2);

        //Loosely follow the midpoint of all active tanks
        Vector3 pos = transform.position;
        if (pos.x - followTarget.x <= -1.5f)
            pos.x = followTarget.x - 1.5f;
        if (pos.x - followTarget.x >= 1.5f)
            pos.x = followTarget.x + 1.5f;
        if (pos.y - followTarget.y <= -1.5f)
            pos.y = followTarget.y - 1.5f;
        if (pos.y - followTarget.y >= 1.5f)
            pos.y = followTarget.y + 1.5f;

        //Debug.Log("---");
        //Debug.Log(mapMinX.ToString());
        //Debug.Log(mapMaxX.ToString());
        //Debug.Log(mapMinY.ToString());
        //Debug.Log(mapMaxY.ToString());

        //pos.x = Mathf.Clamp(pos.x, mapMinX, mapMaxX);
        //pos.y = Mathf.Clamp(pos.y, mapMinY, mapMaxY);
        transform.position = Vector3.Lerp(transform.position,pos,Time.deltaTime * 5);

        //Allow camera zoom control
        if (Input.GetKey(KeyCode.Minus))
            zoom -= 0.25f;
        else if(Input.GetKey(KeyCode.Plus))
            zoom += 0.25f;

        //GetComponent<Camera>().orthographicSize = Mathf.Clamp(zoom, 5, 15);
	}
}

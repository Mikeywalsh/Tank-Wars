using UnityEngine;

public class HealthBarController : MonoBehaviour
{
    public void UpdateHealth(int maxHealth, int health, int damage)
    {
        //Tween the health bar to its new position and scale if the player is taking more than 5 points of damage, otherwise just snap to new values
        if (damage > 5)
        {
            iTween.ScaleTo(transform.GetChild(0).gameObject, iTween.Hash("scale", new Vector3((float)health / maxHealth, 1, 1), "time", 0.5f, "islocal", true));
            iTween.MoveTo(transform.GetChild(0).gameObject, iTween.Hash("position", new Vector3((((float)health / maxHealth) - 1) / 2, 0, 0), "time", 0.5f, "islocal", true));
        }
        else
        {
            transform.GetChild(0).localPosition = new Vector3((((float)health / maxHealth) - 1) / 2, 0, 0);
            transform.GetChild(0).localScale = new Vector3((float)health / maxHealth, 1, 1);
        }
    }

    public void ResetHealth()
    {
        iTween.Stop(transform.GetChild(0).gameObject);
        transform.GetChild(0).localScale = Vector3.one;
        transform.GetChild(0).localPosition = Vector3.zero;
    }
}
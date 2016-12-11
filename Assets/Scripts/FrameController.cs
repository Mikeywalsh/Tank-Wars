using UnityEngine;

/// <summary>
/// Controls the appearance of a tanks frame, including it's healthbar and name display
/// </summary>
public class FrameController : MonoBehaviour
{
    private Transform healthBarIndicator;

    void Start()
    {
        healthBarIndicator = transform.Find("Health Bar/Indicator");
    }

    public void UpdateHealth(int maxHealth, int health, int damage)
    {
        //Tween the health bar to its new position and scale if the player is taking more than 5 points of damage, otherwise just snap to new values
        if (damage > 5)
        {
            iTween.ScaleTo(healthBarIndicator.gameObject, iTween.Hash("scale", new Vector3((float)health / maxHealth, 1, 1), "time", 0.5f, "islocal", true));
            iTween.MoveTo(healthBarIndicator.gameObject, iTween.Hash("position", new Vector3((((float)health / maxHealth) - 1) / 2, 0, 0), "time", 0.5f, "islocal", true));
        }
        else
        {
            healthBarIndicator.localPosition = new Vector3((((float)health / maxHealth) - 1) / 2, 0, 0);
            healthBarIndicator.localScale = new Vector3((float)health / maxHealth, 1, 1);
        }
    }

    public void ResetHealth()
    {
        iTween.Stop(healthBarIndicator.gameObject);
        healthBarIndicator.localScale = Vector3.one;
        healthBarIndicator.localPosition = Vector3.zero;
    }
}
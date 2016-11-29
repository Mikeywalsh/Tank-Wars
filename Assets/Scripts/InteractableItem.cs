using UnityEngine;
using System.Collections;

public class InteractableItem : Entity {

    public Rigidbody2D body;
    public Vector2 direction;

    protected override void Start ()
    {
        base.Start();
        body = GetComponent<Rigidbody2D>();
    }
	
	protected override void FixedUpdate ()
    {
        base.FixedUpdate();

        if (direction != Vector2.zero)
        {
            body.MovePosition(body.position + (direction.normalized * 1.15f * Time.deltaTime));
        }
    }

    protected void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.name.Contains("Destroyer"))
            DestroyEntity();
    }
}

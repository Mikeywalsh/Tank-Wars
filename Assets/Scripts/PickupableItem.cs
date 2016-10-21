using UnityEngine;
sealed public class PickupableItem : Entity
{
    public Rigidbody2D body;
    public Vector2 direction;
    public int itemID;

    protected override void Start()
    {
        base.Start();
        body = GetComponent<Rigidbody2D>();
        itemID = Random.Range(0, 4);
    }

    private void FixedUpdate()
    {
        if (direction != Vector2.zero)
        {
            body.MovePosition(body.position + (direction.normalized * 1.15f * Time.deltaTime));
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.name.Contains("Destroyer"))
            DestroyEntity();
    }
}
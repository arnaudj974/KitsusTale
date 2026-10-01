using UnityEngine;

public class Frog : MonoBehaviour
{
    [SerializeField] private Sprite spJump;
    [SerializeField] private Sprite spFall;
    [SerializeField] private Sprite spIdle;
    private Rigidbody2D body;
    private SpriteRenderer sp;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        body = GetComponentInParent<Rigidbody2D>();
        sp = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if(body.linearVelocityY>= 0.2f)
        {
            sp.sprite = spJump;
        }
        else if (body.linearVelocityY <= -0.2f)
        {
            sp.sprite = spFall;
        }
        else
        {
            sp.sprite = spIdle;
        }
    }
}

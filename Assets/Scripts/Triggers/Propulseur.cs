using Unity.VisualScripting;
using UnityEngine;

public class Propulseur : MonoBehaviour
{   
    private Animator anim;
    [SerializeField] private float boost=2f;
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            anim.Play("Propulseur");
            collision.GetComponent<Player>().Bounce(boost); // inflige un rebond au joueur
        }
    }
}

using UnityEngine;

public class Stalactite : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;
    private Player player;
    private float distance;
    void Start()
    {
        // Trouve le player du niveau
        player = FindAnyObjectByType<Player>();

        if (!player)
        {
            // S'il n'y a pas de player dans le niveau, envoie une erreur et désactive le script
            Debug.LogError("MISING PLAYER");
            enabled = false;
        }
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        anim.enabled = false;
    }


    void Update()
    {
        distance = Vector2.Distance(transform.position, player.transform.position);
        CheckDistancePlayer();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<Player>()?.Kill(); //inflige des dégats au joueur
            Destroy(gameObject); // détruit la stalactite
        }
    }

    private void CheckDistancePlayer()
    {
        if (distance < 10f)
        {
            if (anim.enabled == false) { anim.enabled = true; }
            if (distance < 5f)
            {
                rb.bodyType = RigidbodyType2D.Dynamic;
                rb.AddForceY(-3f);
            }
        }
    }
}

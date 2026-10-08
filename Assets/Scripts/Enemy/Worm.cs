using System.Collections;
using UnityEngine;

public class Worm : MonoBehaviour
{
    [SerializeField] private Vector2[] patrol_pos;
    [SerializeField] private float speed = 5;
    [SerializeField] private float distanceAttack = 2f;
    [SerializeField] private GameObject kill_fX;
    [SerializeField] private ParticleSystem ground_fX;
    private Animator anim;
    private SpriteRenderer sp;
    private CapsuleCollider2D collider;
    private int target_patrol_point_index;
    private float distance_before_point_change = 0.2f;
    private Vector2 move_dir;
    private Vector2 start_pos;
    private Player player;
    private float distancePlayer;
    private bool isAttacking = false;
    private ParticleSystem groundFx;

    private void Awake()
    {
        groundFx = Instantiate(ground_fX, transform.position + Vector3.up * 0.5f, Quaternion.identity, transform);
    }

    void Start()
    {
        // Trouve le player du niveau
        player = FindAnyObjectByType<Player>();
        anim = GetComponentInChildren<Animator>();
        collider = GetComponent<CapsuleCollider2D>();
        sp = GetComponent<SpriteRenderer>();
        sp.sprite = null;
        start_pos = transform.position;
        collider.enabled = false;
    }

    void Update()
    {
        var emission = groundFx.emission;
        if (isAttacking)
        {
            emission.enabled = false;
            return; 
        }
        else
        {
            emission.enabled = true;
        }
        // Vérifie la distance à la target actuelle
        CheckDistanceToTarget();

        // Calcule la direction vers le prochain point
        Vector2 dir = patrol_pos[target_patrol_point_index] + start_pos - (Vector2)transform.position;

        // Applique la vitesse à la direction
        move_dir = dir.normalized * speed;
        transform.position += (Vector3)move_dir * Time.deltaTime;

        distancePlayer = Vector2.Distance(transform.position, player.transform.position);
        /* if (ground_fX)
         {
             Instantiate(ground_fX, transform.position + Vector3.up * 0.5f, Quaternion.identity);
         }*/

        if (distancePlayer < Random.Range(distanceAttack - 0.5f, distanceAttack + 0.5f)) 
        {
            StartCoroutine(Attack());
        }
    }

    IEnumerator Attack()
    {
        transform.position = transform.position + Vector3.up;
        isAttacking = true;
        collider.enabled = true;
        anim.SetTrigger("Attack");
        yield return new WaitForSeconds(2f);
        collider.enabled = false;
        isAttacking = false;
        transform.position = transform.position + Vector3.down;
    }

    void CheckDistanceToTarget()
    {
        if (Vector2.Distance(transform.position, patrol_pos[target_patrol_point_index] + start_pos) <= distance_before_point_change)
        {
            // Si la target est assez proche, passe à la target suivante dans la liste
            target_patrol_point_index = (target_patrol_point_index + 1) % patrol_pos.Length;
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            // L'objet qui est entré dans le trigger est le joueur

            // Récupère le script du joueur
            Player player = collision.GetComponent<Player>();
            // Vérifie s'il est au sol
            if (!player.grounded || player.dashing)// S'il est dans les airs ou en train de dasher, il a écrasé l'ennemi
            {
                // Tue l'ennemi
                Kill();
                // Ajoute une pièce et du score au player
                player.AddCoin();
                // Fait rebondir le joueur
                player.Bounce(1f);
            }
            else
            {
                // Le joueur n'était pas en saut, tue le joueur
                player.Kill();
            }
        }
    }
    void Kill()
    {
        // Si le prefab fx de mort existe, spawn une instance sur l'ennemi
        if (kill_fX)
        {
            Instantiate(kill_fX, transform.position + Vector3.up * 0.5f, Quaternion.identity);
        }

        // Détruit l'ennemi
        Destroy(gameObject);
    }

    // Fonction appelée par Unity dans l'éditeur pour afficher des gizmos supplémentaires
    void OnDrawGizmos()
    {
        // Passe la couleur des gizmos en rouge
        Gizmos.color = Color.red;
        // Pour chaque point, dessine une sphère à la position
        for (int i = 0; i < patrol_pos.Length; i++)
        {
            Gizmos.DrawWireSphere(transform.position + (Vector3)patrol_pos[i], distance_before_point_change);
        }
    }
}

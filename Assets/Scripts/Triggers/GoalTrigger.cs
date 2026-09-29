using UnityEngine;

public class GoalTrigger : MonoBehaviour
{
    [SerializeField] private string player_layer;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer(player_layer))
        {
            // Si l'objet qui entre en collision est sur le layer du joueur
            // Passe au level suivant
            Level.current_level.NextLevel();
        }
    }
}

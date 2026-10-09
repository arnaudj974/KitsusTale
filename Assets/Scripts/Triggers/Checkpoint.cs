using UnityEngine;

public class Checkpoint : MonoBehaviour
{

    [SerializeField] private string player_layer;
    [SerializeField] private Transform spawn_point;
    [SerializeField] private ParticleSystem particles;
    [SerializeField] private AudioSource audio;
    private bool isActivated = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer(player_layer))
        {
            // Si l'objet qui entre en collision est sur le layer du joueur
            // Indique au niveau actuel la nouvelle position du checkpoint
            Level.current_level.NewCheckpoint(spawn_point.position);
            if (audio && !isActivated)
            {
                audio.Play();
                isActivated = true;
            }
            var tempParticles = particles.main;
            tempParticles.startColor = new ParticleSystem.MinMaxGradient(Color.green);
        }
    }
}

using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] private int score;
    [SerializeField] private int life;
    [SerializeField] private int maxLife;
    private PlayerController _player_control;
    private PlayerAnim _player_anim;
    public UnityEvent<int> OnScoreChanged;
    public UnityEvent<int, int> OnLifeChanged;
    public bool dashing { get { return _player_control.dashing; } }
    public bool grounded { get { return _player_control.grounded; } }
    private bool is_respawning = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Récupère les scripts du player
        _player_control = GetComponent<PlayerController>();
        _player_anim = GetComponent<PlayerAnim>();
        if (Level.current_level.next_scene_index != 1) { LoadInfos(); } //charge les infos si le player ne commence pas au niveau 1
    }

    public void Kill()
    {
        if (is_respawning) return; //éviter de prendre plusieurs points de dégats en même temps 
        is_respawning = true;
        // Joue l'animation de mort
        _player_control.SetFreeze(true);
        _player_anim.SetDeathAnim(true);
        RemoveLife(1);
        StartCoroutine(Respawn());
    }

    public void AddCoin()
    {
        score += 100;
        if (score >= 10000)
        {
            score -= 10000;
            RemoveLife(-1); // Ajoute une vie si le score atteint 10000
        }
        OnScoreChanged?.Invoke(score);
    }
    public void RemoveLife(int i)
    {
        life = life - i > maxLife ? maxLife : life - i; // S'assure que la vie ne dépasse pas le maximum
        OnLifeChanged?.Invoke(life, maxLife);
        SaveInfos();
        if (life <= 0)
        {
            GameManager.instance.ReloadScene();
        }
    }
    public void Bounce(float boost)
    {
        _player_control.ForceJump(boost);
    }

    public void OnPauseInput(InputAction.CallbackContext ctx)
    {
        if (ctx.performed)
        {
            Level.current_level.PauseLevel();
        }
    }

    public IEnumerator Respawn()
    {
        yield return new WaitForSeconds(0.5f);

        GameManager.instance.screen_transition.Show();
        yield return new WaitForSeconds(GameManager.instance.screen_transition.transition_duration); // Attend que l'écran termine son fade in

        // Charge le checkpoint
        Level.current_level.LoadCheckpoint();

        // Arrête l'animation de mort
        _player_anim.SetDeathAnim(false);


        GameManager.instance.screen_transition.Hide();
        yield return new WaitForSeconds(GameManager.instance.screen_transition.transition_duration); // Attend que l'écran termine son fade in


        // Réactive le contrôle
        _player_control.SetFreeze(false);
        is_respawning = false;
    }

    public void Teleport(Vector2 position)
    {
        transform.position = position;
    }

    public void SaveInfos()
    {
        PlayerPrefs.SetInt("player_life", life);
        PlayerPrefs.SetInt("player_score", score);
        PlayerPrefs.Save();
    }

    public void LoadInfos()
    {
        life = PlayerPrefs.GetInt("player_life", 3);
        score = PlayerPrefs.GetInt("player_score", 0);
        if (life == 0)
        {
            life = 3;
            score = 0;
        }
        OnLifeChanged?.Invoke(life, maxLife);
        OnScoreChanged?.Invoke(score);
    }
}

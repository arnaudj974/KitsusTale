using System;
using UnityEngine;

public class AnimationFunction : MonoBehaviour
{
    [SerializeField] private AudioSource step_audiosource;

    public void PlayStep()
    {
        if (!step_audiosource) return;

        step_audiosource.Play();
    }
}

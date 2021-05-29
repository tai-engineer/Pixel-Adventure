using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goomba : MonoBehaviour
{
    [Space]
    [Header("Take Damage")]
    [SerializeField] AudioClip DeadSound = default;
    [SerializeField] string deadParameter = "";

    EnemyController _controller;
    AudioSource _audioSource;
    Animator _animator;

    public bool IsDead { get; private set; }
    void Awake()
    {
        _controller = GetComponent<EnemyController>();
        _audioSource = GetComponent<AudioSource>();
        _animator = GetComponent<Animator>();
    }

    void Update()
    {
        _controller.Patrol();
        _controller.TouchAttack();
    }

    // Call by TakeDamage event
    public void OnTakeDamage()
    {
        IsDead = true;
        _animator.SetBool(deadParameter, true);
        _audioSource.clip = DeadSound;
        _audioSource.Play();
    }
}

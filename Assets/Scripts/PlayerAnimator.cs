using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private const string IS_WALKING = "IsWalking";
    private Animator animator;

    [SerializeField] private Player player;

    // Awake方法在脚本实例被加载时调用，比Start方法更早执行
    private void Awake()
    {
        animator = GetComponent<Animator>();

    }

    private void Update()
    {
        animator.SetBool(IS_WALKING, player.IsWalking());
    }
}

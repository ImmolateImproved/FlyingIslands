using UnityEngine;

public class RobotShpereAnimator : MonoBehaviour
{
    [SerializeField]
    private Animator animator;

    private PlayerInput input;
    private ThirdPersonController controller;

    private void Awake()
    {
        input = GetComponent<PlayerInput>();
        controller = GetComponent<ThirdPersonController>();
    }

    private void Update()
    {
        var walk = input.move != Vector2.zero && controller.Grounded;

        animator.SetBool("Walk_Anim", walk);
    }
}
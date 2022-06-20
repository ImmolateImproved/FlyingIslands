using UnityEngine;

public class HumanoidAnimator : MonoBehaviour
{
    [SerializeField]
    public ThirdPersonController controller;

    [SerializeField]
    public PlayerInput input;

    [SerializeField]
    public Animator animator;

    [SerializeField]
    public float moveAnimationBlendSpeed;

    public float animationBlend { get; set; }

    public int animIDSpeed { get; private set; }
    public int animIDStrafe { get; private set; }
    public int animIDGrounded { get; private set; }
    public int animIDJump { get; private set; }
    public int animIDFreeFall { get; private set; }

    private void Awake()
    {
        AssignAnimationIDs();
    }

    private void Update()
    {
        MoveAnimation();
        JumpAnimation();
        GroundedAnimation();
    }

    private void AssignAnimationIDs()
    {
        animIDSpeed = Animator.StringToHash("Speed");
        animIDStrafe = Animator.StringToHash("Strafe");
        animIDGrounded = Animator.StringToHash("Grounded");
        animIDJump = Animator.StringToHash("Jump");
        animIDFreeFall = Animator.StringToHash("FreeFall");
    }

    private void MoveAnimation()
    {
        animationBlend = Mathf.MoveTowards(animationBlend, controller.Speed, moveAnimationBlendSpeed * Time.deltaTime);
        animator.SetFloat(animIDSpeed, animationBlend);

        if (input.combatState)
        {
            animator.SetFloat(animIDStrafe, input.move.x);
        }
    }

    private void JumpAnimation()
    {
        animator.SetBool(animIDJump, controller.Jump);
        animator.SetBool(animIDFreeFall, controller.FreeFall);
    }

    private void GroundedAnimation()
    {
        animator.SetBool(animIDGrounded, controller.Grounded);
    }
}
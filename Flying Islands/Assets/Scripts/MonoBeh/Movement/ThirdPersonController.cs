using Unity.Physics;
using Unity.Physics.Authoring;
using UnityEngine;

public class ThirdPersonController : MonoBehaviour
{
    private CharacterController controller;
    private PlayerInput input;
    private Transform _mainCamera;

    public Transform platformCheck;

    public PhysicsCategoryTags belongsTo;
    public PhysicsCategoryTags collidesWith;

    public CollisionFilter platformLayer
    {
        get
        {
            return new CollisionFilter
            {
                BelongsTo = belongsTo.Value,
                CollidesWith = collidesWith.Value
            };
        }
    }

    public float platformCheckDistance;

    public bool disableMovement;

    [Header("Player")]
    [Tooltip("Move speed of the character in m/s")]
    public float MoveSpeed = 2.0f;
    [Tooltip("Sprint speed of the character in m/s")]
    public float SprintSpeed = 5.335f;
    [Tooltip("How fast the character turns to face movement direction")]
    [Range(0.0f, 0.3f)]
    public float RotationSmoothTime = 0.12f;

    [Space(10)]
    [Tooltip("The height the player can jump")]
    public float JumpHeight = 1.2f;
    [Tooltip("The character uses its own gravity value. The engine default is -9.81f")]
    public float Gravity = -15.0f;

    [Space(10)]
    [Tooltip("Time required to pass before being able to jump again. Set to 0f to instantly jump again")]
    public float JumpTimeout = 0.50f;
    [Tooltip("Time required to pass before entering the fall state. Useful for walking down stairs")]
    public float FallTimeout = 0.15f;

    public float jumpDelay;

    [Header("Player Grounded")]
    [Tooltip("If the character is grounded or not. Not part of the CharacterController built in grounded check")]
    public bool Grounded = true;
    [Tooltip("Useful for rough ground")]
    public float GroundedOffset = -0.14f;
    [Tooltip("The radius of the grounded check. Should match the radius of the CharacterController")]
    public float GroundedRadius = 0.28f;
    [Tooltip("What layers the character uses as ground")]
    public LayerMask GroundLayers;

    // player
    private float _speed;
    private float _rotationVelocity;
    private float _verticalVelocity;
    private float _terminalVelocity = 53.0f;

    // timeout deltatime
    private float jumpTimeoutDelta;
    private float fallTimeoutDelta;

    private float jumpDelayDelta;
    private bool lastGroundedStatus;

    public Vector3 TargetDirection { get; private set; }

    public bool Jump { get; private set; }
    public bool FreeFall { get; private set; }

    public float Speed => _speed;

    private void Awake()
    {
        controller = GetComponent<CharacterController>();

        input = GetComponent<PlayerInput>();

        // get a reference to our main camera
        if (_mainCamera == null)
        {
            _mainCamera = Camera.main.transform;
        }
    }

    private void Start()
    {
        // reset our timeouts on start
        jumpTimeoutDelta = JumpTimeout;
        fallTimeoutDelta = FallTimeout;
    }

    private void Update()
    {
        JumpAndGravity();
        GroundedCheck();
        Move();
    }

    private void Move()
    {
        // set target speed based on move speed, sprint speed and if sprint is pressed
        _speed = disableMovement ? 0 : GetCurrentSpeed();

        // normalise input direction
        Vector3 inputDirection = new Vector3(input.move.x, 0.0f, input.move.y).normalized;

        TargetDirection = Rotation(inputDirection);

        //slopeBahaviour.CalculateSlopeDirection(ref targetDirection, ref onSlope);

        var velocity = TargetDirection * (_speed) + new Vector3(0.0f, _verticalVelocity, 0.0f);

        controller.Move(velocity * Time.deltaTime);
    }

    public void Move(Vector3 velocity)
    {
        velocity.y = _verticalVelocity;
        controller.Move(velocity * Time.deltaTime);
    }

    private void GroundedCheck()
    {
        // set sphere position, with offset
        var spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z);
        var groundCheckResult = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers, QueryTriggerInteraction.Ignore);

        if (lastGroundedStatus && !groundCheckResult)
        {
            jumpDelayDelta = jumpDelay;
        }

        lastGroundedStatus = groundCheckResult;

        if (jumpDelayDelta <= 0)
        {
            Grounded = groundCheckResult;
        }
        else
        {
            jumpDelayDelta -= Time.deltaTime;
        }

        Grounded = groundCheckResult || Grounded;
    }

    private float GetCurrentSpeed()
    {
        // set target speed based on move speed, sprint speed and if sprint is pressed
        var currentSpeed = input.sprint ? SprintSpeed : MoveSpeed;

        // note: Vector2's == operator uses approximation so is not floating point error prone, and is cheaper than magnitude
        // if there is no input, set the target speed to 0
        if (input.move == Vector2.zero) currentSpeed = 0.0f;

        return currentSpeed;
    }

    private Vector3 Rotation(Vector3 inputDirection)
    {
        var moveDirection = Vector3.zero;

        if (!input.combatState)
        {
            if (input.move != Vector2.zero)
            {
                var rotationY = _mainCamera.eulerAngles.y + Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg;
                moveDirection = Quaternion.Euler(0, rotationY, 0) * Vector3.forward;

                //rotationY = Mathf.SmoothDampAngle(transform.eulerAngles.y, rotationY, ref _rotationVelocity, RotationSmoothTime);

                transform.rotation = Quaternion.Euler(0, rotationY, 0);
            }
        }
        else
        {
            moveDirection = transform.forward * inputDirection.z + transform.right * inputDirection.x;

            var rotationY = Mathf.SmoothDampAngle(transform.eulerAngles.y, _mainCamera.eulerAngles.y, ref _rotationVelocity, RotationSmoothTime);

            transform.rotation = Quaternion.Euler(0, rotationY, 0);
        }

        return moveDirection;
    }

    private void JumpAndGravity()
    {
        if (Grounded)
        {
            // reset the fall timeout timer
            fallTimeoutDelta = FallTimeout;

            Jump = false;
            FreeFall = false;

            // Jump
            if (input.jump && jumpTimeoutDelta <= 0.0f)
            {
                // the square root of H * -2 * G = how much velocity needed to reach desired height
                _verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);

                Jump = true;
            }

            // jump timeout
            if (jumpTimeoutDelta >= 0.0f)
            {
                jumpTimeoutDelta -= Time.deltaTime;
            }
        }
        else
        {
            // reset the jump timeout timer
            jumpTimeoutDelta = JumpTimeout;

            // fall timeout
            if (fallTimeoutDelta >= 0.0f)
            {
                fallTimeoutDelta -= Time.deltaTime;
            }
            else
            {
                FreeFall = true;
            }

            // if we are not grounded, do not jump
            input.jump = false;

            // apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
            if (_verticalVelocity < _terminalVelocity)
            {
                _verticalVelocity += Gravity * Time.deltaTime;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
        Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

        if (Grounded) Gizmos.color = transparentGreen;
        else Gizmos.color = transparentRed;

        // when selected, draw a gizmo in the position of, and matching radius of, the grounded collider
        Gizmos.DrawSphere(new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z), GroundedRadius);
    }
}
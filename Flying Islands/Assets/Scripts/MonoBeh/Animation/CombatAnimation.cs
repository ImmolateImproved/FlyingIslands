using Cinemachine;
using UnityEngine;

public class CombatAnimation : MonoBehaviour
{
    [SerializeField]
    private PlayerInput input;

    [SerializeField]
    private Animator animator;

    [SerializeField]
    private GameObject gun;

    [SerializeField]
    private Avatar humanoidAvatar, genericAvatar;

    private int combatStateAnimID;

    private void Awake()
    {
        InitAnimationIDs();
    }

    private void Update()
    {
        CheckCombatState();
    }

    private void InitAnimationIDs()
    {
        combatStateAnimID = Animator.StringToHash("CombatState");
    }

    private void CheckCombatState()
    {
        if (input.combatState)
        {
            animator.avatar = genericAvatar;
            animator.SetBool(combatStateAnimID, true);

            gun.SetActive(true);
        }
        else
        {
            animator.avatar = humanoidAvatar;
            animator.SetBool(combatStateAnimID, false);

            gun.SetActive(false);
        }
    }

}
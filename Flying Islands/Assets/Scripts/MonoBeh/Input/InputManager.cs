using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/InputManager")]
public class InputManager : ScriptableObject
{
    private Controls controls;

    public Controls Controls
    {
        get
        {
            controls ??= new Controls();
            return controls;
        }
    }
}
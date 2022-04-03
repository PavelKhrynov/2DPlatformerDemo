using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HeroInputReader : MonoBehaviour
{
    [SerializeField] private Hero _hero;

    private HeroInput _inputAction;

    private void Awake()
    {
        _inputAction = new HeroInput();
        _inputAction.Hero.HorizontalMovement.performed += OnHorizontalMovement;
        _inputAction.Hero.HorizontalMovement.canceled += OnHorizontalMovement;
    }

    private void OnEnable()
    {
        _inputAction.Enable();
    }

    public void OnHorizontalMovement(InputAction.CallbackContext context)
    {
        var _direction = context.ReadValue<Vector2>();
        _hero?.SetDirection(_direction);
    }
}

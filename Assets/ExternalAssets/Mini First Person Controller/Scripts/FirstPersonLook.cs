using System;
using Core.Events;
using Core.Input;
using Core.Pause;
using Reflex.Attributes;
using UnityEngine;

public class FirstPersonLook : MonoBehaviour
{
    [SerializeField] Transform character;
    [Inject] private InputManager _inputManager;
    public float sensitivity = 2;
    public float smoothing = 1.5f;

    Vector2 velocity;
    Vector2 frameVelocity;

    private bool _isActive = true;
    private void OnEnable()
    {
        EventBus.Subscribe<GamePauseEvent>(HandleGamePauseEvent);
    }

    private void HandleGamePauseEvent(GamePauseEvent e)
    {
        _isActive = !e.IsPaused;
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe<GamePauseEvent>(HandleGamePauseEvent);
    }


    void Reset()
    {
        // Get the character from the FirstPersonMovement in parents.
        character = GetComponentInParent<FirstPersonMovement>().transform;
    }

    void Start()
    {
        // Lock the mouse cursor to the game screen.
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if (!_isActive) return;
        // Get smooth velocity.
        Vector2 mouseDelta = _inputManager.MouseInput;
        Vector2 rawFrameVelocity = Vector2.Scale(mouseDelta, Vector2.one * sensitivity);
        frameVelocity = Vector2.Lerp(frameVelocity, rawFrameVelocity, 1 / smoothing);
        velocity += frameVelocity;
        velocity.y = Mathf.Clamp(velocity.y, -90, 90);

        // Rotate camera up-down and controller left-right from velocity.
        transform.localRotation = Quaternion.AngleAxis(-velocity.y, Vector3.right);
        character.localRotation = Quaternion.AngleAxis(velocity.x, Vector3.up);
    }
}

using UnityEngine;
using UnityEngine.InputSystem;

public class MoveAndRotate : MonoBehaviour
{
    public float speed = 5;

    private float _movementX;
    private float _movementY;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();
        
        _movementX = movementVector.x;
        _movementY = movementVector.y;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 movement = new Vector3(-_movementY, _movementX, 0f);
        transform.Rotate(movement * (Time.deltaTime * speed));
    }
}

using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    public GameObject player;
    private Vector3 _offset;
    private PlayerController _playerController;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _offset = transform.position - player.transform.position;
        _playerController = player.GetComponent<PlayerController>();
    }

    private void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Vector2 mousePos = Mouse.current.position.ReadValue();
            Ray rayOrigin = Camera.main.ScreenPointToRay(mousePos);
            RaycastHit hit;
            if (Physics.Raycast(rayOrigin, out hit))
            {
                _playerController.UpdatePosition(hit.point);
            }
        }
    }
    
    void LateUpdate()
    {
        transform.position = player.transform.position + _offset;
    }
}

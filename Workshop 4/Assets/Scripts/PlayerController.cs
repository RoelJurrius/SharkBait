using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 2f;
    public float yPos = 1.5f;
    private Vector3 _destination;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        var direction = _destination - transform.position;
        direction.Normalize();
        transform.Translate(direction * (speed * Time.deltaTime));
    }
    
    public void UpdatePosition(Vector3 position)
    {
        position.y = yPos;
        _destination = position;
    }
}

using UnityEngine;

public class FollowPlayerLight : MonoBehaviour
{
    public Transform player;

    void LateUpdate()
    {
        transform.position = player.position;
    }
}
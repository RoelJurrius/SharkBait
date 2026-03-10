using UnityEngine;

public class FollowPlayerLight : MonoBehaviour
{
    public Transform player;

    void LateUpdate()
    {
        if (player == null) return;

        transform.position = player.position;
    }
}
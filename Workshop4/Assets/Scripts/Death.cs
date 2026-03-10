using UnityEngine;

public class Death : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SimplePlayer player = other.GetComponent<SimplePlayer>();

            if (player != null)
            {
                player.Respawn();
            }
        }
    }
}
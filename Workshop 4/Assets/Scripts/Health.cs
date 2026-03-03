using UnityEngine;

public class Health : MonoBehaviour
{
    [Header("Health")]
    [SerializeField] private int maxHp = 100;
    public int CurrentHp { get; private set; }

    [Header("Respawn")]
    [SerializeField] private Transform respawnPointsRoot; // Empty met meerdere child points
    [SerializeField] private float respawnDelay = 0.25f;

    private Rigidbody rb;
    private Collider col;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
        CurrentHp = maxHp;
    }

    public void TakeDamage(int amount)
    {
        if (CurrentHp <= 0) return;

        CurrentHp -= amount;
        Debug.Log($"Player took {amount} dmg. HP: {CurrentHp}");

        if (CurrentHp <= 0)
            DieAndRespawn();
    }

    private void DieAndRespawn()
    {
        Debug.Log("Player dead -> respawn");

        // tijdelijk “uit” zodat er geen extra hits komen tijdens respawn
        if (col != null) col.enabled = false;
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;   // Unity 6; gebruik rb.velocity als je oudere Unity hebt
            rb.angularVelocity = Vector3.zero;
        }

        Invoke(nameof(Respawn), respawnDelay);
    }

    private void Respawn()
    {
        Transform point = PickRandomRespawnPoint();
        if (point != null)
        {
            transform.position = point.position;
            transform.rotation = point.rotation;
        }
        else
        {
            // fallback: gewoon reset boven origin
            transform.position = new Vector3(0, 1, 0);
            transform.rotation = Quaternion.identity;
        }

        CurrentHp = maxHp;

        if (col != null) col.enabled = true;
        Debug.Log("Player respawned. HP reset.");
    }

    private Transform PickRandomRespawnPoint()
    {
        if (respawnPointsRoot == null) return null;
        int count = respawnPointsRoot.childCount;
        if (count == 0) return null;

        int i = Random.Range(0, count);
        return respawnPointsRoot.GetChild(i);
    }
}
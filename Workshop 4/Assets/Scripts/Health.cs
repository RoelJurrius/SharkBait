using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int maxHp = 100;
    public int CurrentHp { get; private set; }

    private void Awake()
    {
        CurrentHp = maxHp;
    }

    public void TakeDamage(int amount)
    {
        CurrentHp -= amount;
        Debug.Log($"{name} took {amount} damage. HP: {CurrentHp}");

        if (CurrentHp <= 0)
        {
            Debug.Log($"{name} is dead.");
            // TODO: respawn / game over / disable movement, etc.
        }
    }
}
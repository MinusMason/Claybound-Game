using UnityEngine;
using UnityEngine.Events;

public class GoldManager : MonoBehaviour
{
    public static GoldManager Instance { get; private set; }

    public int Gold { get; private set; }

    public UnityEvent<int> onGoldChanged;

    // Stored so that gold stays between scenes
    private static int s_gold = 0;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else { Destroy(gameObject); return; }

        // Restore gold from previous level
        Gold = s_gold;
    }

    private void Start()
    {
        // Ensure gold display correctly on UI
        onGoldChanged?.Invoke(Gold);
    }

    public void Add(int amount)
    {
        Gold += amount;
        s_gold = Gold;
        onGoldChanged?.Invoke(Gold);
    }

    public bool TrySpend(int amount)
    {
        if (Gold < amount) return false;
        Gold  -= amount;
        s_gold  = Gold;
        onGoldChanged?.Invoke(Gold);
        return true;
    }
}

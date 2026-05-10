using UnityEngine;
using UnityEngine.Events;

public class GoldManager : MonoBehaviour
{
    public static GoldManager Instance { get; private set; }

    public int Gold { get; private set; }

    public UnityEvent<int> onGoldChanged;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void Add(int amount)
    {
        Gold += amount;
        onGoldChanged?.Invoke(Gold);
    }

    public bool TrySpend(int amount)
    {
        if (Gold < amount) return false;
        Gold -= amount;
        onGoldChanged?.Invoke(Gold);
        return true;
    }
}

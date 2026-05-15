using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance { get; private set; }

    [Header("Current Stats")]
    public int might;
    public int finesse;
    public int weird;
    public int gab;

    [Header("Roll Range")]
    public int minStatValue = 1;
    public int maxStatValue = 5;

    // Stores so they can stay between scenes
    private static int  s_might   = -1;
    private static int  s_finesse = -1;
    private static int  s_weird   = -1;
    private static int  s_gab     = -1;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else { Destroy(gameObject); return; }

        if (s_might < 0)   // First time, roll new stats
            RollStats();
        else               // Next Level save old stats
        {
            might   = s_might;
            finesse = s_finesse;
            weird   = s_weird;
            gab     = s_gab;
        }
    }

    private void RollStats()
    {
        might = Random.Range(minStatValue, maxStatValue + 1);
        finesse = Random.Range(minStatValue, maxStatValue + 1);
        weird = Random.Range(minStatValue, maxStatValue + 1);
        gab = Random.Range(minStatValue, maxStatValue + 1);

        SaveToStatic();
        Debug.Log($"Stats rolled — MIGHT:{might} FINESSE:{finesse} WEIRD:{weird} GAB:{gab}");
    }

    private void SaveToStatic()
    {
        s_might   = might;
        s_finesse = finesse;
        s_weird   = weird;
        s_gab     = gab;
    }

    public void ModifyStat(string statName, int amount)
    {
        switch (statName.ToLower())
        {
            case "might":   might   = Mathf.Max(0, might   + amount); break;
            case "finesse": finesse = Mathf.Max(0, finesse + amount); break;
            case "weird":   weird   = Mathf.Max(0, weird   + amount); break;
            case "gab":     gab     = Mathf.Max(0, gab     + amount); break;
        }
        SaveToStatic();   
    }
}

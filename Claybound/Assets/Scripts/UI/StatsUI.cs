using UnityEngine;
using TMPro;

public class StatsUI : MonoBehaviour
{
    private TextMeshProUGUI statsText;

    private void Start()
    {
        statsText = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        if (PlayerStats.Instance == null || statsText == null) return;

        statsText.text = $"MIGHT   {PlayerStats.Instance.might}\n" +
                         $"FINESSE {PlayerStats.Instance.finesse}\n" +
                         $"WEIRD   {PlayerStats.Instance.weird}\n" +
                         $"GAB     {PlayerStats.Instance.gab}";
    }
}

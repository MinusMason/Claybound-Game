using UnityEngine;
using TMPro;

public class DamagePopup : MonoBehaviour
{
    public static void Create(Vector3 worldPosition, float damage, bool isCrit)
    {
        if (!SettingsUI.ShowDamageNumbers) return;
        GameObject obj = new GameObject("DamagePopup");
        obj.transform.position = worldPosition + Vector3.up * 1.5f;
        obj.AddComponent<DamagePopup>().Setup(damage, isCrit);
    }

    [Header("Settings")]
    public float moveSpeed = 2f;
    public float lifetime  = 1f;

    private TextMeshPro tmp;
    private float elapsed;
    private Color startColor;

    private void Setup(float damage, bool isCrit)
    {
        tmp           = gameObject.AddComponent<TextMeshPro>();
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.sortingOrder = 10;

        if (isCrit)
        {
            tmp.text     = $"<b>{Mathf.RoundToInt(damage)}!</b>";
            tmp.fontSize = 6f;
            tmp.color    = new Color(1f, 0.8f, 0f);
        }
        else
        {
            tmp.text     = Mathf.RoundToInt(damage).ToString();
            tmp.fontSize = 4f;
            tmp.color    = Color.white;
        }

        startColor = tmp.color;
        Destroy(gameObject, lifetime);
    }

    private void Update()
    {
        elapsed += Time.deltaTime;

        transform.position += Vector3.up * moveSpeed * Time.deltaTime;

        if (Camera.main != null)
            transform.forward = Camera.main.transform.forward;

        float fade = 1f - Mathf.Clamp01((elapsed - lifetime * 0.5f) / (lifetime * 0.5f));
        tmp.color = new Color(startColor.r, startColor.g, startColor.b, fade);
    }
}

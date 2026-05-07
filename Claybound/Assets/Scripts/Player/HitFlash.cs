using System.Collections;
using UnityEngine;

public class HitFlash : MonoBehaviour
{
    public Color flashColor = Color.red;
    public float flashDuration = 0.15f;

    private Renderer[] renderers;
    private Color[] originalColors;

    private void Awake()
    {
        renderers = GetComponentsInChildren<Renderer>();
        originalColors = new Color[renderers.Length];
        for (int i = 0; i < renderers.Length; i++)
            originalColors[i] = renderers[i].material.color;
    }

    public void Flash()
    {
        StopAllCoroutines();
        StartCoroutine(DoFlash());
    }

    private IEnumerator DoFlash()
    {
        for (int i = 0; i < renderers.Length; i++)
            renderers[i].material.color = flashColor;

        yield return new WaitForSeconds(flashDuration);

        for (int i = 0; i < renderers.Length; i++)
            renderers[i].material.color = originalColors[i];
    }
}

using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class LevelUI : MonoBehaviour
{
    public TextMeshProUGUI levelText;

    private void Start()
    {
        if (levelText != null)
            levelText.text = "Level " + SceneManager.GetActiveScene().buildIndex;
    }
}

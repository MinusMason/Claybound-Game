using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsUI : MonoBehaviour
{
    public static SettingsUI Instance { get; private set; }

    [Header("Panel")]
    public GameObject settingsPanel;

    [Header("Sensitivity")]
    public Slider sensitivitySlider;
    public TextMeshProUGUI sensitivityLabel;

    [Header("Volume")]
    public Slider volumeSlider;
    public TextMeshProUGUI volumeLabel;

    [Header("Damage Numbers")]
    public Toggle damageNumbersToggle;

    public static bool ShowDamageNumbers = true;

    private CameraController cameraController;
    private bool isPaused = false;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        if (settingsPanel != null) settingsPanel.SetActive(false);
    }

    private void Start()
    {
        cameraController = FindFirstObjectByType<CameraController>();

        // Set slider starting values
        if (sensitivitySlider != null)
        {
            sensitivitySlider.minValue = 0.5f;
            sensitivitySlider.maxValue = 10f;
            sensitivitySlider.value    = cameraController != null ? cameraController.mouseSensitivity : 2f;
            sensitivitySlider.onValueChanged.AddListener(OnSensitivityChanged);
        }

        if (volumeSlider != null)
        {
            volumeSlider.minValue = 0f;
            volumeSlider.maxValue = 1f;
            volumeSlider.value    = AudioListener.volume;
            volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
        }

        if (damageNumbersToggle != null)
        {
            damageNumbersToggle.isOn = ShowDamageNumbers;
            damageNumbersToggle.onValueChanged.AddListener(OnDamageNumbersChanged);
        }

        UpdateLabels();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused) Close();
            else Open();
        }
    }

    public void Open()
    {
        if (settingsPanel == null) return;
        isPaused = true;
        settingsPanel.SetActive(true);
        Time.timeScale   = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible   = true;
    }

    public void Close()
    {
        if (settingsPanel == null) return;
        isPaused = false;
        settingsPanel.SetActive(false);
        Time.timeScale   = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible   = false;
    }

    private void OnSensitivityChanged(float value)
    {
        if (cameraController != null)
            cameraController.mouseSensitivity = value;
        UpdateLabels();
    }

    private void OnVolumeChanged(float value)
    {
        AudioListener.volume = value;
        UpdateLabels();
    }

    private void OnDamageNumbersChanged(bool value)
    {
        ShowDamageNumbers = value;
    }

    private void UpdateLabels()
    {
        if (sensitivityLabel != null)
            sensitivityLabel.text = $"Sensitivity: {sensitivitySlider.value:F1}";
        if (volumeLabel != null)
            volumeLabel.text = $"Volume: {Mathf.RoundToInt(volumeSlider.value * 100)}%";
    }
}

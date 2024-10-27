using UnityEngine;
using UnityEngine.UI;

public class OptionsManager : MonoBehaviour
{
    [SerializeField] private Toggle muteSoundToggle;

    void Start()
    {
        // Initialize the toggle based on the current mute state
        muteSoundToggle.isOn = AudioManager.Instance.IsMuted();
        muteSoundToggle.onValueChanged.AddListener(OnToggleChanged);
    }

    private void OnToggleChanged(bool isOn)
    {
        AudioManager.Instance.MuteSounds(isOn);
    }
}

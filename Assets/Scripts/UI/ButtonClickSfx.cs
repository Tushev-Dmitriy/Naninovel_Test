using UnityEngine;
using UnityEngine.UI;

public class ButtonClickSfx : MonoBehaviour
{
    [SerializeField] private string sfxPath = "SFX/move_button";
    [SerializeField] [Range(0f, 1f)] private float volume = 0.14f;

    private Button button;

    private void Awake ()
    {
        button = GetComponent<Button>();
        if (button != null)
            button.onClick.AddListener(PlayClickSound);
    }

    private void PlayClickSound ()
    {
        NaninovelAudioCue.PlaySfx(sfxPath, volume);
    }
}

using Naninovel;

public static class NaninovelAudioCue
{
    public static void PlaySfx (string path, float volume)
    {
        if (!Engine.Initialized || string.IsNullOrEmpty(path))
            return;

        var audioManager = Engine.GetService<IAudioManager>();
        if (audioManager == null)
            return;

        audioManager.PlaySfxAsync(path, volume).Forget();
    }
}

namespace SlipeServer.Resources.NoClip;

public interface INoClipEventHub
{
    void SetEnabled(bool enabled);
    void SetPosition(float x, float y, float z);
    void UpdateConfiguration(float verticalSpeed, float horizontalSpeed);
}

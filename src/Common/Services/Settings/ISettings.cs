namespace Common.Services.Settings;

public interface ISettings
{
    int Version { get; set; }
    void Validate();
}
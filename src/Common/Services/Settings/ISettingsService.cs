namespace Common.Services.Settings;

public interface ISettingsService<T> where T :  ISettings, new()
{
    T Current { get; set; }

    object Sync { get; }
    
    string ConfigName { get; set; }
    string AppName { get; set; }
    bool Load();
    void Save();
}
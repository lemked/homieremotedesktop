namespace Homie.Service.Settings
{
    public interface IServiceSettingsProvider
    {
        ServiceSettings GetSettings();

        void SaveSettings(ServiceSettings settings);
    }
}

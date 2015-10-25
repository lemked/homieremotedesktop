using System;
using System.Linq;
using Homie.Common.Interfaces;
using Homie.Model;

namespace Homie.Service.Settings
{
    public class DbServiceSettingsProvider : IServiceSettingsProvider
    {
        private class SettingNames
        {
            public const string AuthenticationMode = "AuthenticationMode";
            public const string CertificateFilePath = "CertificateFilePath";
            public const string EndPoint = "EndPoint";
            public const string ListenPort = "ListenPort";
        }

        private readonly DatabaseDatasource<Setting> dataSource;
        private readonly DatabaseContext databaseContext;

        public DbServiceSettingsProvider()
        {
            databaseContext = new DatabaseContext();
            dataSource = new DatabaseDatasource<Setting>();
            dataSource.EnableAutoCommit = false;
        }

        public ServiceSettings GetSettings()
        {
            var settings = new ServiceSettings();

            settings.AuthenticationMode = GetAuthenticationMode();
            settings.CertificateFilePath = GetSettingAsString(SettingNames.CertificateFilePath);
            settings.EndPoint = GetSettingAsString(SettingNames.EndPoint);
            settings.ListenPort = GetSettingAsInt(SettingNames.ListenPort);

            return settings;
        }

        private Setting GetSetting(string key)
        {
            var result = databaseContext.Settings.FirstOrDefault(setting => setting.Key == key);
            if (result == null)
            {
                throw new InvalidOperationException($"Setting with name \"{key}\" not found");
            }
            return result;
        }

        private string GetSettingAsString(string key)
        {
            var result = GetSetting(key);
            return result.Value.ToString();
        }

        private int GetSettingAsInt(string key)
        {
            var result = GetSetting(key);
            return int.Parse(result.Value.ToString());
        }

        private AuthenticationMode GetAuthenticationMode()
        {
            AuthenticationMode result;
            Enum.TryParse(GetSettingAsString(SettingNames.AuthenticationMode), out result);
            return result;
        }

        private void UpdateStringSetting(string key, string value)
        {
            var settingCertificateFilePath = dataSource.GetAll().Single(s => s.Key == key);
            settingCertificateFilePath.Value = value;
            dataSource.Update(settingCertificateFilePath);
        }

        public void SaveSettings(ServiceSettings settings)
        {
            var authenticationModeSetting = dataSource.GetAll().Single(s => s.Key == SettingNames.AuthenticationMode);
            authenticationModeSetting.Value = settings.AuthenticationMode.ToString();
            dataSource.Update(authenticationModeSetting);

            UpdateStringSetting(SettingNames.CertificateFilePath, settings.CertificateFilePath);
            UpdateStringSetting(SettingNames.EndPoint, settings.EndPoint);

            var listenPortSetting = dataSource.GetAll().Single(s => s.Key == SettingNames.ListenPort);
            listenPortSetting.Value = settings.ListenPort.ToString();
            dataSource.Update(listenPortSetting);

            dataSource.SaveChanges();
        }
    }
}

using UnityEditor;
using UnityEditor.SettingsManagement;

namespace BlackBox.Editor
{
    static class BlackBoxSettingsManager
    {
        internal const string k_PackageName = "io.continis.blackbox";

        private static Settings instance;

        internal static Settings settings
        {
            get
            {
                if (instance == null)
                    instance = new Settings(k_PackageName, "Settings");
                
                return instance;
            }
        }
    }

    public class PackageSetting<T> : UserSetting<T>
    {
        public PackageSetting(string key, T value)
            : base(BlackBoxSettingsManager.settings, key, value, SettingsScope.Project) { }
    }

    public class UserPref<T> : UserSetting<T>
    {
        public UserPref(string key, T value)
            : base(BlackBoxSettingsManager.settings, key, value, SettingsScope.User) { }
    }
}
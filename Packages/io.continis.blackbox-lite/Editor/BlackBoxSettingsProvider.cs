using UnityEditor;
using UnityEditor.SettingsManagement;

namespace BlackBox.Editor
{
    static class BlackBoxSettingsProvider
    {
        const string SettingsPath = "Project/Black Box";

        [SettingsProvider]
        static SettingsProvider CreateSettingsProvider()
        {
            UserSettingsProvider provider = new(SettingsPath,
                BlackBoxSettingsManager.settings,
                new [] { typeof(BlackBoxSettingsProvider).Assembly }, SettingsScope.Project)
            {
                keywords = new[] { "Prefab", "Prefabs", "Blackbox", "Encapsulation", "Lock" }
            };

            BlackBoxSettingsManager.settings.afterSettingsSaved += OnSettingsSaved;
            
            return provider;
        }

        private static void OnSettingsSaved()
        {
            BlackBox.LockingDisabled = BlackBoxSettings.DisableLocking.value;
            SceneWatcher.UpdateAllPrefabsInScene();
        }
    }
}
using UnityEditor.SettingsManagement;

namespace BlackBox.Editor
{
    public static class BlackBoxSettings
    {
        // Shared team settings
        [UserSetting("Auto-add BlackBox component", "To new Prefabs", 
            "Automatically adds a BlackBox component to all newly created Prefabs.")]
        public static PackageSetting<bool> AutoAddToPrefabs = new("general.autoAddToPrefab", false);
        [UserSetting("Auto-add BlackBox component", "To new Prefab variants", 
            "Automatically adds a BlackBox component to all newly created Prefab variants.")]
        public static PackageSetting<bool> AutoAddToVariants = new("general.autoAddToVariant", false);

        // User-specific data
        [UserSetting("User-specific preferences", 
            "Disable locking", 
            "General switch to disable all BlackBox components in the project. " +
                    "This can be used to temporarily disable BlackBox functionality, then later re-enable it.\n\n" +
                    "Set it to on to unlock all Prefabs, regardless of their unique setting. Set it to off to restore normal BlackBox behaviour.")]
        public static UserPref<bool> DisableLocking = new("prefs.disableLocking", false);
    }
}
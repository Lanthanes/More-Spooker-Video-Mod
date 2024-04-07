using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using MoreSpookerVideo.Networks;
using Photon.Pun;
using System.Collections.Generic;
using System.Linq;
using Zorro.Core;

namespace MoreSpookerVideo
{
    [BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
    public class MoreSpookerVideo : BaseUnityPlugin
    {
        public static MoreSpookerVideo Instance { get; private set; } = null!;

        internal new static ManualLogSource Logger { get; private set; } = null!;
        internal static Harmony? Harmony { get; set; }

        internal static ConfigEntry<int>? CameraPrice { get; private set; }
        internal static ConfigEntry<float>? CameraTimeMultiplier { get; private set; }
        internal static ConfigEntry<int>? ViewRateMultiplier { get; private set; }
        internal static ConfigEntry<int>? StartMoney { get; private set; }
        internal static ConfigEntry<bool>? EnabledAllItem { get; private set; }
        internal static ConfigEntry<bool>? AllItemFree { get; private set; }

        public static CustomPrefabPool? CustomPrefabPool;

        public static List<Item> AllItems => ((DatabaseAsset<ItemDatabase, Item>) (object) SingletonAsset<ItemDatabase>.Instance).Objects.ToList(); // Resources.FindObjectsOfTypeAll<Item>().Concat(FindObjectsByType<Item>(FindObjectsInactive.Include, FindObjectsSortMode.InstanceID)).ToList();

        private void Awake()
        {
            Logger = base.Logger;
            Instance = this;

            CameraPrice = Config.Bind("General", "CameraPrice", 100, "The price of camera in shop");
            CameraTimeMultiplier = Config.Bind("General", "CameraTimeMultiplier", 0f,
                "Multiplier of camera times (0 = default ingame value, -1 = infinite time)");
            ViewRateMultiplier = Config.Bind("General", "ViewRateMultiplier", 0,
                "Multiplier of view rates (0 = default ingame value)");
            EnabledAllItem = Config.Bind("General", "EnabledAllItem", false,
                "Unlock all game items (default false)");
            StartMoney = Config.Bind("General", "StartMoney", 0,
                "Define money in start of game party");
            AllItemFree = Config.Bind("General", "AllItemFree", false,
                "Make all items free (default false)");

            Patch();

            Logger.LogInfo($"{MyPluginInfo.PLUGIN_GUID} v{MyPluginInfo.PLUGIN_VERSION} has loaded!");
        }

        private void Start()
        {
            PhotonNetwork.PrefabPool = CustomPrefabPool;
        }

        internal static void Patch()
        {
            Harmony ??= new Harmony(MyPluginInfo.PLUGIN_GUID);

            Logger.LogDebug("Patching...");

            Harmony.PatchAll();

            Logger.LogDebug("Finished patching!");
        }
    }
}

using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
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
        internal static ConfigEntry<int>? CameraTimeMultiplier { get; private set; }
        internal static ConfigEntry<float>? ViewRateMultiplier { get; private set; }
        internal static ConfigEntry<int>? StartMoney { get; private set; }
        internal static ConfigEntry<bool>? EnabledAllItem { get; private set; }
        internal static ConfigEntry<float>? ChangePriceOfItem { get; private set; }

        public static List<Item> AllItems => ((DatabaseAsset<ItemDatabase, Item>) (object) SingletonAsset<ItemDatabase>.Instance).Objects.ToList(); // Resources.FindObjectsOfTypeAll<Item>().Concat(FindObjectsByType<Item>(FindObjectsInactive.Include, FindObjectsSortMode.InstanceID)).ToList();

        private void Awake()
        {
            Logger = base.Logger;
            Instance = this;

            CameraPrice = Config.Bind("General", "CameraPrice", 100, "The price of camera in shop");
            CameraTimeMultiplier = Config.Bind("General", "CameraTimeMultiplier", 0,
                "Multiplier of camera times (0 = default ingame value, -1 = infinite time)");
            ViewRateMultiplier = Config.Bind("General", "ViewRateMultiplier", 0f,
                "Multiplier of view rates (0 = default ingame value)");
            EnabledAllItem = Config.Bind("General", "EnabledAllItem", false,
                "Unlock all game items (default false)");
            StartMoney = Config.Bind("General", "StartMoney", 0,
                "Define money in start of game party");
            ChangePriceOfItem = Config.Bind("General", "ChangePriceOfItem", 1f,
                "Change price value of item (1 = default ingame value, 0 = free item)");

            AddShopItem();

            Patch();

            Logger.LogInfo($"{MyPluginInfo.PLUGIN_GUID} v{MyPluginInfo.PLUGIN_VERSION} has loaded!");
        }

        internal static void Patch()
        {
            Harmony ??= new Harmony(MyPluginInfo.PLUGIN_GUID);

            Logger.LogDebug("Patching...");

            Harmony.PatchAll();

            Logger.LogDebug("Finished patching!");
        }

        internal static void AddShopItem()
        {
            Item? defaultItem = AllItems.FirstOrDefault(item => item.name.ToLower().Equals("disc"));

            AllItems.ForEach(item =>
            {
                if (EnabledAllItem!.Value)
                {
                    if (item.displayName == null || item.displayName.Equals(""))
                    {
                        item.displayName = item.name;
                    }

                    if (item.icon == null || item.icon.Equals(""))
                    {
                        item.icon = defaultItem.icon;
                    }

                    item.Category = (item.Category.Equals(ShopItemCategory.Invalid) ? ShopItemCategory.Misc : item.Category);
                    item.purchasable = true;

                    Logger?.LogInfo($"Item {item.displayName} has unlocked! ({item.Category})");
                }

                // select playable camera, not broken !!!
                if (item.itemType.Equals(Item.ItemType.Camera) && item.name.ToLower().Equals("camera"))
                {
                    item.purchasable = true;
                    item.Category = ShopItemCategory.Gadgets;

                    if (CameraPrice!.Value >= 0)
                    {
                        item.price = CameraPrice!.Value;
                    }

                    Logger?.LogInfo($"{item.displayName} added to {item.Category} shop!");
                }

                if (ChangePriceOfItem!.Value != 1f)
                {
                    item.price = UnityEngine.Mathf.FloorToInt(item.price * ChangePriceOfItem!.Value);
                }
            });
        }
    }
}

/*
(Boom Mic) Boom Mic for 100$ purchasable is normal! => toolBudgetCost: 1, budgetCost: 0, rarity: 1, quantity: 0, Tooltips: 1
(Camera) Camera for 0$ purchasable is normal! => toolBudgetCost: 1, budgetCost: 0, rarity: 1, quantity: 0, Tooltips: 4
(CameraBroken) Broken Camera for 100$ not purchasable is normal! => toolBudgetCost: 1, budgetCost: 0, rarity: 1, quantity: 0, Tooltips: 0
(Clapper) Clapper for 100$ purchasable is normal! => toolBudgetCost: 1, budgetCost: 0, rarity: 1, quantity: 0, Tooltips: 2
(Defibrilator) Defibrilator for 300$ purchasable is normal! => toolBudgetCost: 1, budgetCost: 0, rarity: 1, quantity: 0, Tooltips: 1
(Disc) Disc for 0$ not purchasable is normal! => toolBudgetCost: 1, budgetCost: 0, rarity: 1, quantity: 0, Tooltips: 1
(FakeOldFlashlight)  for 20$ not purchasable is normal! => toolBudgetCost: 1, budgetCost: 0, rarity: 1, quantity: 0, Tooltips: 1
(Flare) Flare for 40$ purchasable is normal! => toolBudgetCost: 1, budgetCost: 0, rarity: 1, quantity: 0, Tooltips: 1
(GooBall) Goo Ball for 50$ purchasable is normal! => toolBudgetCost: 1, budgetCost: 0, rarity: 1, quantity: 0, Tooltips: 1
(Hugger) Hugger for 100$ purchasable is normal! => toolBudgetCost: 1, budgetCost: 0, rarity: 1, quantity: 0, Tooltips: 2
(Long Flashlight Pro) Long Flashlight Pro for 600$ purchasable is normal! => toolBudgetCost: 1, budgetCost: 0, rarity: 1, quantity: 0, Tooltips: 1
(Long Flashlight) Long Flashlight for 200$ purchasable is normal! => toolBudgetCost: 1, budgetCost: 0, rarity: 1, quantity: 0, Tooltips: 1
(LostDisc) Disc for 0$ not purchasable is normal! => toolBudgetCost: 1, budgetCost: 0, rarity: 1, quantity: 0, Tooltips: 1
(Modern Flashlight Pro) Modern Flashlight Pro for 500$ purchasable is normal! => toolBudgetCost: 1, budgetCost: 0, rarity: 1, quantity: 0, Tooltips: 1
(Modern Flashlight) Modern Flashlight for 150$ purchasable is normal! => toolBudgetCost: 1, budgetCost: 0, rarity: 1, quantity: 0, Tooltips: 1
(Old Flashlight) Old Flashlight for 20$ purchasable is normal! => toolBudgetCost: 1, budgetCost: 0, rarity: 1, quantity: 0, Tooltips: 1
(PartyPopper) Party Popper for 5$ purchasable is normal! => toolBudgetCost: 1, budgetCost: 0, rarity: 1, quantity: 0, Tooltips: 1
(Reporter Mic) Reporter Mic for 50$ purchasable is normal! => toolBudgetCost: 1, budgetCost: 0, rarity: 1, quantity: 0, Tooltips: 2
(ShockStick) Shock Stick for 400$ purchasable is normal! => toolBudgetCost: 1, budgetCost: 0, rarity: 1, quantity: 0, Tooltips: 1
(SoundPlayer) Sound Player for 100$ purchasable is normal! => toolBudgetCost: 1, budgetCost: 0, rarity: 1, quantity: 0, Tooltips: 2
(WalkieTalkie) Walkie-talkie for 70$ not purchasable is normal! => toolBudgetCost: 1, budgetCost: 0, rarity: 1, quantity: 0, Tooltips: 1
(Wide Flashlight 2)  for 50$ not purchasable is normal! => toolBudgetCost: 1, budgetCost: 0, rarity: 1, quantity: 0, Tooltips: 1
(Wide Flashlight 3)  for 800$ not purchasable is normal! => toolBudgetCost: 1, budgetCost: 0, rarity: 1, quantity: 0, Tooltips: 1
(Winch)  for 0$ not purchasable is normal! => toolBudgetCost: 1, budgetCost: 0, rarity: 1, quantity: 0, Tooltips: 0
(Aminal stateu)  for 40$ not purchasable is an artifact! => toolBudgetCost: 1, budgetCost: 2, rarity: 0,05, quantity: 0, Tooltips: 0
(Animal statue)  for 40$ not purchasable is an artifact! => toolBudgetCost: 1, budgetCost: 3, rarity: 0,05, quantity: 0, Tooltips: 0
(Bone)  for 40$ not purchasable is an artifact! => toolBudgetCost: 1, budgetCost: 1, rarity: 2, quantity: 0, Tooltips: 0
(Brain on a stick)  for 40$ not purchasable is an artifact! => toolBudgetCost: 1, budgetCost: 2, rarity: 0,01, quantity: 0, Tooltips: 0
(Chorby)  for 40$ not purchasable is an artifact! => toolBudgetCost: 1, budgetCost: 1, rarity: 2, quantity: 0, Tooltips: 0
(Container)  for 40$ not purchasable is an artifact! => toolBudgetCost: 1, budgetCost: 2, rarity: 0,01, quantity: 0, Tooltips: 0
(Old Painting)  for 40$ not purchasable is an artifact! => toolBudgetCost: 1, budgetCost: 3, rarity: 0,007, quantity: 0, Tooltips: 0
(Radio)  for 40$ not purchasable is an artifact! => toolBudgetCost: 1, budgetCost: 2, rarity: 0,025, quantity: 0, Tooltips: 1
(Ribcage)  for 40$ not purchasable is an artifact! => toolBudgetCost: 1, budgetCost: 1, rarity: 2, quantity: 0, Tooltips: 0
(Skull)  for 40$ not purchasable is an artifact! => toolBudgetCost: 1, budgetCost: 1, rarity: 2, quantity: 0, Tooltips: 0
(Spine)  for 40$ not purchasable is an artifact! => toolBudgetCost: 1, budgetCost: 1, rarity: 2, quantity: 0, Tooltips: 0
(Fov_1)  for 50$ not purchasable is normal! => toolBudgetCost: 1, budgetCost: 0, rarity: 1, quantity: -1, Tooltips: 0
(Fov_2)  for 50$ not purchasable is normal! => toolBudgetCost: 1, budgetCost: 0, rarity: 1, quantity: -1, Tooltips: 0
(Fov_3)  for 50$ not purchasable is normal! => toolBudgetCost: 1, budgetCost: 0, rarity: 1, quantity: -1, Tooltips: 0
(Zoom_1)  for 50$ not purchasable is normal! => toolBudgetCost: 1, budgetCost: 0, rarity: 1, quantity: -1, Tooltips: 0
(Zoom_2)  for 50$ not purchasable is normal! => toolBudgetCost: 1, budgetCost: 0, rarity: 1, quantity: 0, Tooltips: 0
(Zoom_3)  for 50$ not purchasable is normal! => toolBudgetCost: 1, budgetCost: 0, rarity: 1, quantity: 0, Tooltips: 0
(Emote_Applause) Applause for 100$ purchasable is normal! => toolBudgetCost: 1, budgetCost: 0, rarity: 1, quantity: 1, Tooltips: 1
(Emote_Dance1) Dance 101 for 250$ purchasable is normal! => toolBudgetCost: 1, budgetCost: 0, rarity: 1, quantity: 1, Tooltips: 1
(Emote_Dance2) Dance 102 for 200$ purchasable is normal! => toolBudgetCost: 1, budgetCost: 0, rarity: 1, quantity: 1, Tooltips: 1
(Emote_Dance3) Dance 103 for 150$ purchasable is normal! => toolBudgetCost: 1, budgetCost: 0, rarity: 1, quantity: 1, Tooltips: 1
(Emote_FingerScratch) Confused for 120$ purchasable is normal! => toolBudgetCost: 1, budgetCost: 0, rarity: 1, quantity: 1, Tooltips: 1
(Emote_HalfBackflip) Backflip p1 for 300$ purchasable is normal! => toolBudgetCost: 1, budgetCost: 0, rarity: 1, quantity: 1, Tooltips: 1
(Emote_Handstand) Gymnastics for 400$ purchasable is normal! => toolBudgetCost: 1, budgetCost: 0, rarity: 1, quantity: 1, Tooltips: 1
(Emote_HuggerHeal) Thumbnail 1 for 1000$ not purchasable is normal! => toolBudgetCost: 1, budgetCost: 0, rarity: 1, quantity: 1, Tooltips: 1
(Emote_JumpJack) Workout 1 for 100$ purchasable is normal! => toolBudgetCost: 1, budgetCost: 0, rarity: 1, quantity: 1, Tooltips: 1
(Emote_MiddleFings) Ancient Gestures 1 for 500$ purchasable is normal! => toolBudgetCost: 1, budgetCost: 0, rarity: 1, quantity: 1, Tooltips: 1
(Emote_Peace) Ancient gestures 2 for 100$ purchasable is normal! => toolBudgetCost: 1, budgetCost: 0, rarity: 1, quantity: 1, Tooltips: 1
(Emote_PushUp) Workout 2 for 350$ purchasable is normal! => toolBudgetCost: 1, budgetCost: 0, rarity: 1, quantity: 1, Tooltips: 1
(Emote_Shrug) Caring for 50$ purchasable is normal! => toolBudgetCost: 1, budgetCost: 0, rarity: 1, quantity: 1, Tooltips: 1
(Emote_Stretch) Yoga for 250$ purchasable is normal! => toolBudgetCost: 1, budgetCost: 0, rarity: 1, quantity: 1, Tooltips: 1
(Emote_Thumbnail1) Thumbnail 1 for 400$ purchasable is normal! => toolBudgetCost: 1, budgetCost: 0, rarity: 1, quantity: 1, Tooltips: 1
(Emote_Thumbnail2) Thumbnail 2 for 450$ purchasable is normal! => toolBudgetCost: 1, budgetCost: 0, rarity: 1, quantity: 1, Tooltips: 1
(Emote_ThumbsUp) Ancient gestures 3 for 80$ purchasable is normal! => toolBudgetCost: 1, budgetCost: 0, rarity: 1, quantity: 1, Tooltips: 1
(Bomb)  for 40$ not purchasable is normal! => toolBudgetCost: 1, budgetCost: 0, rarity: 1, quantity: 0, Tooltips: 0
*/

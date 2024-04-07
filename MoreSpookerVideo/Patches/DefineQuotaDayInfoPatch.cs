using System;
using HarmonyLib;

namespace MoreSpookerVideo.Patches
{
    [HarmonyPatch(typeof(RoomStatsHolder))]
    internal class DefineQuotaDayInfoPatch
    {
        [HarmonyPatch(MethodType.Constructor, new Type[] { typeof(SurfaceNetworkHandler), typeof(int), typeof(int), typeof(int) })]
        [HarmonyPrefix]
        static void RoomStatsHolderCtorPrefix(ref int startMoney, ref int startQuotaToReachToReach)
        {
            startMoney = MoreSpookerVideo.StartMoney!.Value;

            bool initRoomStats = MoreSpookerVideo.StartMoney!.Value > 0;

            if (MoreSpookerVideo.ViewRateMultiplier!.Value > 0)
            {
                startQuotaToReachToReach = UnityEngine.Mathf.FloorToInt(startQuotaToReachToReach * MoreSpookerVideo.ViewRateMultiplier!.Value);
                initRoomStats = true;
            }

            if (initRoomStats)
            {
                MoreSpookerVideo.Logger?.LogInfo($"You have {startMoney}$ in start game!");
                MoreSpookerVideo.Logger?.LogInfo($"You have {startQuotaToReachToReach} quota to reach!");
            }
        }

        [HarmonyPatch(nameof(RoomStatsHolder.CalculateNewQuota))]
        [HarmonyPostfix]
        static void CalculateNewQuotaPostfix(RoomStatsHolder __instance, ref int ___m_quotaToReachInternal)
        {
            if (MoreSpookerVideo.ViewRateMultiplier!.Value > 0)
            {
                ___m_quotaToReachInternal = UnityEngine.Mathf.FloorToInt(__instance.QuotaToReach * MoreSpookerVideo.ViewRateMultiplier!.Value);
            }

            MoreSpookerVideo.Logger?.LogInfo($"You have {___m_quotaToReachInternal} quota to reach!");
        }
    }
}

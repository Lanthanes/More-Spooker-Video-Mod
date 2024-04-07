using System;
using HarmonyLib;

namespace MoreSpookerVideo.Patches
{
    [HarmonyPatch(typeof(RoomStatsHolder), MethodType.Constructor, new Type[] { typeof(SurfaceNetworkHandler), typeof(int), typeof(int), typeof(int) })]
    internal class DefineQuotaDayInfoPatch
    {
        static void Prefix(ref int startMoney, ref int startQuotaToReachToReach)
        {
            // TODO check load or other data ???

            startMoney = MoreSpookerVideo.StartMoney!.Value;

            bool initRoomStats = MoreSpookerVideo.StartMoney!.Value > 0;

            if (MoreSpookerVideo.ViewRateMultiplier!.Value > 0)
            {
                startQuotaToReachToReach *= MoreSpookerVideo.ViewRateMultiplier!.Value;
                initRoomStats = true;
            }

            if (initRoomStats)
            {
                MoreSpookerVideo.Logger?.LogInfo("Quota is init by player config!");

                MoreSpookerVideo.Logger?.LogInfo($"You have {startMoney}$ in start game!");
                MoreSpookerVideo.Logger?.LogInfo($"You have {startQuotaToReachToReach} quota to reach!");
            }
            else
            {
                MoreSpookerVideo.Logger?.LogInfo("Quota by default in game!");
            }
        }
    }
}

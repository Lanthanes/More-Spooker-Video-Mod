using HarmonyLib;

namespace MoreSpookerVideo.Patches
{
    [HarmonyPatch(typeof(VideoInfoEntry))]
    internal class DefineTimeOfCameraPatch
    {
        private static bool fisrtPass = false;

        [HarmonyPatch(nameof(VideoInfoEntry.Deserialize))]
        [HarmonyPostfix]
        private static void OnSerializePostfix(ref float ___maxTime, ref float ___timeLeft)
        {
            if (!fisrtPass && ___maxTime == ___timeLeft)
            {
                fisrtPass = true;

                if (MoreSpookerVideo.CameraTimeMultiplier!.Value < 0)
                {
                    MoreSpookerVideo.Logger?.LogDebug($"Apply infinite time in camera!");
                    ___maxTime = float.MaxValue;
                    ___timeLeft = ___maxTime;
                }
                else if (MoreSpookerVideo.CameraTimeMultiplier!.Value > 0)
                {
                    MoreSpookerVideo.Logger?.LogDebug($"Apply value time * {MoreSpookerVideo.CameraTimeMultiplier!.Value} in camera on {___maxTime} default value!");
                    ___maxTime *= MoreSpookerVideo.CameraTimeMultiplier!.Value;
                    ___timeLeft = ___maxTime;
                }

                MoreSpookerVideo.Logger?.LogInfo($"Your can rec {___maxTime}s with camera!");
            }
            else
            {
                fisrtPass = false;
            }
        }
    }
}

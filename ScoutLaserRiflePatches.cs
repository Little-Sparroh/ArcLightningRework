using System.Collections.Generic;
using HarmonyLib;
using Pigeon.Math;
using Unity.Netcode;

namespace ArcLightningRework;

[HarmonyPatch(typeof(ScoutLaserRifle), "OnTargetDamaged")]
public static class ScoutLaserRifle_OnTargetDamaged_Patch
{
    internal static readonly Dictionary<ScoutLaserRifle, bool> lightningSpawned = new();

    private static bool Prefix(ScoutLaserRifle __instance, in DamageCallbackData data)
    {
        if (!ConfigManager.EnableArcLightningRework.Value) return true;

        if (__instance.IsLaserModeActive
            && __instance.LaserData.lightningArcChance > 0f
            && Random.shared.NextFloat() <= __instance.LaserData.lightningArcChance
            && (!lightningSpawned.ContainsKey(__instance) || !lightningSpawned[__instance]))
        {
            var isTurbocharged = false;
            foreach (var activeUpgrade in __instance.ActiveUpgrades)
            {
                for (var i = 0; i < activeUpgrade.Upgrade.Properties.Count; i++)
                    if (activeUpgrade.Upgrade.Properties[i] is UpgradeProperty_DMLR_LightningArc &&
                        activeUpgrade.IsTurbocharged)
                    {
                        isTurbocharged = true;
                        break;
                    }

                if (isTurbocharged) break;
            }

            if (isTurbocharged)
                GameManager.Instance.SpawnLightningForkEffect_ServerRpc(
                    (NetworkBehaviourReference)__instance,
                    data.position,
                    10f,
                    TargetType.Enemy,
                    10,
                    15f);
            else
                GameManager.Instance.SpawnLightningForkEffect_ServerRpc(
                    (NetworkBehaviourReference)__instance,
                    data.position,
                    10f,
                    TargetType.Enemy,
                    1);

            lightningSpawned[__instance] = true;
            return false;
        }

        return true;
    }
}

[HarmonyPatch(typeof(ScoutLaserRifle), "OnStartedFiring")]
public static class ScoutLaserRifle_OnStartedFiring_Patch
{
    private static void Postfix(ScoutLaserRifle __instance)
    {
        if (ConfigManager.EnableArcLightningRework.Value)
            ScoutLaserRifle_OnTargetDamaged_Patch.lightningSpawned[__instance] = false;
    }
}

[HarmonyPatch(typeof(ScoutLaserRifle), "OnStoppedFiring")]
public static class ScoutLaserRifle_OnStoppedFiring_Patch
{
    private static void Postfix(ScoutLaserRifle __instance)
    {
        if (ConfigManager.EnableArcLightningRework.Value
            && ScoutLaserRifle_OnTargetDamaged_Patch.lightningSpawned.ContainsKey(__instance))
            ScoutLaserRifle_OnTargetDamaged_Patch.lightningSpawned[__instance] = false;
    }
}
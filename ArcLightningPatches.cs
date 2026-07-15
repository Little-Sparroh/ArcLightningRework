using System.Collections.Generic;
using HarmonyLib;
using Pigeon.Math;
using Unity.Netcode;

[HarmonyPatch(typeof(ScoutLaserRifle), "OnTargetDamaged")]
public static class ScoutLaserRifle_OnTargetDamaged_Patch
{
    private static bool Prefix(ScoutLaserRifle __instance, in DamageCallbackData data)
    {
        if (!SparrohPlugin.enableArcLightningRework.Value)
        {
            return true;
        }

        if (__instance.IsLaserModeActive
            && __instance.LaserData.lightningArcChance > 0f
            && Random.shared.NextFloat() <= __instance.LaserData.lightningArcChance
            && (!SparrohPlugin.lightningSpawned.ContainsKey(__instance) || !SparrohPlugin.lightningSpawned[__instance]))
        {
            bool isTurbocharged = false;
            foreach (UpgradeInstance activeUpgrade in __instance.ActiveUpgrades)
            {
                for (int i = 0; i < activeUpgrade.Upgrade.Properties.Count; i++)
                {
                    if (activeUpgrade.Upgrade.Properties[i] is UpgradeProperty_DMLR_LightningArc && activeUpgrade.IsTurbocharged)
                    {
                        isTurbocharged = true;
                        break;
                    }
                }
                if (isTurbocharged)
                {
                    break;
                }
            }

            if (isTurbocharged)
            {
                GameManager.Instance.SpawnLightningForkEffect_ServerRpc(
                    (NetworkBehaviourReference)__instance,
                    data.position,
                    10f,
                    TargetType.Enemy,
                    10,
                    15f);
            }
            else
            {
                GameManager.Instance.SpawnLightningForkEffect_ServerRpc(
                    (NetworkBehaviourReference)__instance,
                    data.position,
                    10f,
                    TargetType.Enemy,
                    1);
            }

            SparrohPlugin.lightningSpawned[__instance] = true;
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
        if (SparrohPlugin.enableArcLightningRework.Value)
        {
            SparrohPlugin.lightningSpawned[__instance] = false;
        }
    }
}

[HarmonyPatch(typeof(ScoutLaserRifle), "OnStoppedFiring")]
public static class ScoutLaserRifle_OnStoppedFiring_Patch
{
    private static void Postfix(ScoutLaserRifle __instance)
    {
        if (SparrohPlugin.enableArcLightningRework.Value && SparrohPlugin.lightningSpawned.ContainsKey(__instance))
        {
            SparrohPlugin.lightningSpawned[__instance] = false;
        }
    }
}

[HarmonyPatch(typeof(TextBlocks), "GetString", new System.Type[] { typeof(string) })]
public static class TextBlocks_GetString_Patch
{
    private static void Postfix(ref string __result, string id)
    {
        if (!SparrohPlugin.enableArcLightningRework.Value)
        {
            return;
        }

        if (id == "ArcLightning_2")
        {
            __result = "Chains to 5 enemies with 15 base damage\nMax Hits: 10\nDamage: 15";
        }
    }
}

[HarmonyPatch(typeof(Upgrade), "get_Flags")]
public static class Upgrade_Flags_Getter_Patch
{
    private static void Postfix(ref Upgrade.UpgradeFlags __result, Upgrade __instance)
    {
        if (!SparrohPlugin.enableArcLightningRework.Value)
        {
            return;
        }

        try
        {
            for (int i = 0; i < __instance.Properties.Count; i++)
            {
                if (__instance.Properties[i] is UpgradeProperty_DMLR_LightningArc)
                {
                    __result |= Upgrade.UpgradeFlags.CanTurbocharge;
                    break;
                }
            }
        }
        catch
        {
        }
    }
}

[HarmonyPatch(typeof(Upgrade), "get_Description")]
public static class Upgrade_Description_Getter_Patch
{
    private static void Postfix(ref string __result, Upgrade __instance)
    {
        if (!SparrohPlugin.enableArcLightningRework.Value)
        {
            return;
        }

        if ((__instance.Flags & Upgrade.UpgradeFlags.CanTurbocharge) == 0)
        {
            return;
        }

        try
        {
            for (int i = 0; i < __instance.Properties.Count; i++)
            {
                if (__instance.Properties[i] is UpgradeProperty_DMLR_LightningArc)
                {
                    __result += "\nWhen Turbocharged:\nMax Hits: 10\nDamage: 15";
                    break;
                }
            }
        }
        catch
        {
        }
    }
}

[HarmonyPatch(typeof(UpgradeProperty), "GetTurbochargedInfo")]
public static class UpgradeProperty_GetTurbochargedInfo_Patch
{
    private static bool Prefix(ref string __result, UpgradeProperty __instance)
    {
        if (!SparrohPlugin.enableArcLightningRework.Value)
        {
            return true;
        }

        if (__instance is UpgradeProperty_DMLR_LightningArc)
        {
            __result = "Chains to 10 enemies with 15 base damage";
            return false;
        }

        return true;
    }
}

[HarmonyPatch(typeof(UpgradeProperty_DMLR_LightningArc), "GetStatData")]
public static class UpgradeProperty_DMLR_LightningArc_GetStatData_Patch
{
    private static void Postfix(ref IEnumerator<StatData> __result, Random rand, IUpgradable gear, UpgradeInstance upgrade)
    {
        if (!SparrohPlugin.enableArcLightningRework.Value)
        {
            return;
        }

        __result = GetModifiedStatData(__result, rand, gear, upgrade);
    }

    private static IEnumerator<StatData> GetModifiedStatData(
        IEnumerator<StatData> original,
        Random rand,
        IUpgradable gear,
        UpgradeInstance upgrade)
    {
        while (original.MoveNext())
        {
            yield return original.Current;
        }

        yield return StatData.Create("Max Hits", 10f, OverrideType.Override, StatData.LabelType.BeforeWithColon);
        yield return StatData.Create("Damage", 15f, OverrideType.Override, StatData.LabelType.BeforeWithColon);
    }
}

using System.Collections.Generic;
using HarmonyLib;
using Pigeon.Math;

namespace ArcLightningRework;

[HarmonyPatch(typeof(TextBlocks), "GetString", typeof(string))]
public static class TextBlocks_GetString_Patch
{
    private static void Postfix(ref string __result, string id)
    {
        if (!ConfigManager.EnableArcLightningRework.Value) return;

        if (id == "ArcLightning_2") __result = "Chains to 5 enemies with 15 base damage\nMax Hits: 10\nDamage: 15";
    }
}

[HarmonyPatch(typeof(Upgrade), "get_Flags")]
public static class Upgrade_Flags_Getter_Patch
{
    private static void Postfix(ref Upgrade.UpgradeFlags __result, Upgrade __instance)
    {
        if (!ConfigManager.EnableArcLightningRework.Value) return;

        try
        {
            for (var i = 0; i < __instance.Properties.Count; i++)
                if (__instance.Properties[i] is UpgradeProperty_DMLR_LightningArc)
                {
                    __result |= Upgrade.UpgradeFlags.CanTurbocharge;
                    break;
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
        if (!ConfigManager.EnableArcLightningRework.Value) return;

        if ((__instance.Flags & Upgrade.UpgradeFlags.CanTurbocharge) == 0) return;

        try
        {
            for (var i = 0; i < __instance.Properties.Count; i++)
                if (__instance.Properties[i] is UpgradeProperty_DMLR_LightningArc)
                {
                    __result += "\nWhen Turbocharged:\nMax Hits: 10\nDamage: 15";
                    break;
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
        if (!ConfigManager.EnableArcLightningRework.Value) return true;

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
    private static void Postfix(ref IEnumerator<StatData> __result, Random rand, IUpgradable gear,
        UpgradeInstance upgrade)
    {
        if (!ConfigManager.EnableArcLightningRework.Value) return;

        __result = GetModifiedStatData(__result, rand, gear, upgrade);
    }

    private static IEnumerator<StatData> GetModifiedStatData(
        IEnumerator<StatData> original,
        Random rand,
        IUpgradable gear,
        UpgradeInstance upgrade)
    {
        while (original.MoveNext()) yield return original.Current;

        yield return StatData.Create("Max Hits", 10f);
        yield return StatData.Create("Damage", 15f);
    }
}
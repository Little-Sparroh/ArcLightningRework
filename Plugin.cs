using System.Collections.Generic;
using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;

[BepInPlugin(PluginGUID, PluginName, PluginVersion)]
[MycoMod(null, ModFlags.IsClientSide)]
public class SparrohPlugin : BaseUnityPlugin
{
    public const string PluginGUID = "sparroh.arclightningrework";
    public const string PluginName = "ArcLightningRework";
    public const string PluginVersion = "1.0.0";

    internal static new ManualLogSource Logger;
    internal static ConfigEntry<bool> enableArcLightningRework;
    internal static Dictionary<ScoutLaserRifle, bool> lightningSpawned = new Dictionary<ScoutLaserRifle, bool>();

    private Harmony harmony;

    private void Awake()
    {
        Logger = base.Logger;

        enableArcLightningRework = Config.Bind(
            "General",
            "Enable Arc Lightning Rework",
            true,
            "Enhances lightning arc behavior for Scout Laser Rifle, allowing chaining and turbocharged upgrades.");

        harmony = new Harmony(PluginGUID);
        harmony.PatchAll(typeof(ScoutLaserRifle_OnTargetDamaged_Patch));
        harmony.PatchAll(typeof(ScoutLaserRifle_OnStartedFiring_Patch));
        harmony.PatchAll(typeof(ScoutLaserRifle_OnStoppedFiring_Patch));
        harmony.PatchAll(typeof(TextBlocks_GetString_Patch));
        harmony.PatchAll(typeof(Upgrade_Flags_Getter_Patch));
        harmony.PatchAll(typeof(Upgrade_Description_Getter_Patch));
        harmony.PatchAll(typeof(UpgradeProperty_GetTurbochargedInfo_Patch));
        harmony.PatchAll(typeof(UpgradeProperty_DMLR_LightningArc_GetStatData_Patch));

        Logger.LogInfo($"{PluginName} v{PluginVersion} loaded successfully.");
    }

    private void OnDestroy()
    {
        harmony?.UnpatchSelf();
    }
}

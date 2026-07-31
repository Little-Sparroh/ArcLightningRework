using BepInEx;
using BepInEx.Logging;
using HarmonyLib;

namespace ArcLightningRework;

[BepInPlugin(PluginGUID, PluginName, PluginVersion)]
[MycoMod(null, ModFlags.IsSandbox)]
public class ArcLightningReworkPlugin : BaseUnityPlugin
{
    public const string PluginGUID = "sparroh.arclightningrework";
    public const string PluginName = "ArcLightningRework";
    public const string PluginVersion = "1.0.1";

    internal new static ManualLogSource Logger;

    private Harmony harmony;

    private void Awake()
    {
        Logger = base.Logger;

        ConfigManager.Initialize(Config, Logger);

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

    private void Update()
    {
        ConfigManager.Tick();
    }

    private void OnDestroy()
    {
        ConfigManager.Dispose();
        harmony?.UnpatchSelf();
    }
}
using BoneLib.BoneMenu;
using HarmonyLib;
using LabFusion.Data;
using LabFusion.Network;
using LabFusion.Safety;
using MelonLoader;
using UnityEngine;
using static NoGlobalBanList.NoGlobalBan;

[assembly: MelonInfo(typeof(NoGlobalBanList.NoBanList), Name, ModVersion, Author)]
[assembly: MelonGame("Stress Level Zero", "BONELAB")]

namespace NoGlobalBanList
{
    public static class NoGlobalBan
    {
        public const string Name = "NoGlobalBanList";
        public const string Author = "Lakatrazz";
        public const string ModVersion = "1.0.0"; 
        public static bool removeGlobalBan = true;
        public static Page? nofusionbans;
    }

    /// <summary>
    /// Returns the ban information of player to return empty.
    /// </summary>
    [HarmonyPatch(typeof(GlobalBanManager), nameof(GlobalBanManager.GetBanInfo))]
    public static class GetBanInfoRemoval
    {
        static bool Prefix(ref GlobalBanInfo __result)
        {
            if (removeGlobalBan)
            {
                __result = null;
                return false;
            }
            return true;
        }
    }

    /// <summary>
    /// Returns false to disable the ban check for the lobby searcher.
    /// </summary>
    [HarmonyPatch(typeof(GlobalBanManager), nameof(GlobalBanManager.IsBanned))]
    [HarmonyPatch(new[] { typeof(LobbyInfo) })]
    public static class IsBannedLobbyCheckRemoval
    {
        static bool Prefix(ref bool __result)
        {
            if (removeGlobalBan)
            {
                __result = false;
                return false;
            }
            return true;
        }
    }

    /// <summary>
    /// Returns false to disable the ban check for the platform information for the player.
    /// </summary>
    [HarmonyPatch(typeof(GlobalBanManager), nameof(GlobalBanManager.IsBanned))]
    [HarmonyPatch(new[] { typeof(PlatformInfo) })]
    public static class IsBannedCheckRemoval
    {
        static bool Prefix(ref bool __result)
        {
            if (removeGlobalBan)
            {
                __result = false;
                return false;
            }
            return true;
        }
    }

    /// <summary>
    /// Creates the bonemenu page to turn on & off the Global Ban List.
    /// </summary>
    [HarmonyPatch(typeof(SteamNetworkLayer), nameof(SteamNetworkLayer.LogIn))]
    public static class CreateBoneMenuNGBL
    { 
        public static void Postfix()
        {
            nofusionbans = Page.Root.CreatePage("No Global Ban List", Color.red);
            nofusionbans.CreateBool("Global Ban List", Color.red, NoGlobalBan.removeGlobalBan, newVal =>
            {
                removeGlobalBan = newVal;
            });
        }
    }
    public class NoBanList : MelonMod
    {
     ///don't need anything here
    }
}
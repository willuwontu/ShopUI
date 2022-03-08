using BepInEx;
using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using ItemShops.Interfaces;
using UnboundLib;
using UnboundLib.Cards;
using Jotunn.Utils;
using UnityEngine;

namespace ItemShops
{
    // These are the mods required for our mod to work
    [BepInDependency("com.willis.rounds.unbound", BepInDependency.DependencyFlags.HardDependency)]
    [BepInDependency("pykess.rounds.plugins.moddingutils", BepInDependency.DependencyFlags.HardDependency)]
    [BepInDependency("pykess.rounds.plugins.cardchoicespawnuniquecardpatch", BepInDependency.DependencyFlags.HardDependency)]
    // Declares our mod to Bepin
    [BepInPlugin(ModId, ModName, Version)]
    // The game our mod is associated with
    [BepInProcess("Rounds.exe")]
    public class ItemShops : BaseUnityPlugin
    {
        private const string ModId = "com.My.Mod.Id";
        private const string ModName = "MyModName";
        public const string Version = "0.0.0"; // What version are we on (major.minor.patch)?

        public static ItemShops instance { get; private set; }

        public AssetBundle assets { get; private set; }

        public List<AudioClip> click;
        public List<AudioClip> hover;

        void Awake()
        {

        }
        void Start()
        {
            instance = this;

            // Use this to call any harmony patch files your mod may have
            var harmony = new Harmony(ModId);
            harmony.PatchAll();

            gameObject.AddComponent<InterfaceGameModeHooksManager>();

            assets = AssetUtils.LoadAssetBundleFromResources("shopuiassets", typeof(ItemShops).Assembly);
            click = assets.LoadAllAssets<AudioClip>().ToList().Where(clip => clip.name.Contains("UI_Button_Click")).ToList();
            hover = assets.LoadAllAssets<AudioClip>().ToList().Where(clip => clip.name.Contains("UI_Button_Hover")).ToList();
        }
    }
}

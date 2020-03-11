using System;
using System.Linq;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewModdingAPI.Utilities;
using StardewValley;
using static LuckyShoes.API;

namespace LuckyShoes
{
    /// <summary>The mod entry point.</summary>
    public class ModEntry : Mod
    {
        /// <summary>The mod entry point, called after the mod is first loaded.</summary>
        /// <param name="helper">Provides simplified APIs for writing mods.</param>
        /// 
        /*********
        ** Fields
        *********/

        //setting up for a config, the SMAPI helper, and other uses later
        internal static ModConfig Config;
        internal static IManifest Mod; 

        private int leprechaunShoes = 14;
        private bool isWearingLeprechaunShoes = false;

        public override void Entry(IModHelper helper)
        {
            helper.Events.GameLoop.UpdateTicked += onUpdateTicked;
            Helper.Events.GameLoop.GameLaunched += onGameLaunched;

            Config = Helper.ReadConfig<ModConfig>();

        }

        private void onGameLaunched(object sender, GameLaunchedEventArgs e)
        {
            var GMCMApi = Helper.ModRegistry.GetApi<GenericModConfigMenuAPI>("spacechase0.GenericModConfigMenu");
            if (GMCMApi != null)
            {
                GMCMApi.RegisterModConfig(ModManifest, () => Config = new ModConfig(), () => Helper.WriteConfig(Config));
                GMCMApi.RegisterSimpleOption(ModManifest, "Verbose Logging", "Enable verbose logging for troubleshooting", () => Config.VerboseLogging, (bool val) => Config.VerboseLogging = val);

            }
        }

        private void onUpdateTicked(object sender, UpdateTickedEventArgs e)
        {
            if (!Context.IsWorldReady)
            {
                return;
            }
            
            else if (e.IsOneSecond)
            {
                this.equippedBoots();

                if (isWearingLeprechaunShoes == true)
                {
                    this.giveBuff();
                    if (ModEntry.Config.VerboseLogging)
                    {
                        this.Monitor.Log($"Buff applied.", LogLevel.Debug);
                    }
                }
                else
                {
                    this.buffOff();
                    if (ModEntry.Config.VerboseLogging)
                    {
                        this.doNothing();
                    }
                       
                }
            }
            
        }

        private void giveBuff()
        {

            Buff luckBuff = Game1.buffsDisplay.otherBuffs.Find(b => b.source == "Lucky");

            if (luckBuff == null)
            {

                luckBuff = new Buff(0, 0, 0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 1000, "Lucky", "Lucky");
                Game1.buffsDisplay.addOtherBuff(luckBuff);
                if (ModEntry.Config.VerboseLogging)
                {
                    this.Monitor.Log($"Adding luck buff.", LogLevel.Debug);
                }
            }

            else if (luckBuff != null)
            {
                luckBuff.millisecondsDuration = 10000;
            }
            
        }
        private void buffOff()
        {

            Buff luckBuff = Game1.buffsDisplay.otherBuffs.Find(b => b.source == "Lucky");

            if (luckBuff != null)
            {

                luckBuff.removeBuff();
                Game1.buffsDisplay.otherBuffs.Remove(luckBuff);
                if (ModEntry.Config.VerboseLogging)
                {
                    this.Monitor.Log($"Removing luck buff.", LogLevel.Debug);
                }
            }

            else if (luckBuff == null)
            {
                return;
            }

        }

        private void equippedBoots()
        {
            var currentBoots = Game1.player.shoes.Value;
            Monitor.Log($"Current boots value is {currentBoots}.", LogLevel.Debug);

            if (currentBoots != leprechaunShoes)
            {
                if (ModEntry.Config.VerboseLogging)
                {
                    this.Monitor.Log($"Boots are not Lucky Shoes.", LogLevel.Debug);
                }
                isWearingLeprechaunShoes = false;
                return;
            }
            else
            {
                isWearingLeprechaunShoes = true;
                if (ModEntry.Config.VerboseLogging)
                {
                    this.Monitor.Log($"Player is wearing correct boots? {isWearingLeprechaunShoes}.", LogLevel.Debug);
                }
            }
        }

        private void doNothing()
        {
            if (ModEntry.Config.VerboseLogging)
            {
                this.Monitor.Log($"We're doing nothing", LogLevel.Debug);
            }
        }

    }
}
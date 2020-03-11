using StardewModdingAPI;
using System;
using System.Collections.Generic;
using System.Text;

namespace LuckyShoes
{
    class API
    {
        public interface GenericModConfigMenuAPI
        {
            void RegisterModConfig(IManifest mod, Action revertToDefault, Action saveToFile);

            void RegisterLabel(IManifest mod, string labelName, string labelDesc);
            void RegisterSimpleOption(IManifest mod, string optionName, string optionDesc, Func<bool> optionGet, Action<bool> optionSet);
        }
    }
}

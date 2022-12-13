using System;
using System.Collections.Generic;
using System.Text;

namespace CACO_Patcher.Settings {
    class Settings {
        public bool PatchIngestibles { get; set; } = true;
        public bool PatchBooks { get; set; } = true;
        public bool PatchRecipies { get; set; } = true;
        public bool PatchLootables { get; set; } = true;
        public bool PatchFlora { get; set; } = true;
        public bool PatchFormIdLists { get; set; } = true;
        public bool PatchAlchemySkillFactor { get; set; } = true;
        public bool PatchIngredients { get; set; } = true;
        public bool PatchLeveledItems { get; set; } = true;
        public bool PatchMessages { get; set; } = true;
        public bool PatchMagicEffects { get; set; } = true;
        public bool PatchMiscItems { get; set; } = true;
        public bool PatchNpcs { get; set; } = true;
        public bool PatchPerks { get; set; } = true;
        public bool PatchQuests { get; set; } = true;
        public bool PatchSpells { get; set; } = true;
        public bool PatchTrees { get; set; } = true;
        public String CacoBaseModName { get; set; } = "Complete Alchemy & Cooking Overhaul.esp";
    }
}

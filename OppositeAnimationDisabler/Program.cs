using System;
using System.Collections.Generic;
using System.Linq;
using Mutagen.Bethesda;
using Mutagen.Bethesda.Synthesis;
using Mutagen.Bethesda.Skyrim;
using Noggog;
using System.Threading.Tasks;

namespace OppositeAnimationDisabler
{
    public class Program
    {
        public static Task<int> Main(string[] args)
        {
            return SynthesisPipeline.Instance
                .SetTypicalOpen(GameRelease.SkyrimSE, "OppositeAnimationDisabler.esp")
                .AddPatch<ISkyrimMod, ISkyrimModGetter>(RunPatch)
                .Run(args);
        }

        public static void RunPatch(IPatcherState<ISkyrimMod, ISkyrimModGetter> state)
        {
            foreach (var npc in state.LoadOrder.PriorityOrder.Npc().WinningOverrides())
            {
                if (!npc.Configuration.Flags.HasFlag(NpcConfiguration.Flag.OppositeGenderAnims)) continue;

                var modifiedNPc = state.PatchMod.Npcs.GetOrAddAsOverride(npc);
                modifiedNPc.Configuration.Flags &= ~NpcConfiguration.Flag.OppositeGenderAnims;
            }
        }
    }
}

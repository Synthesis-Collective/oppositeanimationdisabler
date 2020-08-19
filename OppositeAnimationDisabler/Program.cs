using System;
using System.Collections.Generic;
using System.Linq;
using Mutagen.Bethesda;
using Mutagen.Bethesda.Synthesis;
using Mutagen.Bethesda.Skyrim;
using Noggog;

namespace OppositeAnimationDisabler
{
    public class Program
    {
        public static int Main(string[] args)
        {
            return SynthesisPipeline.Instance.Patch<ISkyrimMod, ISkyrimModGetter>(
                args: args,
                patcher: RunPatch,
                new UserPreferences()
                {
                    ActionsForEmptyArgs = new RunDefaultPatcher()
                    {
                        IdentifyingModKey = "OppositeAnimationDisabler.esp",
                        TargetRelease = GameRelease.SkyrimSE
                    }
                }
            );
        }

        public static void RunPatch(SynthesisState<ISkyrimMod, ISkyrimModGetter> state)
        {
            foreach (var npc in state.LoadOrder.PriorityOrder.WinningOverrides<INpcGetter>())
            {
                if (!npc.Configuration.Flags.HasFlag(NpcConfiguration.Flag.OppositeGenderAnims)) continue;

                var modifiedNPc = state.PatchMod.Npcs.GetOrAddAsOverride(npc);
                modifiedNPc.Configuration.Flags &= ~NpcConfiguration.Flag.OppositeGenderAnims;
            }
        }
    }
}

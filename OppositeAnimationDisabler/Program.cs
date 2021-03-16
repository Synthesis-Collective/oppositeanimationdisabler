using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mutagen.Bethesda;
using Mutagen.Bethesda.Synthesis;
using Mutagen.Bethesda.Skyrim;
using Noggog;

namespace OppositeAnimationDisabler
{
    public class Program
    {
        public static async Task<int> Main(string[] args)
        {
            return await SynthesisPipeline.Instance
                .AddPatch<ISkyrimMod, ISkyrimModGetter>(RunPatch)
                .Run(args, new RunPreferences()
                {
                    ActionsForEmptyArgs = new RunDefaultPatcher()
                    {
                        IdentifyingModKey = "OppositeAnimationDisabler.esp",
                        TargetRelease = GameRelease.SkyrimSE
                    }
                });
        }

        public static void RunPatch(IPatcherState<ISkyrimMod, ISkyrimModGetter> state)
        {
            foreach (var npc in state.LoadOrder.PriorityOrder.WinningOverrides<INpcGetter>().
                Where(candidate => candidate.Configuration.Flags.HasFlag(NpcConfiguration.Flag.OppositeGenderAnims)))
            {
                Console.WriteLine("Unflag NPC {0}", npc.EditorID);
                var modifiedNPc = state.PatchMod.Npcs.GetOrAddAsOverride(npc);
                modifiedNPc.Configuration.Flags &= ~NpcConfiguration.Flag.OppositeGenderAnims;
            }
        }
    }
}

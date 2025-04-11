using RimWorld.Planet;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using VFECore.Abilities;
using VFECore;
using static UnityEngine.GraphicsBuffer;
using System.Runtime.Remoting.Messaging;
using Verse.Sound;

namespace EmpireStraesaxSpecForces
{
    public class QuantumTeleportExtension : DefModExtension
    {

        public SoundDef soundPreviousLocation;
    }
    public class QuantumTeleport : VFECore.Abilities.Ability
    {
        public float Range => def.range;

        public SoundDef soundBegin => def.GetModExtension<QuantumTeleportExtension>().soundPreviousLocation;
        public override bool CanHitTarget(LocalTargetInfo target)
        {
            return target.Cell.WalkableBy(pawn.Map, pawn) && target.Cell.DistanceTo(pawn.Position) <= Range;
        }

        public override float GetRangeForPawn()
        {
            return Range;
        }

        public override Gizmo GetGizmo()
        {
            VFECore.Abilities.Command_Ability command_Ability = new VFECore.Abilities.Command_Ability(pawn, this);
            return command_Ability;
        }
        Effecter effecter;
        public override void Cast(params GlobalTargetInfo[] targets)
        {
            base.Cast(targets);
            soundBegin?.PlayOneShot(new TargetInfo(pawn.Position, pawn.MapHeld));
            SF_DefOf.SF_TeleportEffecter.Spawn(pawn, pawn.Map);
            IntVec3 cell = targets[0].Cell;
            pawn.Position = cell;
            pawn.Notify_Teleported();
            SF_DefOf.SF_TeleportEffecter.Spawn(cell, pawn.Map);
        }
    }

    [DefOf]
    public static class SF_DefOf
    {
        public static EffecterDef SF_TeleportEffecter;
    }
}

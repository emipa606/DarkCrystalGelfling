using RimWorld;
using Verse;

namespace DarkCrystal_Gelfling;

public class Projectile_Bola : Bullet
{
    public ThingDef_Bola Def => def as ThingDef_Bola;


    protected override void Impact(Thing hitThing, bool blockedByShield = false)
    {
        base.Impact(hitThing);

        // Always check for null, things may not exist
        // Along with checking if thing exists and that it hit something,
        // also makes sure hitThing hits an instance of a hitPawn
        if (Def == null || hitThing == null || hitThing is not Pawn hitPawn)
        {
            return;
        }

        // get a random value between 0 and 1
        var rand = Rand.Value;
        // if the Hediff fails...
        if (!(rand <= Def.AddHediffChance))
        {
            return;
        }

        Messages.Message("JJH_Bola_SuccessMessage".Translate(launcher.Label, hitPawn.Label),
            MessageTypeDefOf.NeutralEvent);

        var anesthesiaOnPawn = hitPawn.health?.hediffSet?.GetFirstHediffOfDef(Def.HediffToAdd);

        var randomSeverity = Rand.Range(0.15f, 0.30f);

        if (anesthesiaOnPawn != null)
        {
            anesthesiaOnPawn.Severity += randomSeverity;
        }
        else
        {
            // mote maker handles all small visual effects
            MoteMaker.ThrowText(hitThing.PositionHeld.ToVector3(),
                hitThing.MapHeld, "JJH_Bola_FailureMote".Translate(Def.AddHediffChance), 12f);
        }
    }
}
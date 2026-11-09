using Terraria;
using Terraria.ID;

namespace Abyssal.Modules;

/// <summary>
/// Strikes every hostile NPC inside TShock's NPC strike range each tick, for exactly the health it has left.
/// </summary>
[ModuleInfo("killaura", "Kill Aura", "Kills every enemy within 128 tiles.", ModuleCategory.Combat)]
public sealed class KillAura : Module
{
    // TShock rejects strikes on NPCs more than 128 tiles away on either axis (RangeChecks).
    private const int MaxRangeTiles = 128;

    // TShock disables or kicks for strike damage >= MaxDamage (20000 by default).
    private const int MaxStrikeDamage = 19999;

    // Bounds the packets sent for one NPC per tick; enemies with more health than one strike can take need several.
    private const int MaxStrikesPerTick = 8;

    public static KillAura Instance { get; } = new();

    public override void OnTickEnd()
    {
        if (Main.dedServ)
            return;

        Player player = Main.LocalPlayer;
        if (player.dead)
            return;

        int playerX = (int)(player.position.X / 16f);
        int playerY = (int)(player.position.Y / 16f);

        foreach (NPC npc in Main.npc)
        {
            if (!IsTarget(npc))
                continue;

            // Same measurement TShock uses: top-left tiles, compared per axis.
            if (Math.Abs((int)(npc.position.X / 16f) - playerX) > MaxRangeTiles
                || Math.Abs((int)(npc.position.Y / 16f) - playerY) > MaxRangeTiles)
                continue;

            // Worm segments share the head's health.
            NPC body = npc.realLife >= 0 ? Main.npc[npc.realLife] : npc;
            int direction = npc.Center.X < player.Center.X ? -1 : 1;

            // Strikes lower the client's copy of the health immediately, so each one covers what's left.
            for (int strike = 0; strike < MaxStrikesPerTick && body.active && body.life > 0; strike++)
                npc.StrikeNPC(DamageToKill(npc, body.life), 0f, direction, crit: false, fromNet: false, Main.myPlayer);
        }
    }

    /// <summary>
    /// The strike damage that takes away <paramref name="health"/>, capped at what TShock accepts.
    /// Mirrors NPC.StrikeNPC: damage minus half the defense, scaled by takenDamageMultiplier when above 1.
    /// </summary>
    private static int DamageToKill(NPC npc, int health)
    {
        double needed = health;
        if (npc.takenDamageMultiplier > 1f)
            needed /= npc.takenDamageMultiplier;

        int damage = (int)Math.Ceiling(needed + npc.defense * 0.5);
        return Math.Clamp(damage, 1, MaxStrikeDamage);
    }

    private static bool IsTarget(NPC npc) =>
        npc.active
        && npc.life > 0
        && !npc.friendly
        && !npc.townNPC
        && !npc.dontTakeDamage
        && !npc.immortal
        && npc.lifeMax > 5
        && !NPCID.Sets.CountsAsCritter[npc.type];
}

﻿using CalamityMod.NPCs;
using CalamityMod.World;
using CalamityMod;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.IO;
using Terraria.GameContent;
using Terraria.ID;
using Terraria;
using Terraria.ModLoader;

namespace CalamityOverhaul.Content.NPCs.HeavenEaters
{
    internal class HEBody : ModNPC
    {
        public override string Texture => HEHead.Path + Name;
        public override void SetStaticDefaults() {
            this.HideFromBestiary();
        }

        public override void SetDefaults() {
            NPC.damage = 100;
            NPC.width = 60;
            NPC.height = 88;
            NPC.defense = 0;
            NPC.lifeMax = 300000;
            double HPBoost = CalamityConfig.Instance.BossHealthBoost * 0.01;
            NPC.lifeMax += (int)(NPC.lifeMax * HPBoost);
            NPC.aiStyle = -1;
            AIType = -1;
            NPC.knockBackResist = 0f;
            NPC.Opacity = 0f;
            NPC.behindTiles = true;
            NPC.noGravity = true;
            NPC.noTileCollide = true;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath6;
            NPC.netAlways = true;
            NPC.chaseable = false;
            Music = MusicLoader.GetMusicSlot(Mod, "Assets/Sounds/Music/ConsumedByDarkness");
        }

        public override void SendExtraAI(BinaryWriter writer) {
            writer.Write(NPC.localAI[0]);
        }

        public override void ReceiveExtraAI(BinaryReader reader) {
            NPC.localAI[0] = reader.ReadSingle();
        }

        public override bool? DrawHealthBar(byte hbPosition, ref float scale, ref Vector2 position) => false;

        public override void AI() {
            NPC.damage = 0;

            // Difficulty modes
            bool death = CalamityWorld.death;
            bool revenge = CalamityWorld.revenge;
            bool expertMode = Main.expertMode;

            if (NPC.ai[2] > 0f)
                NPC.realLife = (int)NPC.ai[2];

            // Check if other segments are still alive. If not, die.
            bool shouldDespawn = true;
            int wyrmHeadID = ModContent.NPCType<HEHead>();
            for (int i = 0; i < Main.maxNPCs; i++) {
                if (Main.npc[i].active && Main.npc[i].type == wyrmHeadID) {
                    shouldDespawn = false;
                    break;
                }
            }
            if (!shouldDespawn) {
                if (NPC.ai[1] <= 0f)
                    shouldDespawn = true;
                else if (Main.npc[(int)NPC.ai[1]].life <= 0)
                    shouldDespawn = true;
            }
            if (shouldDespawn) {
                NPC.life = 0;
                NPC.HitEffect(0, 10.0);
                NPC.checkDead();
                NPC.active = false;
            }

            CalamityGlobalNPC calamityGlobalNPC_Head = Main.npc[(int)NPC.ai[2]].Calamity();

            float chargePhaseGateValue = death ? 180f : revenge ? 210f : expertMode ? 240f : 300f;
            float lightningChargePhaseGateValue = death ? 120f : revenge ? 135f : expertMode ? 150f : 180f;

            bool invisiblePartOfChargePhase = calamityGlobalNPC_Head.newAI[2] >= chargePhaseGateValue && calamityGlobalNPC_Head.newAI[2] <= chargePhaseGateValue + 1f && (calamityGlobalNPC_Head.newAI[0] == (float)HEHead.Phase.ChargeOne || calamityGlobalNPC_Head.newAI[0] == (float)HEHead.Phase.ChargeTwo || calamityGlobalNPC_Head.newAI[0] == (float)HEHead.Phase.FastCharge);
            bool invisiblePartOfLightningChargePhase = calamityGlobalNPC_Head.newAI[2] >= lightningChargePhaseGateValue && calamityGlobalNPC_Head.newAI[2] <= lightningChargePhaseGateValue + 1f && calamityGlobalNPC_Head.newAI[0] == (float)HEHead.Phase.LightningCharge;
            bool invisiblePhase = calamityGlobalNPC_Head.newAI[0] == 1f || calamityGlobalNPC_Head.newAI[0] == 5f || calamityGlobalNPC_Head.newAI[0] == 7f;
            if (!invisiblePartOfChargePhase && !invisiblePartOfLightningChargePhase && !invisiblePhase) {
                if (Main.npc[(int)NPC.ai[1]].Opacity > 0.5f) {
                    NPC.Opacity += 0.2f;
                    if (NPC.Opacity > 1f)
                        NPC.Opacity = 1f;
                }
            }
            else {
                NPC.Opacity -= 0.05f;
                if (NPC.Opacity < 0f)
                    NPC.Opacity = 0f;
            }

            bool spawnAncientLights = (calamityGlobalNPC_Head.newAI[0] == (float)HEHead.Phase.ShadowFireballSpin && calamityGlobalNPC_Head.newAI[2] > 0f) ||
                (calamityGlobalNPC_Head.newAI[0] == (float)HEHead.Phase.FinalPhase && calamityGlobalNPC_Head.newAI[1] > 0f);
            if (spawnAncientLights) {
                if (Vector2.Distance(NPC.Center, Main.player[Main.npc[(int)NPC.ai[2]].target].Center) > 160f) {
                    NPC.localAI[0] += 1f;
                    float spawnAncientLightGateValue = death ? 100f : revenge ? 105f : expertMode ? 110f : 120f;
                    float divisor = 4f;
                    if (NPC.ai[3] % divisor == 0f && NPC.localAI[0] >= spawnAncientLightGateValue) {
                        NPC.localAI[0] = 0f;
                        if (Main.netMode != NetmodeID.MultiplayerClient) {
                            float distanceVelocityBoost = MathHelper.Clamp((Vector2.Distance(Main.npc[(int)NPC.ai[2]].Center, Main.player[Main.npc[(int)NPC.ai[2]].target].Center) - 1600f) * 0.025f, 0f, 16f);
                            float lightVelocity = (Main.player[Main.npc[(int)NPC.ai[2]].target].Calamity().ZoneAbyssLayer4 ? 6f : 8f) + distanceVelocityBoost;
                            Vector2 destination = Main.player[Main.npc[(int)NPC.ai[2]].target].Center - NPC.Center;
                            Vector2 velocity = Vector2.Normalize(destination) * lightVelocity;
                            int type = NPCID.AncientLight;
                            float ai = (Main.rand.NextFloat() - 0.5f) * 0.3f * MathHelper.TwoPi / 60f;
                            int light = NPC.NewNPC(NPC.GetSource_FromAI(), (int)NPC.Center.X, (int)NPC.Center.Y, type, 0, 0f, ai, velocity.X, velocity.Y, 255);
                            Main.npc[light].velocity = velocity;
                        }
                    }
                }
            }

            // Decide segment offset stuff.
            NPC aheadSegment = Main.npc[(int)NPC.ai[1]];
            Vector2 directionToNextSegment = aheadSegment.Center - NPC.Center;
            if (aheadSegment.rotation != NPC.rotation) {
                directionToNextSegment = directionToNextSegment.RotatedBy(MathHelper.WrapAngle(aheadSegment.rotation - NPC.rotation) * 0.08f);
                directionToNextSegment = directionToNextSegment.MoveTowards((aheadSegment.rotation - NPC.rotation).ToRotationVector2(), 1f);
            }

            NPC.rotation = directionToNextSegment.ToRotation() + MathHelper.PiOver2;
            NPC.Center = aheadSegment.Center - directionToNextSegment.SafeNormalize(Vector2.Zero) * NPC.scale * NPC.width;
            NPC.spriteDirection = (directionToNextSegment.X > 0).ToDirectionInt();
        }

        public override bool PreDraw(SpriteBatch spriteBatch, Vector2 screenPos, Color drawColor) {
            SpriteEffects spriteEffects = SpriteEffects.None;
            if (NPC.spriteDirection == 1)
                spriteEffects = SpriteEffects.FlipHorizontally;

            Texture2D texture = TextureAssets.Npc[NPC.type].Value;
            Vector2 vector = new Vector2(TextureAssets.Npc[NPC.type].Value.Width / 2, TextureAssets.Npc[NPC.type].Value.Height / Main.npcFrameCount[NPC.type] / 2);

            Vector2 center = NPC.Center - screenPos;
            center -= new Vector2(texture.Width, texture.Height / Main.npcFrameCount[NPC.type]) * NPC.scale / 2f;
            center += vector * NPC.scale + new Vector2(0f, NPC.gfxOffY);
            spriteBatch.Draw(texture, center, NPC.frame, NPC.GetAlpha(drawColor), NPC.rotation, vector, NPC.scale, spriteEffects, 0f);

            return false;
        }

        public override bool CheckActive() => false;

        public override void HitEffect(NPC.HitInfo hit) {
            if (NPC.life <= 0) {
                for (int k = 0; k < 10; k++)
                    Dust.NewDust(NPC.position, NPC.width, NPC.height, 4, hit.HitDirection, -1f, 0, default, 1f);

                if (Main.netMode != NetmodeID.Server) {
                    Gore.NewGore(NPC.GetSource_Death(), NPC.Center, NPC.velocity, ModLoader.GetMod("CalamityMod").Find<ModGore>("DoT2").Type, 1f);
                }
            }
        }

        public override void ModifyTypeName(ref string typeName) {
            if (Main.zenithWorld) {
                typeName = CalamityUtils.GetTextValue("NPCs.Jared");
            }
        }
    }
}
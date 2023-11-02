using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System.Reflection;
using System;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework;
using static MooMooLib.DrawSets;

namespace MooMooLib
{
	public class MooMooLibMod : Mod
	{
        public override void Load()
        {
            On_Main.DrawProj_DrawYoyoString += Test;
        }

        public override void Unload()
        {
            On_Main.DrawProj_DrawYoyoString -= Test;
        }

        private void DrawCustomYoyoString(Projectile projectile, Vector2 mountedCenter, Color textureColor, Asset<Texture2D> texture)
        {
            // Adapted Vanilla Code for drawing custom yoyo strings.
            // Yes, I know it's a horrible sight. No, I will not fix it.

            if (projectile.aiStyle == 99 || DrawSets.CanHaveYoyoStringDrawnFromProjectile[projectile.type])
            {

                if (projectile.counterweight)
                {
                    texture = TextureAssets.FishingLine;
                }

                Player player = Main.player[projectile.owner];

                if (player.stringColor == (int)DrawSets.StringDrawTypes.Invisible)
                {
                    return;
                }
                

                Vector2 vector = mountedCenter;
                vector.Y += player.gfxOffY;

                float num2 = projectile.Center.X - vector.X;
                float num3 = projectile.Center.Y - vector.Y;

                Math.Sqrt((double)(num2 * num2 + num3 * num3));

                if (!projectile.counterweight)
                {
                    int num5 = -1;
                    if (projectile.position.X + projectile.width / 2 < Main.player[projectile.owner].position.X + Main.player[projectile.owner].width / 2)
                    {
                        num5 = 1;
                    }
                    num5 *= -1;
                    player.itemRotation = MathF.Atan2(num3 * num5, num2 * num5);
                }

                bool drawString = true;

                if (num2 == 0f && num3 == 0f)
                {
                    drawString = false;
                }
                else
                {
                    float num6 = (float)Math.Sqrt((double)(num2 * num2 + num3 * num3));
                    num6 = 12f / num6;
                    num2 *= num6;
                    num3 *= num6;
                    vector.X -= num2 * 0.1f;
                    vector.Y -= num3 * 0.1f;
                    num2 = projectile.position.X + projectile.width * 0.5f - vector.X;
                    num3 = projectile.position.Y + projectile.height * 0.5f - vector.Y;
                }
                while (drawString)
                {
                    float num7 = 12f;
                    float num8 = (float)Math.Sqrt((double)(num2 * num2 + num3 * num3));
                    float num9 = num8;

                    if (float.IsNaN(num8) || float.IsNaN(num9))
                    {
                        drawString = false;
                    }

                    else
                    {
                        if (num8 < 20f)
                        {
                            num7 = num8 - 8f;
                            drawString = false;
                        }
                        num8 = 12f / num8;
                        num2 *= num8;
                        num3 *= num8;
                        vector.X += num2;
                        vector.Y += num3;
                        num2 = projectile.position.X + projectile.width * 0.5f - vector.X;
                        num3 = projectile.position.Y + projectile.height * 0.1f - vector.Y;
                        if (num9 > 12f)
                        {
                            float num10 = 0.3f;
                            float num11 = Math.Abs(projectile.velocity.X) + Math.Abs(projectile.velocity.Y);
                            if (num11 > 16f)
                            {
                                num11 = 16f;
                            }
                            num11 = 1f - num11 / 16f;
                            num10 *= num11;
                            num11 = num9 / 80f;
                            if (num11 > 1f)
                            {
                                num11 = 1f;
                            }
                            num10 *= num11;
                            if (num10 < 0f)
                            {
                                num10 = 0f;
                            }
                            num10 *= num11;
                            num10 *= 0.5f; // Intensity of string bending 
                            if (num3 > 0f)
                            {
                                num3 *= 1f + num10;
                                num2 *= 1f - num10;
                            }
                            else
                            {
                                num11 = Math.Abs(projectile.velocity.X) / 3f;
                                if (num11 > 1f)
                                {
                                    num11 = 1f;
                                }
                                num11 -= 0.5f;
                                num10 *= num11;
                                if (num10 > 0f)
                                {
                                    num10 *= 2f;
                                }
                                num3 *= 1f + num10; // Controls vertical climb of the string on the Y+ axis
                                num2 *= 1f - num10;
                            }
                        }

                        float num4 = (float)Math.Atan2((double)num3, (double)num2) - 1.57f;

                        textureColor = Color.White; // Starts textureColor at white. If you don't want to change the color at all, delete the 2 lines below.

                        if (player.stringColor != (int)StringDrawTypes.WhiteRegardlessOfStrings)
                        {
                            // Calling Main.TryApplyingPlayerStringColor with reflection. This allows us to actually make yoyo strings influence color.
                            var method = typeof(Main).GetMethod("TryApplyingPlayerStringColor", BindingFlags.Static | BindingFlags.NonPublic);
                            textureColor = (Color)method.Invoke(null, new object[] { player.stringColor, textureColor });
                            textureColor = WhichColorShouldBeUsed(textureColor, projectile);
                        }

                        textureColor.A = (byte)(textureColor.A * 0.4f);
                        textureColor = Lighting.GetColor((int)vector.X / 16, (int)(vector.Y / 16f), textureColor); // Makes the string use Terraria's lighting system to turn darker / lighter in the appropriate enviornment.

                        float alphaDilation = 0.6f; // Dilates the texture's alpha. Otherwise, it wouldn't look right
                        // Drawing the string itself
                        Color textureDrawColor = new((byte)(textureColor.R * alphaDilation), (byte)(textureColor.G * alphaDilation), (byte)(textureColor.B * alphaDilation), (byte)(textureColor.A * alphaDilation));

                        //if (projectile.type != ProjectileType<BlackHoleProjectile>())
                        Main.EntitySpriteDraw(texture.Value, new Vector2(vector.X - Main.screenPosition.X + texture.Width() * 0.5f,
                            vector.Y - Main.screenPosition.Y + texture.Height() * 0.5f) - new Vector2(6f, 0f),
                            new Rectangle?(new Rectangle(0, 0, texture.Width(), (int)num7)),
                            textureDrawColor * 1.3f, num4, new Vector2(texture.Width() * 0.5f, 0f), 1f, 0, 0);
                    }
                }
            }
        }

        private void Test(On_Main.orig_DrawProj_DrawYoyoString orig, Main self, Projectile projectile, Vector2 mountedCenter)
        {
            Player player = Main.player[projectile.owner];

            MooMooLibModsystem modSystem = new MooMooLibModsystem();

            Asset<Texture2D> texture = modSystem.GetStringFromDictionary(player.HeldItem.type);

            DrawCustomYoyoString(
                projectile,
                mountedCenter,
                Color.White,
                texture
            );
        }

        public Color WhichColorShouldBeUsed(Color textureColor, Projectile projectile)
        {
            MooMooLibModsystem modSystem = new MooMooLibModsystem();
            Color color = modSystem.GetStringColorFromDictionary(textureColor, Main.player[projectile.owner].stringColor);
            return color;

        }
    }
}
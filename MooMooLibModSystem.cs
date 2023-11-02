using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Content;
using System.Collections.Generic;
using Terraria.GameContent;
using Terraria.ModLoader;
namespace MooMooLib
{
    public class MooMooLibModsystem : ModSystem
    {
        public static Dictionary<int, StringTexture> yoyoStringDictionary = new Dictionary<int, StringTexture>();
        public static Dictionary<int, StringColor> yoyoStringColorDictionary = new Dictionary<int, StringColor>();

        /// <summary>
        /// Attempts to get the new color to draw based on the player's yoyo string color.<br>Will return the previous player.stringColor converted into a Color if no entry is found.</br>
        /// </summary>
        public Color GetStringColorFromDictionary(Color prevColor, int stringColor)
        {
            if (yoyoStringColorDictionary.TryGetValue(stringColor, out StringColor color))
            {
                return color.getStringColor();
            }

            return prevColor;
        }

        /// <summary>
        /// Gets the string asset from the dictionary. Returns TextureAssets.FishingLine if no result for the key is found.<br>The key is the ItemID of the player's held item.</br>
        /// </summary>
        public Asset<Texture2D> GetStringFromDictionary(int itemID)
        {
            if (yoyoStringDictionary.TryGetValue(itemID, out StringTexture instance))
            {
                return instance.getStringTexture();
            }

            return TextureAssets.FishingLine;
        }

        public class StringTexture
        {
            public Asset<Texture2D> texture;
            public void setStringTexture(Asset<Texture2D> asttex)
            {
                texture = asttex;
            }

            public Asset<Texture2D> getStringTexture()
            {
                return texture;
            }
        }

        public class StringColor
        {
            public Color color;
            public void setStringColor(Color inColor)
            {
                color = inColor;
            }

            public Color getStringColor()
            {
                return color;
            }
        }
    }
}

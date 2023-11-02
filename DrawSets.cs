using Terraria.ID;

namespace MooMooLib
{
    public class DrawSets
    {
        public static bool[] CanHaveYoyoStringDrawnFromProjectile = ItemID.Sets.Factory.CreateBoolSet();
        public enum StringDrawTypes
        {
            /// <summary>
            /// Does not call any of the string drawing code, making it invisible.
            /// </summary>
            Invisible = -1,
            /// <summary>
            /// Draws the string as white even if the player has yoyo strings equipped
            /// </summary>
            WhiteRegardlessOfStrings = -2
        }
        public enum StringColors
        {
            White = 0,
            Red = 1,
            Orange = 2,
            Yellow = 3,
            Lime = 4,
            Green = 5,
            Teal = 6,
            Cyan = 7,
            SkyBlue = 8,
            Blue = 9,
            Purple = 10,
            Violet = 11,
            Pink = 12,
            Black = 13,
            Rainbow = 27,
            Brown = 28
        }
    }
}

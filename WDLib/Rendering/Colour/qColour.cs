/*  ---------------------------------------------------------------------------------------------------------------------------------------
 *  (C) 2019, Dr Warren Creemers.
 *  This file is subject to the terms and conditions defined in the included file 'LICENSE.txt'
 *  ---------------------------------------------------------------------------------------------------------------------------------------
 */
using System;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using WD_toolbox.Maths.Range;

namespace WD_toolbox.Rendering.Colour
{
    /// <summary>
    /// A class for fast access to colours stored in the win32 (BGRA) format.
    /// This class is very old (created in 1999) and several versions existerd. 
    /// Refactored in 2018.
    /// </summary>
    //[StructLayout(LayoutKind.Sequential, Pack = 8)]
    [Serializable]
    [StructLayout(LayoutKind.Explicit, Pack = 1)]
    public struct QColor : IFormattable, IColour<QColor>, ICloneable, IEquatable<QColor>
    {
        private static Random colorRand = new Random();

        #region data elements
        /// <summary>
        /// Represents the blue component
        /// </summary>
        [FieldOffset(0)]
        public byte b;

        /// <summary>
        /// Represents the green component
        /// </summary>
        [FieldOffset(1)]
        public byte g;

        /// <summary>
        /// Represents the red component
        /// </summary>
        [FieldOffset(2)]
        public byte r;

        /// <summary>
        /// Represents the alpha component (0=transparent, 255=solid)
        /// </summary>
        [FieldOffset(3)]
        public byte a;

        [FieldOffset(0)]
        public int BGRA;
        #endregion

        #region X11Colors
        /// <summary>
        /// X11 Colors
        /// Under the permisive X11 license, as noted in license.txt.
        /// </summary>
        public static class X11Colors
        {
            internal static Dictionary<QColor, string> names = new Dictionary<QColor, string>()
            {
                { AliceBlue, "Alice Blue"}, {AntiqueWhite, "Antique White"}, {Aqua, "Aqua"}, {Aquamarine, "Aquamarine"}, {Azure, "Azure"}, {Beige, "Beige"},
                { Bisque, "Bisque"}, {Black, "Black"}, {BlanchedAlmond, "Blanched Almond"}, {Blue, "Blue"}, {BlueViolet, "Blue Violet"}, {Brown, "Brown"},
                { Burlywood, "Burlywood"}, {CadetBlue, "Cadet Blue"}, {Chartreuse, "Chartreuse"}, {Chocolate, "Chocolate"}, {Coral, "Coral"},
                { Cornflower, "Cornflower"}, {Cornsilk, "Cornsilk"}, {Crimson, "Crimson"}, {Cyan, "Cyan"}, {DarkBlue, "Dark Blue"}, {DarkCyan, "Dark Cyan"},
                { DarkGoldenrod, "Dark Goldenrod"}, {DarkGray, "Dark Gray"}, {DarkGreen, "Dark Green"}, {DarkKhaki, "Dark Khaki"},
                { DarkMagenta, "Dark Magenta"}, {DarkOliveGreen, "Dark Olive Green"}, {DarkOrange, "Dark Orange"}, {DarkOrchid, "Dark Orchid"},
                { DarkRed, "Dark Red"}, {DarkSalmon, "Dark Salmon"}, {DarkSeaGreen, "Dark Sea Green"}, {DarkSlateBlue, "Dark Slate Blue"},
                { DarkSlateGray, "Dark Slate Gray"}, {DarkTurquoise, "Dark Turquoise"}, {DarkViolet, "Dark Violet"}, {DeepPink, "Deep Pink"},
                { DeepSkyBlue, "Deep Sky Blue"}, {DimGray, "Dim Gray"}, {DodgerBlue, "Dodger Blue"}, {Firebrick, "Firebrick"}, {FloralWhite, "Floral White"},
                { ForestGreen, "Forest Green"}, {Fuchsia, "Fuchsia"}, {Gainsboro, "Gainsboro"}, {GhostWhite, "Ghost White"}, {Gold, "Gold"},
                { Goldenrod, "Goldenrod"}, {Gray, "Gray"}, {WebGray, "Web Gray"}, {Green, "Green"}, {WebGreen, "Web Green"}, {GreenYellow, "Green Yellow"},
                { Honeydew, "Honeydew"}, {HotPink, "Hot Pink"}, {IndianRed, "Indian Red"}, {Indigo, "Indigo"}, {Ivory, "Ivory"}, {Khaki, "Khaki"},
                { Lavender, "Lavender"}, {LavenderBlush, "Lavender Blush"}, {LawnGreen, "Lawn Green"}, {LemonChiffon, "Lemon Chiffon"},
                { LightBlue, "Light Blue"}, {LightCoral, "Light Coral"}, {LightCyan, "Light Cyan"}, {LightGoldenrod, "Light Goldenrod"},
                { LightGray, "Light Gray"}, {LightGreen, "Light Green"}, {LightPink, "Light Pink"}, {LightSalmon, "Light Salmon"},
                { LightSeaGreen, "Light Sea Green"}, {LightSkyBlue, "Light Sky Blue"}, {LightSlateGray, "Light Slate Gray"},
                { LightSteelBlue, "Light Steel Blue"}, {LightYellow, "Light Yellow"}, {Lime, "Lime"}, {LimeGreen, "Lime Green"}, {Linen, "Linen"},
                { Magenta, "Magenta"}, {Maroon, "Maroon"}, {WebMaroon, "Web Maroon"}, {MediumAquamarine, "Medium Aquamarine"}, {MediumBlue, "Medium Blue"},
                { MediumOrchid, "Medium Orchid"}, {MediumPurple, "Medium Purple"}, {MediumSeaGreen, "Medium Sea Green"},
                { MediumSlateBlue, "Medium Slate Blue"}, {MediumSpringGreen, "Medium Spring Green"}, {MediumTurquoise, "Medium Turquoise"},
                { MediumVioletRed, "Medium Violet Red"}, {MidnightBlue, "Midnight Blue"}, {MintCream, "Mint Cream"}, {MistyRose, "Misty Rose"},
                { Moccasin, "Moccasin"}, {NavajoWhite, "Navajo White"}, {NavyBlue, "Navy Blue"}, {OldLace, "Old Lace"}, {Olive, "Olive"},
                { OliveDrab, "Olive Drab"}, {Orange, "Orange"}, {OrangeRed, "Orange Red"}, {Orchid, "Orchid"}, {PaleGoldenrod, "Pale Goldenrod"},
                { PaleGreen, "Pale Green"}, {PaleTurquoise, "Pale Turquoise"}, {PaleVioletRed, "Pale Violet Red"}, {PapayaWhip, "Papaya Whip"},
                { PeachPuff, "Peach Puff"}, {Peru, "Peru"}, {Pink, "Pink"}, {Plum, "Plum"}, {PowderBlue, "Powder Blue"}, {Purple, "Purple"},
                { WebPurple, "Web Purple"}, {RebeccaPurple, "Rebecca Purple"}, {Red, "Red"}, {RosyBrown, "Rosy Brown"}, {RoyalBlue, "Royal Blue"},
                { SaddleBrown, "Saddle Brown"}, {Salmon, "Salmon"}, {SandyBrown, "Sandy Brown"}, {SeaGreen, "Sea Green"}, {Seashell, "Seashell"},
                { Sienna, "Sienna"}, {Silver, "Silver"}, {SkyBlue, "Sky Blue"}, {SlateBlue, "Slate Blue"}, {SlateGray, "Slate Gray"}, {Snow, "Snow"},
                { SpringGreen, "Spring Green"}, {SteelBlue, "Steel Blue"}, {Tan, "Tan"}, {Teal, "Teal"}, {Thistle, "Thistle"}, {Tomato, "Tomato"},
                { Turquoise, "Turquoise"}, {Violet, "Violet"}, {Wheat, "Wheat"}, {White, "White"}, {WhiteSmoke, "White Smoke"}, {Yellow, "Yellow"},
                { YellowGreen, "Yellow Green"},
            };

            public static readonly QColor AntiqueWhite = fromRGB(0xFAEBD7);
            public static readonly QColor AliceBlue = fromRGB(0xF0F8FF);
            public static readonly QColor Aqua = fromRGB(0x00FFFF);
            public static readonly QColor Aquamarine = fromRGB(0x7FFFD4);
            public static readonly QColor Azure = fromRGB(0xF0FFFF);
            public static readonly QColor Beige = fromRGB(0xF5F5DC);
            public static readonly QColor Bisque = fromRGB(0xFFE4C4);
            public static readonly QColor Black = fromRGB(0x000000);
            public static readonly QColor BlanchedAlmond = fromRGB(0xFFEBCD);
            public static readonly QColor Blue = fromRGB(0x0000FF);
            public static readonly QColor BlueViolet = fromRGB(0x8A2BE2);
            public static readonly QColor Brown = fromRGB(0xA52A2A);
            public static readonly QColor Burlywood = fromRGB(0xDEB887);
            public static readonly QColor CadetBlue = fromRGB(0x5F9EA0);
            public static readonly QColor Chartreuse = fromRGB(0x7FFF00);
            public static readonly QColor Chocolate = fromRGB(0xD2691E);
            public static readonly QColor Coral = fromRGB(0xFF7F50);
            public static readonly QColor Cornflower = fromRGB(0x6495ED);
            public static readonly QColor Cornsilk = fromRGB(0xFFF8DC);
            public static readonly QColor Crimson = fromRGB(0xDC143C);
            public static readonly QColor Cyan = fromRGB(0x00FFFF);
            public static readonly QColor DarkBlue = fromRGB(0x00008B);
            public static readonly QColor DarkCyan = fromRGB(0x008B8B);
            public static readonly QColor DarkGoldenrod = fromRGB(0xB8860B);
            public static readonly QColor DarkGray = fromRGB(0xA9A9A9);
            public static readonly QColor DarkGreen = fromRGB(0x006400);
            public static readonly QColor DarkKhaki = fromRGB(0xBDB76B);
            public static readonly QColor DarkMagenta = fromRGB(0x8B008B);
            public static readonly QColor DarkOliveGreen = fromRGB(0x556B2F);
            public static readonly QColor DarkOrange = fromRGB(0xFF8C00);
            public static readonly QColor DarkOrchid = fromRGB(0x9932CC);
            public static readonly QColor DarkRed = fromRGB(0x8B0000);
            public static readonly QColor DarkSalmon = fromRGB(0xE9967A);
            public static readonly QColor DarkSeaGreen = fromRGB(0x8FBC8F);
            public static readonly QColor DarkSlateBlue = fromRGB(0x483D8B);
            public static readonly QColor DarkSlateGray = fromRGB(0x2F4F4F);
            public static readonly QColor DarkTurquoise = fromRGB(0x00CED1);
            public static readonly QColor DarkViolet = fromRGB(0x9400D3);
            public static readonly QColor DeepPink = fromRGB(0xFF1493);
            public static readonly QColor DeepSkyBlue = fromRGB(0x00BFFF);
            public static readonly QColor DimGray = fromRGB(0x696969);
            public static readonly QColor DodgerBlue = fromRGB(0x1E90FF);
            public static readonly QColor Firebrick = fromRGB(0xB22222);
            public static readonly QColor FloralWhite = fromRGB(0xFFFAF0);
            public static readonly QColor ForestGreen = fromRGB(0x228B22);
            public static readonly QColor Fuchsia = fromRGB(0xFF00FF);
            public static readonly QColor Gainsboro = fromRGB(0xDCDCDC);
            public static readonly QColor GhostWhite = fromRGB(0xF8F8FF);
            public static readonly QColor Gold = fromRGB(0xFFD700);
            public static readonly QColor Goldenrod = fromRGB(0xDAA520);
            public static readonly QColor Gray = fromRGB(0xBEBEBE);
            public static readonly QColor WebGray = fromRGB(0x808080);
            public static readonly QColor Green = fromRGB(0x00FF00);
            public static readonly QColor WebGreen = fromRGB(0x008000);
            public static readonly QColor GreenYellow = fromRGB(0xADFF2F);
            public static readonly QColor Honeydew = fromRGB(0xF0FFF0);
            public static readonly QColor HotPink = fromRGB(0xFF69B4);
            public static readonly QColor IndianRed = fromRGB(0xCD5C5C);
            public static readonly QColor Indigo = fromRGB(0x4B0082);
            public static readonly QColor Ivory = fromRGB(0xFFFFF0);
            public static readonly QColor Khaki = fromRGB(0xF0E68C);
            public static readonly QColor Lavender = fromRGB(0xE6E6FA);
            public static readonly QColor LavenderBlush = fromRGB(0xFFF0F5);
            public static readonly QColor LawnGreen = fromRGB(0x7CFC00);
            public static readonly QColor LemonChiffon = fromRGB(0xFFFACD);
            public static readonly QColor LightBlue = fromRGB(0xADD8E6);
            public static readonly QColor LightCoral = fromRGB(0xF08080);
            public static readonly QColor LightCyan = fromRGB(0xE0FFFF);
            public static readonly QColor LightGoldenrod = fromRGB(0xFAFAD2);
            public static readonly QColor LightGray = fromRGB(0xD3D3D3);
            public static readonly QColor LightGreen = fromRGB(0x90EE90);
            public static readonly QColor LightPink = fromRGB(0xFFB6C1);
            public static readonly QColor LightSalmon = fromRGB(0xFFA07A);
            public static readonly QColor LightSeaGreen = fromRGB(0x20B2AA);
            public static readonly QColor LightSkyBlue = fromRGB(0x87CEFA);
            public static readonly QColor LightSlateGray = fromRGB(0x778899);
            public static readonly QColor LightSteelBlue = fromRGB(0xB0C4DE);
            public static readonly QColor LightYellow = fromRGB(0xFFFFE0);
            public static readonly QColor Lime = fromRGB(0x00FF00);
            public static readonly QColor LimeGreen = fromRGB(0x32CD32);
            public static readonly QColor Linen = fromRGB(0xFAF0E6);
            public static readonly QColor Magenta = fromRGB(0xFF00FF);
            public static readonly QColor Maroon = fromRGB(0xB03060);
            public static readonly QColor WebMaroon = fromRGB(0x7F0000);
            public static readonly QColor MediumAquamarine = fromRGB(0x66CDAA);
            public static readonly QColor MediumBlue = fromRGB(0x0000CD);
            public static readonly QColor MediumOrchid = fromRGB(0xBA55D3);
            public static readonly QColor MediumPurple = fromRGB(0x9370DB);
            public static readonly QColor MediumSeaGreen = fromRGB(0x3CB371);
            public static readonly QColor MediumSlateBlue = fromRGB(0x7B68EE);
            public static readonly QColor MediumSpringGreen = fromRGB(0x00FA9A);
            public static readonly QColor MediumTurquoise = fromRGB(0x48D1CC);
            public static readonly QColor MediumVioletRed = fromRGB(0xC71585);
            public static readonly QColor MidnightBlue = fromRGB(0x191970);
            public static readonly QColor MintCream = fromRGB(0xF5FFFA);
            public static readonly QColor MistyRose = fromRGB(0xFFE4E1);
            public static readonly QColor Moccasin = fromRGB(0xFFE4B5);
            public static readonly QColor NavajoWhite = fromRGB(0xFFDEAD);
            public static readonly QColor NavyBlue = fromRGB(0x000080);
            public static readonly QColor OldLace = fromRGB(0xFDF5E6);
            public static readonly QColor Olive = fromRGB(0x808000);
            public static readonly QColor OliveDrab = fromRGB(0x6B8E23);
            public static readonly QColor Orange = fromRGB(0xFFA500);
            public static readonly QColor OrangeRed = fromRGB(0xFF4500);
            public static readonly QColor Orchid = fromRGB(0xDA70D6);
            public static readonly QColor PaleGoldenrod = fromRGB(0xEEE8AA);
            public static readonly QColor PaleGreen = fromRGB(0x98FB98);
            public static readonly QColor PaleTurquoise = fromRGB(0xAFEEEE);
            public static readonly QColor PaleVioletRed = fromRGB(0xDB7093);
            public static readonly QColor PapayaWhip = fromRGB(0xFFEFD5);
            public static readonly QColor PeachPuff = fromRGB(0xFFDAB9);
            public static readonly QColor Peru = fromRGB(0xCD853F);
            public static readonly QColor Pink = fromRGB(0xFFC0CB);
            public static readonly QColor Plum = fromRGB(0xDDA0DD);
            public static readonly QColor PowderBlue = fromRGB(0xB0E0E6);
            public static readonly QColor Purple = fromRGB(0xA020F0);
            public static readonly QColor WebPurple = fromRGB(0x7F007F);
            public static readonly QColor RebeccaPurple = fromRGB(0x663399);
            public static readonly QColor Red = fromRGB(0xFF0000);
            public static readonly QColor RosyBrown = fromRGB(0xBC8F8F);
            public static readonly QColor RoyalBlue = fromRGB(0x4169E1);
            public static readonly QColor SaddleBrown = fromRGB(0x8B4513);
            public static readonly QColor Salmon = fromRGB(0xFA8072);
            public static readonly QColor SandyBrown = fromRGB(0xF4A460);
            public static readonly QColor SeaGreen = fromRGB(0x2E8B57);
            public static readonly QColor Seashell = fromRGB(0xFFF5EE);
            public static readonly QColor Sienna = fromRGB(0xA0522D);
            public static readonly QColor Silver = fromRGB(0xC0C0C0);
            public static readonly QColor SkyBlue = fromRGB(0x87CEEB);
            public static readonly QColor SlateBlue = fromRGB(0x6A5ACD);
            public static readonly QColor SlateGray = fromRGB(0x708090);
            public static readonly QColor Snow = fromRGB(0xFFFAFA);
            public static readonly QColor SpringGreen = fromRGB(0x00FF7F);
            public static readonly QColor SteelBlue = fromRGB(0x4682B4);
            public static readonly QColor Tan = fromRGB(0xD2B48C);
            public static readonly QColor Teal = fromRGB(0x008080);
            public static readonly QColor Thistle = fromRGB(0xD8BFD8);
            public static readonly QColor Tomato = fromRGB(0xFF6347);
            public static readonly QColor Turquoise = fromRGB(0x40E0D0);
            public static readonly QColor Violet = fromRGB(0xEE82EE);
            public static readonly QColor Wheat = fromRGB(0xF5DEB3);
            public static readonly QColor White = fromRGB(0xFFFFFF);
            public static readonly QColor WhiteSmoke = fromRGB(0xF5F5F5);
            public static readonly QColor Yellow = fromRGB(0xFFFF00);
            public static readonly QColor YellowGreen = fromRGB(0x9ACD32);
        }
        #endregion

        #region static from* methods
        /// <summary>
        /// Creates a new qColour
        /// </summary>
        /// <param name="red">0-255</param>
        /// <param name="green">0-255</param>
        /// <param name="blue">0-255</param>
        /// <returns></returns>
        public static QColor fromRGB(int red, int green, int blue)
        {
            QColor c = new QColor();
            c.b = (byte)blue;
            c.g = (byte)green;
            c.r = (byte)red;
            c.a = 255;
            return c;
        }

        /// <summary>
        /// Creates a new Qcolor.
        /// </summary>
        /// <param name="RGB">As per a hex color code.</param>
        /// <returns></returns>
        public static QColor fromRGB(int RGB)
        {
            QColor c = new QColor();
            c.BGRA = RGB;
            c.a = 255;
            return c;
        }

        /// <summary>
        /// Creates a new qColour
        /// </summary>
        /// <param name="red">0-255</param>
        /// <param name="green">0-255</param>
        /// <param name="blue">0-255</param>
        /// <param name="alpha">0-255 (0=transparent, 255=solid)</param>
        /// <returns></returns>
        public static QColor fromRGBA(int red, int green, int blue, int alpha)
        {
            QColor c = new QColor();
            c.b = (byte)blue;
            c.g = (byte)green;
            c.r = (byte)red;
            c.a = (byte)alpha;
            return c;
        }

        /// <summary>
        /// Creates a new qColour
        /// </summary>
        /// <param name="H">Hue</param>
        /// <param name="S">Saturation</param>
        /// <param name="V">Value</param>
        /// <returns></returns>
        public static QColor fromHSV(float H, float S, float V)
        {
            return fromHSVA(H, S, V, 255);
        }
        /// <summary>
        /// Creates a new qColour
        /// </summary>
        /// <param name="H">Hue</param>
        /// <param name="S">Saturation</param>
        /// <param name="V">Value</param>
        /// <param name="alpha">0-255 (0=transparent, 255=solid)</param>
        /// <returns></returns>
        public static QColor fromHSVA(float H, float S, float V, byte alpha)
        {
            QColor c = new QColor();
            c.a = alpha;
            if (S == 0)                       //HSV values = From 0 to 1
            {
                c.r = (byte)(V * 255.0f);                      //RGB results = From 0 to 255
                c.g = (byte)(V * 255.0f);
                c.b = (byte)(V * 255.0f);
            }
            else
            {
                float var_h = H * 6;
                float var_i = (int)var_h;             //Or ... var_i = floor( var_h )
                float var_1 = V * (1 - S);
                float var_2 = V * (1 - S * (var_h - var_i));
                float var_3 = V * (1 - S * (1 - (var_h - var_i)));

                float var_r = V, var_g = var_1, var_b = var_2;
                if (var_i == 0) { var_r = V; var_g = var_3; var_b = var_1; }
                else if (var_i == 1) { var_r = var_2; var_g = V; var_b = var_1; }
                else if (var_i == 2) { var_r = var_1; var_g = V; var_b = var_3; }
                else if (var_i == 3) { var_r = var_1; var_g = var_2; var_b = V; }
                else if (var_i == 4) { var_r = var_3; var_g = var_1; var_b = V; }
                //else                   { var_r = V     ; var_g = var_1 ; var_b = var_2; }

                c.r = (byte)(var_r * 255.0f);                  //RGB results = From 0 to 255
                c.g = (byte)(var_g * 255.0f);
                c.b = (byte)(var_b * 255.0f);
            }
            return c;
        }

        /// <summary>
        /// Creates a colour from a string, Returns black when an error occurs
        /// String can be int the form (i)(i a)(r g b)(r g b a) OR a prenamed color string 
        /// </summary>
        /// <param name="s">String to parse</param>
        /// <returns></returns>
        public static QColor fromString(string s)
        {
            try
            {
                string[] elements = s.Trim().Replace("  ", " ").Split(" ".ToCharArray());

                if (s.IndexOfAny("0123456789".ToCharArray()) != -1)
                {
                    //numerical information string
                    switch (elements.Length)
                    {
                        case 0:
                            return QColor.fromRGB(0, 0, 0);
                        case 1:
                            return QColor.fromRGB(byte.Parse(elements[0]), byte.Parse(elements[0]), byte.Parse(elements[0]));
                        case 2:
                            //assume c c c a (intensity + opacity)
                            return QColor.fromRGBA(byte.Parse(elements[0]), byte.Parse(elements[0]), byte.Parse(elements[0]), byte.Parse(elements[1]));
                        case 3:
                            return QColor.fromRGB(byte.Parse(elements[0]), byte.Parse(elements[1]), byte.Parse(elements[2]));
                        default:
                            //four or more
                            return QColor.fromRGBA(byte.Parse(elements[0]), byte.Parse(elements[1]), byte.Parse(elements[2]), byte.Parse(elements[2]));
                    }
                }
                else
                {
                    throw new NotImplementedException();
                    /*
                    //non numerical string
                    QColor c = QColor.fromRGB(0, 0, 0);
                    Func<string, string> norm = S => S.ToLower().Replace(" ", "").Trim();
                    String key = s;
                    //X11Colors.names.s

                    int i;


                    for (i = 0; i < colorNames.Length; i++)
                    {
                        if (lowerCase.CompareTo(colorNames[i]) == 0)
                        {
                            unsafe
                            {
                                c.r = colorValues[i * 3];
                                c.g = colorValues[i * 3 + 1];
                                c.b = colorValues[i * 3 + 2];
                                c.a = 255;
                            }
                        }
                    }
                    
                    return c;

                    */
                }
            }
            catch
            {
                return QColor.fromRGB(0, 0, 0);
            }
        }

        #endregion

        #region operators
        /// <summary>
        /// Cast to a windows colour
        /// </summary>
        /// <param name="c">a qColor</param>
        /// <returns>a windows colour</returns>
        public static unsafe explicit operator System.Drawing.Color(QColor c)
        {
            //FIX:
            return System.Drawing.Color.FromArgb(*((int*)&c.b));
            //return Color.FromArgb(c.a, c.r, c.g, c.b);
        }

        /// <summary>
        /// Cast from a windows colour
        /// </summary>
        /// <param name="c">a windows colour</param>
        /// <returns>a qColor</returns>
        public static implicit operator QColor(System.Drawing.Color c)
        {
            QColor r = new QColor();
            r.a = c.A;
            r.r = c.R;
            r.g = c.G;
            r.b = c.B;
            return r;
        }



        #endregion

        #region internal functions
        private float min(float a, float b, float c)
        {
            return min(min(a, b), c);
        }
        private float min(float a, float b)
        {
            return (a < b) ? a : b;
        }

        private float max(float a, float b, float c)
        {
            return max(max(a, b), c);
        }
        private float max(float a, float b)
        {
            return (a > b) ? a : b;
        }

        #endregion

        #region to* converters
        /// <summary>
        /// Returns Hue saturation and value information
        /// </summary>
        /// <param name="H">Hue</param>
        /// <param name="S">Saturation</param>
        /// <param name="V">Value</param>
        public void toHSV(out float H, out float S, out float V)
        {
            float var_R = ((float)r / 255.0f);                     //RGB values = From 0 to 255
            float var_G = ((float)g / 255.0f);
            float var_B = ((float)b / 255.0f);

            float var_Min = min(var_R, var_G, var_B);    //Min. value of RGB
            float var_Max = max(var_R, var_G, var_B);    //Max. value of RGB
            float del_Max = var_Max - var_Min;             //Delta RGB value

            V = var_Max;

            if (del_Max == 0)                     //This is a gray, no chroma...
            {
                H = 0;                                //HSV results = From 0 to 1
                S = 0;
            }
            else                                    //Chromatic data...
            {
                S = del_Max / var_Max;

                float del_R = (((var_Max - var_R) / 6) + (del_Max / 2)) / del_Max;
                float del_G = (((var_Max - var_G) / 6) + (del_Max / 2)) / del_Max;
                float del_B = (((var_Max - var_B) / 6) + (del_Max / 2)) / del_Max;

                if (var_R == var_Max) H = del_B - del_G;
                else if (var_G == var_Max) H = (1 / 3) + del_R - del_B;
                else //if ( var_B == var_Max ) 
                    H = (2 / 3) + del_G - del_R;

                if (H < 0) H += 1;
                if (H > 1) H -= 1;
            }
        }

        /// <summary>
        /// Returns a string representing the colour.
        /// </summary>
        /// <returns>A sring in the format "r g b a"</returns>
        public override string ToString()
        {
            return String.Format("{0} {1} {2} {3}", r, g, b, a);
        }

        /// <summary>
        /// Returns the english name of the colour.
        /// </summary>
        /// <returns>An named version of the colour or custom if no name exists.</returns>
        public string toName()
        {
            int i;
            /*
            for (i = 0; i < colorValues.Length; i += 3)
            {
                if (colorValues[i] == r)
                    if (colorValues[i + 1] == g)
                        if (colorValues[i + 2] == b)
                        {
                            return colorNames[i / 3];
                        }
            }
            */
            return "custom";
        }
        #endregion

        #region random generation
        /// <summary>
        /// Generates an array of random colours (with random opacity).
        /// </summary>
        /// <param name="amount">Number of colours to generate.</param>
        /// <returns>An array of random colours.</returns>
        public static QColor[] generateRandomColors(int amount)
        {
            Random r = new Random();
            return generateRandomColors(ref r, amount);
        }

        /// <summary>
        /// Generates an array of random colours (with random opacity).
        /// </summary>
        /// <param name="seed">seed value for random colour generation.</param>
        /// <param name="amount">Number of colours to generate.</param>
        /// <returns>An array of random colours.</returns>
        public static QColor[] generateRandomColors(int seed, int amount)
        {
            Random r = new Random(seed);
            return generateRandomColors(ref r, amount);
        }

        /// <summary>
        /// Generates an array of random colours (with random opacity).
        /// </summary>
        /// <param name="rGenerator">A random number generator to use.</param>
        /// <param name="amount">Number of colours to generate.</param>
        /// <returns>An array of random colours.</returns>
        public static QColor[] generateRandomColors(ref System.Random rGenerator, int amount)
        {
            QColor[] colors = new QColor[amount];
            int i;
            for (i = 0; i < amount; i++)
                colors[i] = generateRandom(rGenerator);
            return colors;
        }

        /// <summary>
        /// Generates an array of random colours (with full opacity).
        /// </summary>
        /// <param name="amount"></param>
        /// <returns></returns>
        public static QColor[] generateRandomColorsOpaque(int amount)
        {
            Random r = new Random();
            return generateRandomColorsOpaque(ref r, amount);
        }

        /// <summary>
        /// Generates an array of random colours (with full opacity).
        /// </summary>
        /// <param name="seed"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        public static QColor[] generateRandomColorsOpaque(int seed, int amount)
        {
            Random r = new Random(seed);
            return generateRandomColorsOpaque(ref r, amount);
        }

        /// <summary>
        /// Generates an array of random colours (with full opacity).
        /// </summary>
        /// <param name="rGenerator"></param>
        /// <param name="amount"></param>
        /// <returns></returns>
        public static QColor[] generateRandomColorsOpaque(ref System.Random rGenerator, int amount)
        {
            QColor[] colors = new QColor[amount];
            int i;
            for (i = 0; i < amount; i++)
                colors[i] = generateRandomOpaque(ref rGenerator);
            return colors;
        }


        /// <summary>
        /// Generates an array of random colours (with random opacity).
        /// </summary>
        /// <returns></returns>
        public static QColor generateRandom()
        {
            return generateRandom(QColor.colorRand);
        }
        /// <summary>
        /// Generates an array of random colours (with random opacity).
        /// </summary>
        /// <param name="rGenerator"></param>
        /// <returns></returns>
        public static QColor generateRandom(System.Random rGenerator)
        {
            return QColor.fromRGBA(rGenerator.Next(0, 255), rGenerator.Next(0, 255), rGenerator.Next(0, 255), rGenerator.Next(0, 255));
        }

        /// <summary>
        /// Generates an array of random colours (with random opacity).
        /// </summary>
        /// <returns></returns>
        public static QColor generateRandomOpaque(int min, int max)
        {
            min = Range.clamp(min, 0, 255);
            max = Range.clamp(max, 0, 255);
            return QColor.fromRGBA(QColor.colorRand.Next(min, max), QColor.colorRand.Next(min, max), QColor.colorRand.Next(min, max), 255);
        }


        /// <summary>
        /// Generates an array of random colours (with full opacity).
        /// </summary>
        /// <param name="rGenerator"></param>
        /// <returns></returns>
        public static QColor generateRandomOpaque(ref System.Random rGenerator)
        {
            return QColor.fromRGB(rGenerator.Next(0, 255), rGenerator.Next(0, 255), rGenerator.Next(0, 255));
        }

        #endregion

        #region IFormattable Members
        /// <summary>
        /// Returns a string representing the colour.
        /// </summary>
        /// <param name="format">unused</param>
        /// <param name="formatProvider">an IFormatProvider, used when turning byte values into strings.</param>
        /// <returns>A sring in the format "r g b a"</returns>
        string System.IFormattable.ToString(string format, IFormatProvider formatProvider)
        {
            return String.Format(formatProvider, "{0} {1} {2} {3}", r, g, b, a);
        }

        #endregion

        #region mixing
        public static QColor mix(QColor a, QColor b, float per)
        {
            QColor c = new QColor();
            c.a = (byte)(a.a + (byte)((b.a - a.a) * per));
            c.r = (byte)(a.r + (byte)((b.r - a.r) * per));
            c.g = (byte)(a.g + (byte)((b.g - a.g) * per));
            c.b = (byte)(a.b + (byte)((b.b - a.b) * per));
            return c;
        }

        public QColor mix(QColor a, float per)
        {
            return QColor.mix(this, a, per);
        }


        public static QColor rgbMix(QColor a, QColor b, float per)
        {
            QColor c = new QColor();
            c.a = a.a;
            c.r = (byte)(a.r + (byte)((b.r - a.r) * per));
            c.g = (byte)(a.g + (byte)((b.g - a.g) * per));
            c.b = (byte)(a.b + (byte)((b.b - a.b) * per));
            return c;
        }

        public QColor rgbMix(QColor a, float per)
        {
            return QColor.rgbMix(this, a, per);
        }

        #endregion

        #region helpers
        private static int abs(int a, int b)
        {
            if (a > b)
                return a - b;
            else
                return b - a;
        }
        #endregion

        #region stats
        /// <summary>
        /// Calculates the colorant contrast of a colour. This is a metric used in many colour calculations.
        /// </summary>
        /// <param name="b">A colour to be mesured.</param>
        /// <returns>The Colorant Contrast of a colour.</returns>
        public float mesureColorantContrast(QColor b)
        {
            return (QColor.abs(r, b.r) + QColor.abs(g, b.g) + QColor.abs(this.b, b.b)) * (1.0f / (3.0f * 255.0f));
        }

        #endregion

        public object Clone()
        {
            return QColor.fromRGBA(r, g, b, a);
        }

        public QColor SainClone()
        {
            return QColor.fromRGBA(r, g, b, a);
        }

        public bool Equals(QColor other)
        {
            return (r == other.r) && (g == other.g) && (b == other.b) && (a == other.a);
        }
    }
}

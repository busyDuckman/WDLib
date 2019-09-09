/*  ---------------------------------------------------------------------------------------------------------------------------------------
 *  (C) 2019, Dr Warren Creemers.
 *  This file is subject to the terms and conditions defined in the included file 'LICENSE.txt'
 *  ---------------------------------------------------------------------------------------------------------------------------------------
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WD_toolbox.Data.DataStructures;
using WD_toolbox.Files;
using WD_toolbox;
using WD_toolbox.Maths.Geometry;
using WD_toolbox.Data;
using System.Drawing;
using System.IO;

namespace WDMedia.SceneGraph.Sprites
{
    public class SpriteSettingsFile : WDSettingsFile<LayeredSprite>
    {
        public Func<string, string> ResolveAbsoluteFileName = S => S;

        enum ReadMode {multilayer_anims_in_sheets, list_of_simple_sheets};
        ReadMode readMode = ReadMode.list_of_simple_sheets;

        string layerName;
        string[] layerFrames;
        bool layerPreCompile;
        List<string> layerParsedImagePaths;

        public string PreCompiledDir { get; set; }

        public SpriteSettingsFile()
        {
            //Value = new LayeredSprite();
            layerParsedImagePaths = new List<string>();
            layerFrames = new string[0];
        }

        public override Why parsePragma(string pragma, string args)
        {
            List<KeyValuePair<string, string>> options = parseArgs(args);
            if (options == null) //error (empty list if no args)
            {
                return Why.FalseBecause("Could not parse option set (eg op1=x; opt2=y), got:", args);
            }

            if(pragma.EqualsIgnoreCaseTrimmed("sprite"))
            {
                int width, height;

                string name = getOption(options, "name")??"un-named";
                
                //frameSize = 32, 32; frameRate=FRAME_RATE
                int[] nums = getOption(options, "framesize").ParseAllIntegers();

                if (nums.Length != 2)
                {
                    return Why.FalseBecause("#frameSize expects two numbers, got {0}.", nums.Length);
                }
                width = nums[0];
                height = nums[1];
                double defaultFps = int.Parse(getOption(options, "defaultfps") ?? "10");

                Value = new LayeredSprite(width, height, defaultFps);
                Value.Name = name;
            }
            else if(pragma.EqualsIgnoreCaseTrimmed("readmode"))
            {
                string mode = getOption(options, "mode") ?? "list_of_simple_sheets";
                readMode = (ReadMode)Enum.Parse(typeof(ReadMode), mode);
            }
            else if (pragma.EqualsIgnoreCaseTrimmed("anim"))
            {
                if (Value == null)
                {
                    return Why.FalseBecause("Not expecting anim definition before sprite is defined.");
                }

                //parse args
                int stancePos = int.Parse(getOption(options, "stancePos")??"-1");
                bool repeat = Misc.DoesStringMeanTrue(getOption(options, "repeat"));
                bool ping = Misc.DoesStringMeanTrue(getOption(options, "ping"));
                bool interuptable = Misc.DoesStringMeanTrue(getOption(options, "interuptable"));
                bool returnToFirstFrame = Misc.DoesStringMeanTrue(getOption(options, "returnToFirstFrame"));
                int frameRate = int.Parse(getOption(options, "frameRate")??"30");
                string name = getOption(options, "name")??"un-named";
                string sheet = getOption(options, "default")??"un-named";
                string set = getOption(options, "set")??"sprite";
                int[] frames = (getOption(options, "frames")??"0").ParseAllIntegers();
                int xMove = int.Parse(getOption(options, "x")??"0");
                int yMove = int.Parse(getOption(options, "y")??"0");

                //create sprite
                double frameMs = 1000.0 / (double)frameRate;
                SpriteAnimation anim = new SpriteAnimation(frames,
                                                            frameMs,
                                                            new Point2D(xMove, yMove));
                Value.Animations.Add(name, anim);
            }
            else if (pragma.EqualsIgnoreCaseTrimmed("layer"))
            {
                if (Value == null)
                {
                    return Why.FalseBecause("Not expecting layer definition before sprite is defined.");
                }
                if (layerParsedImagePaths.Count > 0)
                {
                    return Why.FalseBecause("New layer defined before all sheets in previous layer were defined.");
                }
                //parse layer
                //name, sheets, preCompile
                layerName = getOption(options, "name") ?? "un-named";
                layerFrames = (getOption(options, "sheets") ?? "un-named").Split(",".ToCharArray()).Select(S => S.Trim()).ToArray();
                layerPreCompile = Misc.DoesStringMeanTrue(getOption(options, "preCompile")??"false");
                
                //Bitmap b = Com

                //LayeredSpriteLayer layer = new LayeredSpriteLayer(name, 

                //Value.
            }
            else
            {
                return Why.FalseBecause("unknown pragma ({0})", pragma ?? "NULL");
            }

            return true;
        }

        public override Why parseLine(string line)
        {
            if (string.IsNullOrWhiteSpace(line))
            {
                return true; // blank line = do nothing
            }
            else if (Value == null)
            {
                return Why.FalseBecause("Not expecting regular line (aka a spritesheet) before sprite is defined.");
            }
            else if (layerFrames.Length == 0)
            {
                return Why.FalseBecause("Not expecting (another?) spritesheet.");
            }
            else if (layerParsedImagePaths.Count < layerFrames.Length)
            {
                string filePath = ResolveAbsoluteFileName(line.Trim());
                if(!File.Exists(filePath))
                {
                    return Why.FalseBecause("File not found ({0}).", line.Trim());
                }
                layerParsedImagePaths.Add(filePath);
            }
            
            //no else
            if (layerParsedImagePaths.Count == layerFrames.Length)
            {
                CreateLayerFromImageSet();
                //LayeredSpriteLayer layer = new LayeredSpriteLayer(name, );

                //reset spritesheets
                layerParsedImagePaths = new List<string>();
                layerFrames = new string[0];
            }

            return true;
        }

        private void CreateLayerFromImageSet()
        {
            string preCompiledImageName = null, preCompiledFrameLutName = null;
            Bitmap sheet = null;
            if (layerPreCompile)
            {
                //if any source file is modified, the hash changes and we force a re-compile.
                unchecked
                {
                    int hash = FileHash + layerFrames.Select(L => getNameDateHash(L)).Aggregate(0, (result, element) => result + element);
                    hash = Math.Abs(hash);
                    preCompiledImageName = string.Format("{0}_precompile({2})_{1}.png", layerName, hash, precomplieTag);
                }
                //preCompiledImageName = Path.Combine("PreCompiledSprites", preCompiledImageName);

                preCompiledImageName = Path.Combine((this.PreCompiledDir ?? "PreCompiledSprites"), preCompiledImageName);
                

                if (File.Exists(preCompiledImageName))
                {
                    sheet = ((Bitmap)Bitmap.FromFile(preCompiledImageName)).GetUnFuckedVersion();
                }
                else
                {
                    //clean
                    string pattern = string.Format("{0}_precompile({1})_*.png", layerName, precomplieTag);
                    string[] oldFIles = Directory.GetFiles(Path.GetDirectoryName(preCompiledImageName), pattern);
                    foreach (string file in oldFIles)
	                {
                        File.Delete(file);
	                }
                }
            }

            if (sheet == null)
            {
                SpriteHelper.CombinedSpriteSheet cSprite = SpriteHelper.CombineSpritesSheets(
                                                                            layerParsedImagePaths, 
                                                                            Value.SpriteSize, 
                                                                            preCompiledImageName);
                sheet = cSprite.Image;
            }

            LayeredSpriteLayer layer = new LayeredSpriteLayer(layerName, sheet);
            //for(int i=0; i<Value.Animations)
            //{
            //}
            Value.Layers.Add(layer);
        }

        public override Why parseEndOfFile()
        {
            Value.ReloadFile("");
            return true;
        }

    }
}

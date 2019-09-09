/*  ---------------------------------------------------------------------------------------------------------------------------------------
 *  (C) 2019, Dr Warren Creemers.
 *  This file is subject to the terms and conditions defined in the included file 'LICENSE.txt'
 *  ---------------------------------------------------------------------------------------------------------------------------------------
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using WD_toolbox;
using WD_toolbox.AplicationFramework;
using WDMedia.Rendering;
using WDMedia.SceneGraph.Panes;
using WDMedia.SceneGraph.Sprites;
using WDMedia.Tiles;

namespace WDMedia.SceneGraph
{
    public class World : IPRRenderable, IUpdateable
    {
        public static readonly FileFormat TMX_FORMAT = FileFormat.FromString("TileD TMX [with csv layers] (*.tmx)");

        public List<Pane> Panes {get; protected set;}
        public List<ITileSet> TileSets { get; protected set; }
        public Dictionary<int, ISprite> Sprites { get; protected set; }

        public Dictionary<int, Tile> tileLut;


        public World()
        {
            Panes = new List<Pane>();
            TileSets = new List<ITileSet>();
            Sprites = new Dictionary<int, ISprite>();
        }

        public void loadFile(string path, bool bind)
        {
            if (TMX_FORMAT.Match(path))
            {
                loadTMXFile(path, bind);
            }
        }

        public void loadTMXFile(string path, bool bind)
        {
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(File.ReadAllText(path));
            XmlNode mapNode = xml["map"];

            //get basic info
            int mapWidth = int.Parse(mapNode.Attributes["width"].InnerText);
            int mapHeight = int.Parse(mapNode.Attributes["height"].InnerText);
            int tileWidth = int.Parse(mapNode.Attributes["tilewidth"].InnerText);
            int tileHeight = int.Parse(mapNode.Attributes["tileheight"].InnerText);

            //load world (assumes same sized tiles throughout layers)
            foreach (XmlNode subNode in mapNode)
            {
                if (subNode.Name.EqualsIgnoreCase("tileset"))
                {
                    string imageAtlasFile = subNode["image"].Attributes["source"].InnerText;
                    string imageAtlasPath = Path.Combine(Path.GetDirectoryName(path), imageAtlasFile);
                    int startIndex = int.Parse(subNode.Attributes["firstgid"].InnerText);
                    TileSets.Add(TileSet<Tile>.fromImageAtlas(imageAtlasPath, startIndex, tileWidth, tileHeight));
                    rebuildTileLut(); //(future proofing) do now incase layers ever need to inspect the tiles
                }
                else if (subNode.Name.EqualsIgnoreCase("layer"))
                {
                    string layerFormat = subNode["data"].Attributes["encoding"].InnerText;
                    if(layerFormat.EqualsIgnoreCase("csv"))
                    {
                        //get all numbers from the csv [LIKE A BOSS]
                        //int[] indicies = subNode["data"].InnerText.Split(',').Select(n => int.Parse(n.Trim())).ToArray();
                        int[] indicies = subNode["data"].InnerText.ParseAllIntegers();
                        Panes.Add(new TiledPane(mapWidth, mapHeight, indicies, this));
                    }
                    else
                    {
                        WDAppLog.logError(ErrorLevel.Error, "Unsuported layer format", path);
                    }
                }
                else if (subNode.Name.EqualsIgnoreCase("objectgroup"))
                {

                }
                else if (subNode.Name.EqualsIgnoreCase("imagelayer"))
                {
                    string imageFile = subNode["image"].Attributes["source"].InnerText.Trim();
                    string imagePath = Path.Combine(Path.GetDirectoryName(path), imageFile);
                    Bitmap b = new Bitmap(imagePath);
                    Panes.Add(new ImagePane(b));
                }
            }
        }


        public void rebuildTileLut()
        {
            tileLut = new Dictionary<int, Tile>();
            foreach(ITileSet set in TileSets)
            {
                foreach (Tile tile in set)
                {
                    tileLut.Add(tile.TileID, tile);
                }
            }
        }

        public Tile GetTile(int id)
        {
            Tile tile;
            tileLut.TryGetValue(id, out tile);
            return tile;
        }

        public void Render(I2DPerformanceRenderer renderer, System.Drawing.Rectangle where)
        {
            try
            {
                foreach (Pane pane in Panes)
                {
                    pane.Render(renderer, where);
                    if (pane is TiledPane)
                    {
                        //TiledPane tiledPane = (TiledPane)pane;
                      
                    }
                }
            }
            catch (Exception ex)
            {
                WDAppLog.logException(ErrorLevel.SmallError, ex);
            }
        }

        public void Update(long gameMS)
        {
            foreach (Pane pane in Panes)
            {
                if (pane is IUpdateable)
                {
                    ((IUpdateable)pane).Update(gameMS);
                }
            }
        }
    }
}

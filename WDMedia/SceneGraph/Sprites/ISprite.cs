/*  ---------------------------------------------------------------------------------------------------------------------------------------
 *  (C) 2019, Dr Warren Creemers.
 *  This file is subject to the terms and conditions defined in the included file 'LICENSE.txt'
 *  ---------------------------------------------------------------------------------------------------------------------------------------
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using WDMedia.Rendering;
namespace WDMedia.SceneGraph.Sprites
{
    public enum StandardSpriteAnimations { Idle, Walk, Jump, Attack, Die, Shoot, Duck, Block, Fly };
    public enum StandardSpriteOrientations { None, Up, Down, Left, Right };

    public interface ISprite : IDisposable, IPRImage
    {
        double DefaultFps { get; }
        int DefaultFrameIntervalMS { get; }
        string FileName { get; }
        int SpriteHeight { get; }
        int SpriteWidth { get; }
        Size SpriteSize { get; }
        Dictionary<string, SpriteAnimation> Animations { get; }
        List<Rectangle> FrameBounds  { get;  }
    }
}

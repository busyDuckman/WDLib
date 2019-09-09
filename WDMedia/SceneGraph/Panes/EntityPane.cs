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
using WDMedia.SceneGraph.Sprites;

namespace WDMedia.SceneGraph.Panes
{
    public class EntityPane : Pane, IUpdateable
    {
        public List<Entity> Entities;

        public EntityPane()
        {
            Entities = new List<Entity>();
        }

        public override void Render(Rendering.I2DPerformanceRenderer renderer, System.Drawing.Rectangle where)
        {
            foreach(Entity entity in Entities)
            {
                LayeredSprite lp = entity.Sprite as LayeredSprite;
                if (lp != null)
                {
                    foreach (LayeredSpriteLayer layer in from L in lp.Layers where isOk(L.Name) select L)
                    {
                        renderer.DrawSubImage(layer,
                                          lp.FrameBounds[entity.CurrentSpriteFrame],
                                          (int)entity.Bounds.X,
                                          (int)entity.Bounds.Y);
                    }
                }
                else
                {
                    renderer.DrawSubImage(entity.Sprite,
                                          entity.Sprite.FrameBounds[entity.CurrentSpriteFrame],
                                          (int)entity.Bounds.X,
                                          (int)entity.Bounds.Y);
                }
                /*
                renderer.DrawImage(entity.Sprite,
                                      (int)entity.Bounds.X,
                                      (int)entity.Bounds.Y);
                */
            }
        }

        private bool isOk(string p)
        {
            return p.Contains("leather") || p.Contains("body") || p.Contains("hat") || p.Contains("dagger");
        }

        public void Update(long gameMS)
        {
            foreach (Entity entity in Entities)
            {
                entity.Bounds.Y = entity.Bounds.Y + entity.CurrentAnimation.Movement.Y;
                entity.Bounds.X = entity.Bounds.X + entity.CurrentAnimation.Movement.X;
                entity.Update(gameMS);
            }
        }
    }
}

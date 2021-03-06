﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TClone {
    class Block {
        public static Texture2D texture;
        public bool active = false;

        Rectangle drawDestination;
        public static List<BlockPrefab> prefabs = new List<BlockPrefab>();

        public static readonly BlockPrefab prefabL = new BlockPrefab(new Block[] {
            new Block(new Point(0,0), Color.Yellow),
            new Block(new Point(0,1), Color.Yellow),
            new Block(new Point(0,2), Color.Yellow),
            new Block(new Point(1,2), Color.Yellow),
        },  new Point(0,1));

        public static readonly BlockPrefab prefabLMirror = new BlockPrefab(new Block[] {
            new Block(new Point(1,0), Color.Green),
            new Block(new Point(1,1), Color.Green),
            new Block(new Point(1,2), Color.Green),
            new Block(new Point(0,2), Color.Green),
        }, new Point(1,1));

        public static readonly BlockPrefab prefabT = new BlockPrefab(new Block[] {
            new Block(new Point(0,0), Color.Red),
            new Block(new Point(0,1), Color.Red),
            new Block(new Point(1,1), Color.Red),
            new Block(new Point(0,2), Color.Red),
        },  new Point(0,1));

        public static readonly BlockPrefab prefabZ = new BlockPrefab(new Block[] {
            new Block(new Point(0,0), Color.Brown),
            new Block(new Point(1,0), Color.Brown),
            new Block(new Point(1,1), Color.Brown),
            new Block(new Point(2,1), Color.Brown),
        },  new Point(1,0));

        public static readonly BlockPrefab prefabZMirror = new BlockPrefab(new Block[] {
            new Block(new Point(0,1), Color.Orange),
            new Block(new Point(1,1), Color.Orange),
            new Block(new Point(1,0), Color.Orange),
            new Block(new Point(2,0), Color.Orange),
        },  new Point(1,1));

        public static readonly BlockPrefab prefabLine = new BlockPrefab(new Block[] {
            new Block(new Point(0,0), Color.Blue),
            new Block(new Point(0,1), Color.Blue),
            new Block(new Point(0,2), Color.Blue),
            new Block(new Point(0,3), Color.Blue),
        },  new Point(0,1));

        public static readonly BlockPrefab prefabBlock = new BlockPrefab(new Block[] {
            new Block(new Point(0,0), Color.Violet),
            new Block(new Point(0,1), Color.Violet),
            new Block(new Point(1,0), Color.Violet),
            new Block(new Point(1,1), Color.Violet),
        },  new Point(1,1));

        Point position;
        public Color color;

        public Block(Point position, Color color) {
            SetPosition(position);
            this.color = color;
        }

        public Point GetPosition() {
            return position;
        }

        public void SetPosition(Point newPosition) {
            if (newPosition.X < 0 || newPosition.X >= TClone.WIDTH ||
               newPosition.Y < 0 || newPosition.Y >= TClone.HEIGHT) {
                return;
            }

            position = newPosition;
            drawDestination = new Rectangle(position.X * TClone.TILESIZE, position.Y * TClone.TILESIZE, TClone.TILESIZE, TClone.TILESIZE);
        }

        public void Draw(SpriteBatch sb) {
            sb.Draw(texture, drawDestination, color);
        }

        public void Draw(SpriteBatch sb, Point offset) {
            Rectangle offsetDestination = drawDestination;
            offsetDestination.Location += new Point(offset.X * TClone.TILESIZE, offset.Y * TClone.TILESIZE);
            sb.Draw(texture, offsetDestination, color);
        }

        public struct BlockPrefab {
            public List<Block> blocks;
            public Point origin;

            public BlockPrefab(Block[] blocks, Point origin) {
                this.blocks =  new List<Block>();
                foreach (Block b in blocks) {
                    this.blocks.Add(b);
                }
                this.origin = origin;

                prefabs.Add(this);
            }
        }
    }
}

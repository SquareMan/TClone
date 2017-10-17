using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TClone {
    public class Block {
        public static Texture2D texture;

        Rectangle drawDestination;

        public static BlockPrefab prefab = new BlockPrefab(new Block[] {
            new Block(new Point(0,0), Color.Blue),
            new Block(new Point(0,1), Color.Blue),
            new Block(new Point(0,2), Color.Blue),
            new Block(new Point(1,2), Color.Blue),
        });

        Point position;
        public Color color;

        public Block(Point position, Color color) {
            this.position = position;
            this.color = color;

            drawDestination = new Rectangle(position.X * TClone.TILESIZE, position.Y * TClone.TILESIZE, TClone.TILESIZE, TClone.TILESIZE);
        }

        public Point GetPosition() {
            return position;
        }

        public void SetPosition(Point newPosition) {
            //if(newPosition.X < 0 || newPosition.X >= TClone.WIDTH ||
            //   newPosition.Y < 0 || newPosition.Y >= TClone.HEIGHT) {
            //    return;
            //}

            position = newPosition;
            drawDestination = new Rectangle(position.X * TClone.TILESIZE, position.Y * TClone.TILESIZE, TClone.TILESIZE, TClone.TILESIZE);
        }

        public void Update(float deltaTime) {

        }

        public void Draw(SpriteBatch sb) {
            sb.Draw(texture, drawDestination, color);
        }

        public struct BlockPrefab {
            public List<Block> blocks;

            public BlockPrefab(Block[] blocks) {
                this.blocks =  new List<Block>();
                foreach (Block b in blocks) {
                    this.blocks.Add(b);
                }
            }
        }
    }
}

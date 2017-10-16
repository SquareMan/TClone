using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TClone {
    public class Block {
        public static Texture2D texture;

        public static BlockPrefab prefab = new BlockPrefab( new Block[] {
            new Block(new Point(0,0), Color.Blue),
            new Block(new Point(0,1), Color.Blue)
        });

        public Point position;
        public Color color;

        public Block(Point position, Color color) {
            this.position = position;
            this.color = color;
        }

        public void Update(float deltaTime) {

        }

        public void Draw(SpriteBatch sb) {
            sb.Draw(texture, position.ToVector2() * 16, color);
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

using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TClone {
    static class KeystateHelper {
        public static KeyboardState state;
        static KeyboardState previous;

        public static void Update() {
            previous = state;
            state = Keyboard.GetState();
        }

        public static bool IsKeyReleased(Keys key) {
            return state.IsKeyUp(key) && previous.IsKeyDown(key);
        }
    }
}

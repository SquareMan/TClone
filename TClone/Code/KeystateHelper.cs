using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace TClone {
    static class KeystateHelper {
        public static KeyboardState state;
        static KeyboardState previous;

        public static GamePadState buttonState;
        private static GamePadState previousButtonState;

        public static void Update() {
            previous = state;
            state = Keyboard.GetState();

            previousButtonState = buttonState;
            buttonState = GamePad.GetState(PlayerIndex.One);
        }

        public static bool IsKeyPressed(Keys key)
        {
            return state.IsKeyDown(key) && previous.IsKeyUp(key);
        }

        public static bool IsKeyReleased(Keys key) {
            return state.IsKeyUp(key) && previous.IsKeyDown(key);
        }

        public static bool IsButtonPressed(Buttons button)
        {
            return buttonState.IsButtonDown(button) && previousButtonState.IsButtonUp(button);
        }

        public static bool IsButtonReleased(Buttons button)
        {
            return buttonState.IsButtonUp(button) && previousButtonState.IsButtonDown(button);
        }
    }
}

using System;
using System.Diagnostics;
using System.Drawing;
using SlimDX.DirectInput;

namespace CacheViewer.Domain.Utility
{
    public class UserInput : IDisposable
    {
        private DirectInput directInput;
        private bool isDisposed = false;
        private Keyboard keyboard;
        private KeyboardState keyboardStateCurrent;
        private KeyboardState keyboardStateLast;
        private Mouse mouse;
        private MouseState mouseStateCurrent;
        private MouseState mouseStateLast;

        /// <summary>
        /// The constructor.
        /// </summary>
        public UserInput()
        {
            this.InitDirectInput();

            // We need to intiailize these because otherwise we will get a null reference error
            // if the program tries to access these on the first frame.
            this.keyboardStateCurrent = new KeyboardState();
            this.keyboardStateLast = new KeyboardState();

            this.mouseStateCurrent = new MouseState();
            this.mouseStateLast = new MouseState();
        }

        /// <summary>
        /// Returns a boolean value indicating whether or not this object has been disposed.
        /// </summary>
        public bool IsDisposed
        {
            get { return this.isDisposed; }
        }

        /// <summary>
        /// Gets the keyboard object.
        /// </summary>
        public Keyboard Keyboard
        {
            get { return this.keyboard; }
        }

        /// <summary>
        /// Gets the keyboard state for the current frame.
        /// </summary>
        public KeyboardState KeyboardStateCurrent
        {
            get { return this.keyboardStateCurrent; }
        }

        /// <summary>
        /// Gets the keyboard state from the previous frame.
        /// </summary>
        public KeyboardState KeyboardStatePrevious
        {
            get { return this.keyboardStateLast; }
        }

        /// <summary>
        ///  Gets the mouse object.
        /// </summary>
        public Mouse Mouse
        {
            get { return this.mouse; }
        }

        /// <summary>
        /// Gets the mouse state for the current frame.
        /// </summary>
        public MouseState MouseStateCurrent
        {
            get { return this.mouseStateCurrent; }
        }

        /// <summary>
        /// Gets the mouse state from the previous frame.
        /// </summary>
        public MouseState MouseStatePrevious
        {
            get { return this.mouseStateLast; }
        }

        public void Dispose()
        {
            this.Dispose(true);

            // Since this Dispose() method already cleaned up the resources used by this object, there's no need for the
            // Garbage Collector to call this class's Finalizer, so we tell it not to.
            // We did not implement a Finalizer for this class as in our case we don't need to implement it.
            // The Finalize() method is used to give the object a chance to clean up its unmanaged resources before it
            // is destroyed by the Garbage Collector.  Since we are only using managed code, we do not need to
            // implement the Finalize() method.
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// This method initializes DirectInput.
        /// </summary>
        private void InitDirectInput()
        {
            this.directInput = new DirectInput();
            if (this.directInput == null)
            {
                return;
                // An error has occurred, initialization of DirectInput failed for some reason so simply return from this method.
            }


            // Create our keyboard and mouse devices.
            this.keyboard = new Keyboard(this.directInput);
            if (this.keyboard == null)
            {
                return;
                // An error has occurred, initialization of the keyboard failed for some reason so simply return from this method.
            }

            this.mouse = new Mouse(this.directInput);
        }


        /// <summary>
        /// This function updates the state variables.  It should be called from the game's UpdateScene() function before
        /// it does any input processing.  
        /// </summary>
        public void Update()
        {
            // Reacquire the devices in case another application has taken control of them and check for errors.
            if (this.keyboard.Acquire().IsFailure ||
                this.mouse.Acquire().IsFailure)
            {
                // We failed to successfully acquire one of the devices so abort updating the user input stuff by simply returning from this method.
                return;
            }


            // Update our keyboard state variables.
            this.keyboardStateLast = this.keyboardStateCurrent;
            this.keyboardStateCurrent = this.keyboard.GetCurrentState();


            // NOTE: All of the if statements below are for testing purposes.  In a real program, you would remove them or comment them out
            //       and then recompile before releasing your game.  This is because we don't want debug code slowing down the finished game


            // This is our test code for keyboard input via DirectInput.
            if (this.IsKeyPressed(Key.Space))
            {
                Debug.WriteLine("DIRECTINPUT: KEY SPACE IS PRESSED!");
            }
            if (this.IsKeyHeldDown(Key.Space))
            {
                Debug.WriteLine("DIRECTINPUT: KEY SPACE IS HELD DOWN!");
            }
            if (this.IsKeyPressed(Key.Z))
            {
                Debug.WriteLine("DIRECTINPUT: KEY Z IS PRESSED!");
            }


            // Update our mouse state variables.
            this.mouseStateLast = this.mouseStateCurrent;
            this.mouseStateCurrent = this.mouse.GetCurrentState();

            // This is our test code for mouse input via DirectInput.
            if (this.IsMouseButtonPressed(0))
            {
                Debug.WriteLine("DIRECTINPUT: LEFT MOUSE BUTTON IS PRESSED!");
            }
            if (this.IsMouseButtonPressed(1))
            {
                Debug.WriteLine("DIRECTINPUT: RIGHT MOUSE BUTTON IS PRESSED!");
            }
            if (this.IsMouseButtonPressed(2))
            {
                Debug.WriteLine("DIRECTINPUT: MIDDLE MOUSE BUTTON IS PRESSED!");
            }
        }

        /// <summary>
        /// This method checks if the specified key is pressed.
        /// </summary>
        /// <param name="key">The key to check the state of.</param>
        /// <returns>True if the key is pressed or false otherwise.</returns>
        public bool IsKeyPressed(Key key)
        {
            return this.keyboardStateCurrent.IsPressed(key);
        }

        /// <summary>
        /// This method checks if the specified key was pressed during the previous frame.
        /// </summary>
        /// <param name="key">The key to check the state of.</param>
        /// <returns>True if the key was pressed during the previous frame, or false otherwise.</returns>
        public bool WasKeyPressed(Key key)
        {
            return this.keyboardStateLast.IsPressed(key);
        }

        /// <summary>
        /// This method checks if the specified key is released.
        /// </summary>
        /// <param name="key">The key to check the state of.</param>
        /// <returns>True if the key is released or false otherwise.</returns>
        public bool IsKeyReleased(Key key)
        {
            return this.keyboardStateCurrent.IsReleased(key);
        }

        /// <summary>
        /// This method checks if the specified key was released (not pressed) during the previous frame.
        /// </summary>
        /// <param name="key">The key to check the state of.</param>
        /// <returns>True if the key was not pressed during the previous frame, or false otherwise.</returns>
        public bool WasKeyReleased(Key key)
        {
            return this.keyboardStateLast.IsReleased(key);
        }

        /// <summary>
        /// This method checks if the specified key is held down (meaning it has been held down for 2 or more consecutive frames).
        /// </summary>
        /// <param name="key">The key to check the state of.</param>
        /// <returns>True if the key is being held down or false otherwise.</returns>
        public bool IsKeyHeldDown(Key key)
        {
            return (this.keyboardStateCurrent.IsPressed(key) && this.keyboardStateLast.IsPressed(key));
        }

        /// <summary>
        /// This method checks if the specified mouse button is pressed.
        /// </summary>
        /// <param name="button">The button to check the state of. 0 = left button, 1 = right button, 2 = middle button</param>
        /// <returns>True if the button is pressed or false otherwise.</returns>
        public bool IsMouseButtonPressed(int button)
        {
            return this.mouseStateCurrent.IsPressed(button);
        }

        /// <summary>
        /// This method checks if the specified mouse button was pressed during the previous frame.
        /// </summary>
        /// <param name="button">The button to check the state of. 0 = left button, 1 = right button, 2 = middle button</param>
        /// <returns>True if the button was pressed during the previous frame or false otherwise.</returns>
        public bool WasMouseButtonPressed(int button)
        {
            return this.mouseStateLast.IsPressed(button);
        }

        /// <summary>
        /// This method checks if the specified mouse button is pressed.
        /// </summary>
        /// <param name="button">The button to check the state of. 0 = left button, 1 = right button, 2 = middle button.</param>
        /// <returns>True if the button is released or false otherwise.</returns>
        public bool IsMouseButtonReleased(int button)
        {
            return this.mouseStateCurrent.IsReleased(button);
        }

        /// <summary>
        /// This method checks if the specified mouse button was released (not pressed) during the previous frame.
        /// </summary>
        /// <param name="button">The button to check the state of. 0 = left button, 1 = right button, 2 = middle button</param>
        /// <returns>True if the button was released (not pressed) during the previous frame or false otherwise.</returns>
        public bool WasMouseButtonReleased(int button)
        {
            return this.mouseStateLast.IsReleased(button);
        }

        /// <summary>
        /// This method checks if the specified mouse button is being held down (meaning it has been held down for 2 or more consecutive frames).
        /// </summary>
        /// <param name="button">The button to check the state of. 0 = left button, 1 = right button, 2 = middle button</param>
        /// <returns>True if the button is held down or false otherwise.</returns>
        public bool IsMouseButtonHeldDown(int button)
        {
            return (this.mouseStateCurrent.IsPressed(button) && this.mouseStateLast.IsPressed(button));
        }

        /// <summary>
        /// This method checks if the mouse has moved since the previous frame.
        /// </summary>
        /// <returns>True if the mouse has moved since the previous frame, or false otherwise.</returns>
        public bool MouseHasMoved()
        {
            if ((this.mouseStateCurrent.X != this.mouseStateLast.X) ||
                (this.mouseStateCurrent.Y != this.mouseStateLast.Y))
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// This method gets the mouse position for the current frame.
        /// </summary>
        /// <returns>A System.Drawing.Point object containing the current mouse position.</returns>
        public Point MousePosition()
        {
            return new Point(this.mouseStateCurrent.X, this.mouseStateCurrent.Y);
        }

        /// <summary>
        /// This method gets the mouse position for the previous frame.
        /// </summary>
        /// <returns>A System.Drawing.Point object containing the mouse's position during the previous frame.</returns>
        public Point LastMousePosition()
        {
            return new Point(this.mouseStateLast.X, this.mouseStateLast.Y);
        }

        /// <summary>
        /// This method gets the scrollwheel value in most cases.
        /// Note that this value is a delta, or in other words it is the amount the scroll wheel has been moved
        /// since the last frame.
        /// </summary>
        /// <returns>The amount the scroll wheel has moved.  This can be positive or negative depending on which way it has moved.</returns>
        public int MouseWheelMovement()
        {
            return this.mouseStateCurrent.Z;
        }

        protected void Dispose(bool disposing)
        {
            if (!this.isDisposed)
            {
                /*
                * The following text is from MSDN  (http://msdn.microsoft.com/en-us/library/fs2xkftw%28VS.80%29.aspx)
                * 
                * 
                * Dispose(bool disposing) executes in two distinct scenarios:
                * 
                * If disposing equals true, the method has been called directly or indirectly by a user's code and managed and unmanaged resources can be disposed.
                * If disposing equals false, the method has been called by the runtime from inside the finalizer and only unmanaged resources can be disposed. 
                * 
                * When an object is executing its finalization code, it should not reference other objects, because finalizers do not execute in any particular order. 
                * If an executing finalizer references another object that has already been finalized, the executing finalizer will fail.
                */
                if (disposing)
                {
                    // Unregister events


                    // get rid of managed resources
                    if (this.directInput != null)
                    {
                        this.directInput.Dispose();
                    }

                    if (this.keyboard != null)
                    {
                        this.keyboard.Dispose();
                    }

                    if (this.mouse != null)
                    {
                        this.mouse.Dispose();
                    }
                }

                // get rid of unmanaged resources
            }
        }

    }
}
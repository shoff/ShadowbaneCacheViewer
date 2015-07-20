namespace CacheViewer.Domain.Utility
{
    using System;
    using System.Diagnostics;
    using System.Drawing;
    using SlimDX.DirectInput;

    public class UserInput : IDisposable
    {
        private DirectInput directInput;

        /// <summary>
        /// The constructor.
        /// </summary>
        public UserInput()
        {
            this.InitDirectInput();

            // We need to initialize these because otherwise we will get a null reference error
            // if the program tries to access these on the first frame.
            this.KeyboardStateCurrent = new KeyboardState();
            this.KeyboardStatePrevious = new KeyboardState();
            this.MouseStateCurrent = new MouseState();
            this.MouseStatePrevious = new MouseState();
        }

        /// <summary>
        /// Returns a boolean value indicating whether or not this object has been disposed.
        /// </summary>
        public bool IsDisposed { get; } = false;

        /// <summary>
        /// Gets the keyboard object.
        /// </summary>
        public Keyboard Keyboard { get; private set; }

        /// <summary>
        /// Gets the keyboard state for the current frame.
        /// </summary>
        public KeyboardState KeyboardStateCurrent { get; private set; }

        /// <summary>
        /// Gets the keyboard state from the previous frame.
        /// </summary>
        public KeyboardState KeyboardStatePrevious { get; private set; }

        /// <summary>
        ///  Gets the mouse object.
        /// </summary>
        public Mouse Mouse { get; private set; }

        /// <summary>
        /// Gets the mouse state for the current frame.
        /// </summary>
        public MouseState MouseStateCurrent { get; private set; }

        /// <summary>
        /// Gets the mouse state from the previous frame.
        /// </summary>
        public MouseState MouseStatePrevious { get; private set; }

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
            this.Keyboard = new Keyboard(this.directInput);
            if (this.Keyboard == null)
            {
                return;

                // An error has occurred, initialization of the keyboard failed for some reason so simply return from this method.
            }

            this.Mouse = new Mouse(this.directInput);
        }

        /// <summary>
        /// This function updates the state variables.  It should be called from the game's UpdateScene() function before
        /// it does any input processing.  
        /// </summary>
        public void Update()
        {
            // Reacquire the devices in case another application has taken control of them and check for errors.
            if (this.Keyboard.Acquire().IsFailure || this.Mouse.Acquire().IsFailure)
            {
                // We failed to successfully acquire one of the devices so abort updating the user input stuff by simply returning from this method.
                return;
            }

            // Update our keyboard state variables.
            this.KeyboardStatePrevious = this.KeyboardStateCurrent;
            this.KeyboardStateCurrent = this.Keyboard.GetCurrentState();

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
            this.MouseStatePrevious = this.MouseStateCurrent;
            this.MouseStateCurrent = this.Mouse.GetCurrentState();

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
            return this.KeyboardStateCurrent.IsPressed(key);
        }

        /// <summary>
        /// This method checks if the specified key is held down (meaning it has been held down for 2 or more consecutive frames).
        /// </summary>
        /// <param name="key">The key to check the state of.</param>
        /// <returns>True if the key is being held down or false otherwise.</returns>
        public bool IsKeyHeldDown(Key key)
        {
            return (this.KeyboardStateCurrent.IsPressed(key) && this.KeyboardStatePrevious.IsPressed(key));
        }

        /// <summary>
        /// This method checks if the specified mouse button is pressed.
        /// </summary>
        /// <param name="button">The button to check the state of. 0 = left button, 1 = right button, 2 = middle button</param>
        /// <returns>True if the button is pressed or false otherwise.</returns>
        public bool IsMouseButtonPressed(int button)
        {
            return this.MouseStateCurrent.IsPressed(button);
        }

        /// <summary>
        /// This method checks if the specified mouse button was pressed during the previous frame.
        /// </summary>
        /// <param name="button">The button to check the state of. 0 = left button, 1 = right button, 2 = middle button</param>
        /// <returns>True if the button was pressed during the previous frame or false otherwise.</returns>
        public bool WasMouseButtonPressed(int button)
        {
            return this.MouseStatePrevious.IsPressed(button);
        }

        /// <summary>
        /// This method checks if the specified mouse button is pressed.
        /// </summary>
        /// <param name="button">The button to check the state of. 0 = left button, 1 = right button, 2 = middle button.</param>
        /// <returns>True if the button is released or false otherwise.</returns>
        public bool IsMouseButtonReleased(int button)
        {
            return this.MouseStateCurrent.IsReleased(button);
        }

        /// <summary>
        /// This method checks if the specified mouse button was released (not pressed) during the previous frame.
        /// </summary>
        /// <param name="button">The button to check the state of. 0 = left button, 1 = right button, 2 = middle button</param>
        /// <returns>True if the button was released (not pressed) during the previous frame or false otherwise.</returns>
        public bool WasMouseButtonReleased(int button)
        {
            return this.MouseStatePrevious.IsReleased(button);
        }

        /// <summary>
        /// This method checks if the specified mouse button is being held down (meaning it has been held down for 2 or more consecutive frames).
        /// </summary>
        /// <param name="button">The button to check the state of. 0 = left button, 1 = right button, 2 = middle button</param>
        /// <returns>True if the button is held down or false otherwise.</returns>
        public bool IsMouseButtonHeldDown(int button)
        {
            return (this.MouseStateCurrent.IsPressed(button) && this.MouseStatePrevious.IsPressed(button));
        }

        /// <summary>
        /// This method checks if the mouse has moved since the previous frame.
        /// </summary>
        /// <returns>True if the mouse has moved since the previous frame, or false otherwise.</returns>
        public bool MouseHasMoved()
        {
            if ((this.MouseStateCurrent.X != this.MouseStatePrevious.X) || (this.MouseStateCurrent.Y != this.MouseStatePrevious.Y))
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
            return new Point(this.MouseStateCurrent.X, this.MouseStateCurrent.Y);
        }

        /// <summary>
        /// This method gets the mouse position for the previous frame.
        /// </summary>
        /// <returns>A System.Drawing.Point object containing the mouse's position during the previous frame.</returns>
        public Point LastMousePosition()
        {
            return new Point(this.MouseStatePrevious.X, this.MouseStatePrevious.Y);
        }

        /// <summary>
        /// This method gets the scrollwheel value in most cases.
        /// Note that this value is a delta, or in other words it is the amount the scroll wheel has been moved
        /// since the last frame.
        /// </summary>
        /// <returns>The amount the scroll wheel has moved.  This can be positive or negative depending on which way it has moved.</returns>
        public int MouseWheelMovement()
        {
            return this.MouseStateCurrent.Z;
        }

        protected void Dispose(bool disposing)
        {
            if (!this.IsDisposed)
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
                    this.directInput?.Dispose();
                    this.Keyboard?.Dispose();
                    this.Mouse?.Dispose();
                }

                // get rid of unmanaged resources
            }
        }
    }
}
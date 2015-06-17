using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using SlimDX;
using SlimDX.DirectInput;
using SlimDX.Windows;

namespace CacheViewer.Controls.Viewer
{
    public class CameraComponent : ICamera
    {
        public enum Actions
        {
            FlightYawLeftPrimary,
            FlightYawLeftAlternate,
            FlightYawRightPrimary,
            FlightYawRightAlternate,

            MoveForwardsPrimary,
            MoveForwardsAlternate,
            MoveBackwardsPrimary,
            MoveBackwardsAlternate,

            MoveDownPrimary,
            MoveDownAlternate,
            MoveUpPrimary,
            MoveUpAlternate,

            OrbitRollLeftPrimary,
            OrbitRollLeftAlternate,
            OrbitRollRightPrimary,
            OrbitRollRightAlternate,

            PitchUpPrimary,
            PitchUpAlternate,
            PitchDownPrimary,
            PitchDownAlternate,

            YawLeftPrimary,
            YawLeftAlternate,
            YawRightPrimary,
            YawRightAlternate,

            RollLeftPrimary,
            RollLeftAlternate,
            RollRightPrimary,
            RollRightAlternate,

            StrafeRightPrimary,
            StrafeRightAlternate,
            StrafeLeftPrimary,
            StrafeLeftAlternate
        };

        private const float DEFAULT_ACCELERATION_X = 8.0f;
        private const float DEFAULT_ACCELERATION_Y = 8.0f;
        private const float DEFAULT_ACCELERATION_Z = 8.0f;
        private const float DEFAULT_MOUSE_SMOOTHING_SENSITIVITY = 0.5f;
        private const float DEFAULT_SPEED_FLIGHT_YAW = 100.0f;
        private const float DEFAULT_SPEED_MOUSE_WHEEL = 1.0f;
        private const float DEFAULT_SPEED_ORBIT_ROLL = 100.0f;
        private const float DEFAULT_SPEED_ROTATION = 0.2f;
        private const float DEFAULT_VELOCITY_X = 1.0f;
        private const float DEFAULT_VELOCITY_Y = 1.0f;
        private const float DEFAULT_VELOCITY_Z = 1.0f;

        private const int MOUSE_SMOOTHING_CACHE_SIZE = 10;
        private readonly Dictionary<Actions, Key> actionKeys;

        private readonly Camera camera;
        private readonly RenderForm form;
        private readonly Keyboard keyboard;
        private readonly Mouse mouse;
        private readonly Vector2[] mouseMovement;
        private readonly Vector2[] mouseSmoothingCache;
        private Vector3 acceleration;
        private bool clickAndDragMouseRotation;
        private KeyboardState currentKeyboardState;
        private MouseState currentMouseState;
        private Vector3 currentVelocity;
        private float flightYawSpeed;
        private int mouseIndex;
        private float mouseSmoothingSensitivity;
        private float mouseWheelSpeed;
        private bool movingAlongNegX;
        private bool movingAlongNegY;
        private bool movingAlongNegZ;
        private bool movingAlongPosX;
        private bool movingAlongPosY;
        private bool movingAlongPosZ;
        private float orbitRollSpeed;
        private MouseState previousMouseState;
        private float rotationSpeed;
        private int savedMousePosX;
        private int savedMousePosY;
        private Vector2 smoothedMouseMovement;
        private Vector3 velocity;

        /// <summary>
        ///     Constructs a new instance of the CameraComponent class. The
        ///     camera will have a spectator behavior, and will be initially
        ///     positioned at the world origin looking down the world negative
        ///     z axis. An initial perspective projection matrix is created
        ///     as well as setting up initial key bindings to the actions.
        /// </summary>
        public CameraComponent(RenderForm inForm)
        {
            this.form = inForm;
            DirectInput dInput = new DirectInput();

            this.mouse = new Mouse(dInput);
            this.mouse.SetCooperativeLevel(inForm, CooperativeLevel.Foreground | CooperativeLevel.Nonexclusive);

            this.keyboard = new Keyboard(dInput);
            this.keyboard.SetCooperativeLevel(inForm,
                CooperativeLevel.Foreground | CooperativeLevel.Nonexclusive | CooperativeLevel.NoWinKey);

            this.camera = new Camera();

            this.movingAlongPosX = false;
            this.movingAlongNegX = false;
            this.movingAlongPosY = false;
            this.movingAlongNegY = false;
            this.movingAlongPosZ = false;
            this.movingAlongNegZ = false;

            this.savedMousePosX = -1;
            this.savedMousePosY = -1;

            this.rotationSpeed = DEFAULT_SPEED_ROTATION;
            this.orbitRollSpeed = DEFAULT_SPEED_ORBIT_ROLL;
            this.flightYawSpeed = DEFAULT_SPEED_FLIGHT_YAW;
            this.mouseWheelSpeed = DEFAULT_SPEED_MOUSE_WHEEL;
            this.mouseSmoothingSensitivity = DEFAULT_MOUSE_SMOOTHING_SENSITIVITY;
            this.acceleration = new Vector3(DEFAULT_ACCELERATION_X, DEFAULT_ACCELERATION_Y, DEFAULT_ACCELERATION_Z);
            this.velocity = new Vector3(DEFAULT_VELOCITY_X, DEFAULT_VELOCITY_Y, DEFAULT_VELOCITY_Z);
            this.mouseSmoothingCache = new Vector2[MOUSE_SMOOTHING_CACHE_SIZE];

            this.mouseIndex = 0;
            this.mouseMovement = new Vector2[2];
            this.mouseMovement[0].X = 0.0f;
            this.mouseMovement[0].Y = 0.0f;
            this.mouseMovement[1].X = 0.0f;
            this.mouseMovement[1].Y = 0.0f;

            float aspect = inForm.ClientSize.Width / (float)inForm.ClientSize.Height;

            this.Perspective(Camera.DefaultFovx, aspect, Camera.DefaultZnear, Camera.DefaultZfar);

            this.actionKeys = new Dictionary<Actions, Key>();

            this.actionKeys.Add(Actions.FlightYawLeftPrimary, Key.LeftArrow);
            this.actionKeys.Add(Actions.FlightYawLeftAlternate, Key.A);
            this.actionKeys.Add(Actions.FlightYawRightPrimary, Key.RightArrow);
            this.actionKeys.Add(Actions.FlightYawRightAlternate, Key.D);
            this.actionKeys.Add(Actions.MoveForwardsPrimary, Key.UpArrow);
            this.actionKeys.Add(Actions.MoveForwardsAlternate, Key.W);
            this.actionKeys.Add(Actions.MoveBackwardsPrimary, Key.DownArrow);
            this.actionKeys.Add(Actions.MoveBackwardsAlternate, Key.S);
            this.actionKeys.Add(Actions.MoveDownPrimary, Key.Q);
            this.actionKeys.Add(Actions.MoveDownAlternate, Key.PageDown);
            this.actionKeys.Add(Actions.MoveUpPrimary, Key.E);
            this.actionKeys.Add(Actions.MoveUpAlternate, Key.PageUp);
            this.actionKeys.Add(Actions.OrbitRollLeftPrimary, Key.LeftArrow);
            this.actionKeys.Add(Actions.OrbitRollLeftAlternate, Key.A);
            this.actionKeys.Add(Actions.OrbitRollRightPrimary, Key.RightArrow);
            this.actionKeys.Add(Actions.OrbitRollRightAlternate, Key.D);
            this.actionKeys.Add(Actions.StrafeRightPrimary, Key.RightArrow);
            this.actionKeys.Add(Actions.StrafeRightAlternate, Key.D);
            this.actionKeys.Add(Actions.StrafeLeftPrimary, Key.LeftArrow);
            this.actionKeys.Add(Actions.StrafeLeftAlternate, Key.A);
        }

        /*
        /// <summary>
        /// Initializes the CameraComponent class. This method repositions the
        /// mouse to the center of the game window.
        /// </summary>
        public void Initialize()
        {
            Rectangle clientBounds = Game.Window.ClientBounds;
            mouse.SendData()
            Mouse.SetPosition(clientBounds.Width / 2, clientBounds.Height / 2);
        }
        */

        /// <summary>
        ///     Builds a look at style viewing matrix.
        /// </summary>
        /// <param name="target">The target position to look at.</param>
        public void LookAt(Vector3 target)
        {
            this.camera.LookAt(target);
        }

        /// <summary>
        ///     Builds a look at style viewing matrix.
        /// </summary>
        /// <param name="eye">The camera position.</param>
        /// <param name="target">The target position to look at.</param>
        /// <param name="up">The up direction.</param>
        public void LookAt(Vector3 eye, Vector3 target, Vector3 up)
        {
            this.camera.LookAt(eye, target, up);
        }

        /// <summary>
        ///     Moves the camera by dx world units to the left or right; dy
        ///     world units upwards or downwards; and dz world units forwards
        ///     or backwards.
        /// </summary>
        /// <param name="dx">Distance to move left or right.</param>
        /// <param name="dy">Distance to move up or down.</param>
        /// <param name="dz">Distance to move forwards or backwards.</param>
        public void Move(float dx, float dy, float dz)
        {
            this.camera.Move(dx, dy, dz);
        }

        /// <summary>
        ///     Moves the camera the specified distance in the specified direction.
        /// </summary>
        /// <param name="direction">Direction to move.</param>
        /// <param name="distance">How far to move.</param>
        public void Move(Vector3 direction, Vector3 distance)
        {
            this.camera.Move(direction, distance);
        }

        /// <summary>
        ///     Builds a perspective projection matrix based on a horizontal field
        ///     of view.
        /// </summary>
        /// <param name="fovx">Horizontal field of view in degrees.</param>
        /// <param name="aspect">The viewport's aspect ratio.</param>
        /// <param name="znear">The distance to the near clip plane.</param>
        /// <param name="zfar">The distance to the far clip plane.</param>
        public void Perspective(float fovx, float aspect, float znear, float zfar)
        {
            this.camera.Perspective(fovx, aspect, znear, zfar);
        }

        /// <summary>
        ///     Rotates the camera. Positive angles specify counter clockwise
        ///     rotations when looking down the axis of rotation towards the
        ///     origin.
        /// </summary>
        /// <param name="headingDegrees">Y axis rotation in degrees.</param>
        /// <param name="pitchDegrees">X axis rotation in degrees.</param>
        /// <param name="rollDegrees">Z axis rotation in degrees.</param>
        public void Rotate(float headingDegrees, float pitchDegrees, float rollDegrees)
        {
            this.camera.Rotate(headingDegrees, pitchDegrees, rollDegrees);
        }

        /// <summary>
        ///     Zooms the camera. This method functions differently depending on
        ///     the camera's current behavior. When the camera is orbiting this
        ///     method will move the camera closer to or further away from the
        ///     orbit target. For the other camera behaviors this method will
        ///     change the camera's horizontal field of view.
        /// </summary>
        /// <param name="zoom">
        ///     When orbiting this parameter is how far to move the camera.
        ///     For the other behaviors this parameter is the new horizontal
        ///     field of view.
        /// </param>
        /// <param name="minZoom">
        ///     When orbiting this parameter is the min allowed zoom distance to
        ///     the orbit target. For the other behaviors this parameter is the
        ///     min allowed horizontal field of view.
        /// </param>
        /// <param name="maxZoom">
        ///     When orbiting this parameter is the max allowed zoom distance to
        ///     the orbit target. For the other behaviors this parameter is the max
        ///     allowed horizontal field of view.
        /// </param>
        public void Zoom(float zoom, float minZoom, float maxZoom)
        {
            this.camera.Zoom(zoom, minZoom, maxZoom);
        }

        /// <summary>
        ///     Binds an action to a keyboard key.
        /// </summary>
        /// <param name="action">The action to bind.</param>
        /// <param name="key">The key to map the action to.</param>
        public void MapActionToKey(Actions action, Key key)
        {
            this.actionKeys[action] = key;
        }

        /// <summary>
        ///     Updates the state of the CameraComponent class.
        /// </summary>
        /// <param name="gameTime">Time elapsed since the last call to Update.</param>
        public void Update(float elapsedTimeSec)
        {
            this.UpdateInput();
            this.UpdateCamera(elapsedTimeSec);
        }

        /// <summary>
        ///     Undo any camera rolling by leveling the camera. When the camera is
        ///     orbiting this method will cause the camera to become level with the
        ///     orbit target.
        /// </summary>
        public void UndoRoll()
        {
            this.camera.UndoRoll();
        }

        /// <summary>
        ///     Determines which way to move the camera based on player input.
        ///     The returned values are in the range [-1,1].
        /// </summary>
        /// <param name="direction">The movement direction.</param>
        private void GetMovementDirection(out Vector3 direction)
        {
            direction.X = 0.0f;
            direction.Y = 0.0f;
            direction.Z = 0.0f;

            if (this.currentKeyboardState.PressedKeys.Contains(this.actionKeys[Actions.MoveForwardsPrimary]) ||
                this.currentKeyboardState.PressedKeys.Contains(this.actionKeys[Actions.MoveForwardsAlternate]))
            {
                if (!this.movingAlongNegZ)
                {
                    this.movingAlongNegZ = true;
                    this.currentVelocity.Z = 0.0f;
                }

                direction.Z += 1.0f;
            }
            else
            {
                this.movingAlongNegZ = false;
            }

            if (this.currentKeyboardState.PressedKeys.Contains(this.actionKeys[Actions.MoveBackwardsPrimary]) ||
                this.currentKeyboardState.PressedKeys.Contains(this.actionKeys[Actions.MoveBackwardsAlternate]))
            {
                if (!this.movingAlongPosZ)
                {
                    this.movingAlongPosZ = true;
                    this.currentVelocity.Z = 0.0f;
                }

                direction.Z -= 1.0f;
            }
            else
            {
                this.movingAlongPosZ = false;
            }

            if (this.currentKeyboardState.PressedKeys.Contains(this.actionKeys[Actions.MoveUpPrimary]) ||
                this.currentKeyboardState.PressedKeys.Contains(this.actionKeys[Actions.MoveUpAlternate]))
            {
                if (!this.movingAlongPosY)
                {
                    this.movingAlongPosY = true;
                    this.currentVelocity.Y = 0.0f;
                }

                direction.Y += 1.0f;
            }
            else
            {
                this.movingAlongPosY = false;
            }

            if (this.currentKeyboardState.PressedKeys.Contains(this.actionKeys[Actions.MoveDownPrimary]) ||
                this.currentKeyboardState.PressedKeys.Contains(this.actionKeys[Actions.MoveDownAlternate]))
            {
                if (!this.movingAlongNegY)
                {
                    this.movingAlongNegY = true;
                    this.currentVelocity.Y = 0.0f;
                }

                direction.Y -= 1.0f;
            }
            else
            {
                this.movingAlongNegY = false;
            }

            switch (this.CurrentBehavior)
            {
                case Camera.Behavior.FirstPerson:
                case Camera.Behavior.Spectator:
                    if (this.currentKeyboardState.PressedKeys.Contains(this.actionKeys[Actions.StrafeRightPrimary]) ||
                        this.currentKeyboardState.PressedKeys.Contains(this.actionKeys[Actions.StrafeRightAlternate]))
                    {
                        if (!this.movingAlongPosX)
                        {
                            this.movingAlongPosX = true;
                            this.currentVelocity.X = 0.0f;
                        }

                        direction.X += 1.0f;
                    }
                    else
                    {
                        this.movingAlongPosX = false;
                    }

                    if (this.currentKeyboardState.PressedKeys.Contains(this.actionKeys[Actions.StrafeLeftPrimary]) ||
                        this.currentKeyboardState.PressedKeys.Contains(this.actionKeys[Actions.StrafeLeftAlternate]))
                    {
                        if (!this.movingAlongNegX)
                        {
                            this.movingAlongNegX = true;
                            this.currentVelocity.X = 0.0f;
                        }

                        direction.X -= 1.0f;
                    }
                    else
                    {
                        this.movingAlongNegX = false;
                    }

                    break;

                case Camera.Behavior.Flight:
                    if (this.currentKeyboardState.PressedKeys.Contains(this.actionKeys[Actions.FlightYawLeftPrimary]) ||
                        this.currentKeyboardState.PressedKeys.Contains(this.actionKeys[Actions.FlightYawLeftAlternate]))
                    {
                        if (!this.movingAlongPosX)
                        {
                            this.movingAlongPosX = true;
                            this.currentVelocity.X = 0.0f;
                        }

                        direction.X += 1.0f;
                    }
                    else
                    {
                        this.movingAlongPosX = false;
                    }

                    if (
                        this.currentKeyboardState.PressedKeys.Contains(this.actionKeys[Actions.FlightYawRightPrimary]) ||
                        this.currentKeyboardState.PressedKeys.Contains(this.actionKeys[Actions.FlightYawRightAlternate]))
                    {
                        if (!this.movingAlongNegX)
                        {
                            this.movingAlongNegX = true;
                            this.currentVelocity.X = 0.0f;
                        }

                        direction.X -= 1.0f;
                    }
                    else
                    {
                        this.movingAlongNegX = false;
                    }
                    break;

                case Camera.Behavior.Orbit:
                    if (this.currentKeyboardState.PressedKeys.Contains(this.actionKeys[Actions.OrbitRollLeftPrimary]) ||
                        this.currentKeyboardState.PressedKeys.Contains(this.actionKeys[Actions.OrbitRollLeftAlternate]))
                    {
                        if (!this.movingAlongPosX)
                        {
                            this.movingAlongPosX = true;
                            this.currentVelocity.X = 0.0f;
                        }

                        direction.X += 1.0f;
                    }
                    else
                    {
                        this.movingAlongPosX = false;
                    }

                    if (
                        this.currentKeyboardState.PressedKeys.Contains(this.actionKeys[Actions.OrbitRollRightPrimary]) ||
                        this.currentKeyboardState.PressedKeys.Contains(this.actionKeys[Actions.OrbitRollRightAlternate]))
                    {
                        if (!this.movingAlongNegX)
                        {
                            this.movingAlongNegX = true;
                            this.currentVelocity.X = 0.0f;
                        }

                        direction.X -= 1.0f;
                    }
                    else
                    {
                        this.movingAlongNegX = false;
                    }
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        ///     Determines which way the mouse wheel has been rolled.
        ///     The returned value is in the range [-1,1].
        /// </summary>
        /// <returns>
        ///     A positive value indicates that the mouse wheel has been rolled
        ///     towards the player. A negative value indicates that the mouse
        ///     wheel has been rolled away from the player.
        /// </returns>
        private float GetMouseWheelDirection()
        {
            /*
            int currentWheelValue = mouse.;
            int previousWheelValue = previousMouseState.ScrollWheelValue;

            if (currentWheelValue > previousWheelValue)
                return -1.0f;
            else if (currentWheelValue < previousWheelValue)
                return 1.0f;
            else*/
            return 0.0f;
        }

        /// <summary>
        ///     Filters the mouse movement based on a weighted sum of mouse
        ///     movement from previous frames.
        ///     <para>
        ///         For further details see:
        ///         Nettle, Paul "Smooth Mouse Filtering", flipCode's Ask Midnight column.
        ///         http://www.flipcode.com/cgi-bin/fcarticles.cgi?show=64462
        ///     </para>
        /// </summary>
        /// <param name="x">Horizontal mouse distance from window center.</param>
        /// <param name="y">Vertical mouse distance from window center.</param>
        private void PerformMouseFiltering(float x, float y)
        {
            // Shuffle all the entries in the cache.
            // Newer entries at the front. Older entries towards the back.
            for (int i = this.mouseSmoothingCache.Length - 1; i > 0; --i)
            {
                this.mouseSmoothingCache[i].X = this.mouseSmoothingCache[i - 1].X;
                this.mouseSmoothingCache[i].Y = this.mouseSmoothingCache[i - 1].Y;
            }

            // Store the current mouse movement entry at the front of cache.
            this.mouseSmoothingCache[0].X = x;
            this.mouseSmoothingCache[0].Y = y;

            float averageX = 0.0f;
            float averageY = 0.0f;
            float averageTotal = 0.0f;
            float currentWeight = 1.0f;

            // Filter the mouse movement with the rest of the cache entries.
            // Use a weighted average where newer entries have more effect than
            // older entries (towards the back of the cache).
            for (int i = 0; i < this.mouseSmoothingCache.Length; ++i)
            {
                averageX += this.mouseSmoothingCache[i].X * currentWeight;
                averageY += this.mouseSmoothingCache[i].Y * currentWeight;
                averageTotal += 1.0f * currentWeight;
                currentWeight *= this.mouseSmoothingSensitivity;
            }

            // Calculate the new smoothed mouse movement.
            this.smoothedMouseMovement.X = averageX / averageTotal;
            this.smoothedMouseMovement.Y = averageY / averageTotal;
        }

        /// <summary>
        ///     Averages the mouse movement over a couple of frames to smooth out
        ///     the mouse movement.
        /// </summary>
        /// <param name="x">Horizontal mouse distance from window center.</param>
        /// <param name="y">Vertical mouse distance from window center.</param>
        private void PerformMouseSmoothing(float x, float y)
        {
            this.mouseMovement[this.mouseIndex].X = x;
            this.mouseMovement[this.mouseIndex].Y = y;

            this.smoothedMouseMovement.X = (this.mouseMovement[0].X + this.mouseMovement[1].X) * 0.5f;
            this.smoothedMouseMovement.Y = (this.mouseMovement[0].Y + this.mouseMovement[1].Y) * 0.5f;

            this.mouseIndex ^= 1;
            this.mouseMovement[this.mouseIndex].X = 0.0f;
            this.mouseMovement[this.mouseIndex].Y = 0.0f;
        }

        /// <summary>
        ///     Resets all mouse states. This is called whenever the mouse input
        ///     behavior switches from click-and-drag mode to real-time mode.
        /// </summary>
        private void ResetMouse()
        {
            if (this.mouse.Acquire().IsFailure)
            {
                return;
            }
            if (this.mouse.Poll().IsFailure)
            {
                return;
            }

            this.currentMouseState = this.mouse.GetCurrentState();
            if (Result.Last.IsFailure)
            {
                return;
            }
            this.previousMouseState = this.currentMouseState;

            for (int i = 0; i < this.mouseMovement.Length; ++i)
            {
                this.mouseMovement[i] = Vector2.Zero;
            }

            for (int i = 0; i < this.mouseSmoothingCache.Length; ++i)
            {
                this.mouseSmoothingCache[i] = Vector2.Zero;
            }

            this.savedMousePosX = -1;
            this.savedMousePosY = -1;

            this.smoothedMouseMovement = Vector2.Zero;
            this.mouseIndex = 0;

            Rectangle clientBounds = this.form.ClientRectangle;

            int centerX = clientBounds.Width / 2;
            int centerY = clientBounds.Height / 2;
            int deltaX = centerX - this.currentMouseState.X;
            int deltaY = centerY - this.currentMouseState.Y;

            Cursor.Position = new Point(centerX, centerY);
        }

        /// <summary>
        ///     Dampens the rotation by applying the rotation speed to it.
        /// </summary>
        /// <param name="headingDegrees">Y axis rotation in degrees.</param>
        /// <param name="pitchDegrees">X axis rotation in degrees.</param>
        /// <param name="rollDegrees">Z axis rotation in degrees.</param>
        private void RotateSmoothly(float headingDegrees, float pitchDegrees, float rollDegrees)
        {
            headingDegrees *= this.rotationSpeed;
            pitchDegrees *= this.rotationSpeed;
            rollDegrees *= this.rotationSpeed;

            this.Rotate(headingDegrees, pitchDegrees, rollDegrees);
        }

        /// <summary>
        ///     Gathers and updates input from all supported input devices for use
        ///     by the CameraComponent class.
        /// </summary>
        private void UpdateInput()
        {
            if (this.keyboard.Acquire().IsFailure)
            {
                return;
            }
            if (this.keyboard.Poll().IsFailure)
            {
                return;
            }

            this.currentKeyboardState = this.keyboard.GetCurrentState();
            if (Result.Last.IsFailure)
            {
                return;
            }

            if (this.mouse.Acquire().IsFailure)
            {
                return;
            }
            if (this.mouse.Poll().IsFailure)
            {
                return;
            }

            this.previousMouseState = this.currentMouseState;
            this.currentMouseState = this.mouse.GetCurrentState();
            if (Result.Last.IsFailure)
            {
                return;
            }

            if (this.clickAndDragMouseRotation)
            {
                int deltaX = 0;
                int deltaY = 0;

                if (this.currentMouseState.IsPressed(0))
                {
                    switch (this.CurrentBehavior)
                    {
                        case Camera.Behavior.FirstPerson:
                        case Camera.Behavior.Spectator:
                        case Camera.Behavior.Flight:
                            deltaX = -this.currentMouseState.X;
                            deltaY = -this.currentMouseState.Y;
                            break;

                        case Camera.Behavior.Orbit:
                            deltaX = this.currentMouseState.X;
                            deltaY = this.currentMouseState.Y;
                            break;
                    }

                    this.PerformMouseFiltering(deltaX, deltaY);
                    this.PerformMouseSmoothing(this.smoothedMouseMovement.X, this.smoothedMouseMovement.Y);
                }
            }
            else
            {
                Rectangle clientBounds = this.form.ClientRectangle;

                int centerX = clientBounds.Width / 2;
                int centerY = clientBounds.Height / 2;
                int deltaX = centerX - this.currentMouseState.X;
                int deltaY = centerY - this.currentMouseState.Y;

                Cursor.Position = new Point(centerX, centerY);

                this.PerformMouseFiltering(deltaX, deltaY);
                this.PerformMouseSmoothing(this.smoothedMouseMovement.X, this.smoothedMouseMovement.Y);
            }
        }

        /// <summary>
        ///     Updates the camera's velocity based on the supplied movement
        ///     direction and the elapsed time (since this method was last
        ///     called). The movement direction is the in the range [-1,1].
        /// </summary>
        /// <param name="direction">Direction moved.</param>
        /// <param name="elapsedTimeSec">Elapsed game time.</param>
        private void UpdateVelocity(ref Vector3 direction, float elapsedTimeSec)
        {
            if (direction.X != 0.0f)
            {
                // Camera is moving along the x axis.
                // Linearly accelerate up to the camera's max speed.

                this.currentVelocity.X += direction.X * this.acceleration.X * elapsedTimeSec;

                if (this.currentVelocity.X > this.velocity.X)
                {
                    this.currentVelocity.X = this.velocity.X;
                }
                else if (this.currentVelocity.X < -this.velocity.X)
                {
                    this.currentVelocity.X = -this.velocity.X;
                }
            }
            else
            {
                // Camera is no longer moving along the x axis.
                // Linearly decelerate back to stationary state.

                if (this.currentVelocity.X > 0.0f)
                {
                    if ((this.currentVelocity.X -= this.acceleration.X * elapsedTimeSec) < 0.0f)
                    {
                        this.currentVelocity.X = 0.0f;
                    }
                }
                else
                {
                    if ((this.currentVelocity.X += this.acceleration.X * elapsedTimeSec) > 0.0f)
                    {
                        this.currentVelocity.X = 0.0f;
                    }
                }
            }

            if (direction.Y != 0.0f)
            {
                // Camera is moving along the y axis.
                // Linearly accelerate up to the camera's max speed.

                this.currentVelocity.Y += direction.Y * this.acceleration.Y * elapsedTimeSec;

                if (this.currentVelocity.Y > this.velocity.Y)
                {
                    this.currentVelocity.Y = this.velocity.Y;
                }
                else if (this.currentVelocity.Y < -this.velocity.Y)
                {
                    this.currentVelocity.Y = -this.velocity.Y;
                }
            }
            else
            {
                // Camera is no longer moving along the y axis.
                // Linearly decelerate back to stationary state.

                if (this.currentVelocity.Y > 0.0f)
                {
                    if ((this.currentVelocity.Y -= this.acceleration.Y * elapsedTimeSec) < 0.0f)
                    {
                        this.currentVelocity.Y = 0.0f;
                    }
                }
                else
                {
                    if ((this.currentVelocity.Y += this.acceleration.Y * elapsedTimeSec) > 0.0f)
                    {
                        this.currentVelocity.Y = 0.0f;
                    }
                }
            }

            if (direction.Z != 0.0f)
            {
                // Camera is moving along the z axis.
                // Linearly accelerate up to the camera's max speed.

                this.currentVelocity.Z += direction.Z * this.acceleration.Z * elapsedTimeSec;

                if (this.currentVelocity.Z > this.velocity.Z)
                {
                    this.currentVelocity.Z = this.velocity.Z;
                }
                else if (this.currentVelocity.Z < -this.velocity.Z)
                {
                    this.currentVelocity.Z = -this.velocity.Z;
                }
            }
            else
            {
                // Camera is no longer moving along the z axis.
                // Linearly decelerate back to stationary state.

                if (this.currentVelocity.Z > 0.0f)
                {
                    if ((this.currentVelocity.Z -= this.acceleration.Z * elapsedTimeSec) < 0.0f)
                    {
                        this.currentVelocity.Z = 0.0f;
                    }
                }
                else
                {
                    if ((this.currentVelocity.Z += this.acceleration.Z * elapsedTimeSec) > 0.0f)
                    {
                        this.currentVelocity.Z = 0.0f;
                    }
                }
            }
        }

        /// <summary>
        ///     Moves the camera based on player input.
        /// </summary>
        /// <param name="direction">Direction moved.</param>
        /// <param name="elapsedTimeSec">Elapsed game time.</param>
        private void UpdatePosition(ref Vector3 direction, float elapsedTimeSec)
        {
            if (this.currentVelocity.LengthSquared() != 0.0f)
            {
                // Only move the camera if the velocity vector is not of zero
                // length. Doing this guards against the camera slowly creeping
                // around due to floating point rounding errors.

                Vector3 displacement = (this.currentVelocity * elapsedTimeSec) +
                                       (0.5f * this.acceleration * elapsedTimeSec * elapsedTimeSec);

                // Floating point rounding errors will slowly accumulate and
                // cause the camera to move along each axis. To prevent any
                // unintended movement the displacement vector is clamped to
                // zero for each direction that the camera isn't moving in.
                // Note that the UpdateVelocity() method will slowly decelerate
                // the camera's velocity back to a stationary state when the
                // camera is no longer moving along that direction. To account
                // for this the camera's current velocity is also checked.

                if (direction.X == 0.0f && Math.Abs(this.currentVelocity.X) < 1e-6f)
                {
                    displacement.X = 0.0f;
                }

                if (direction.Y == 0.0f && Math.Abs(this.currentVelocity.Y) < 1e-6f)
                {
                    displacement.Y = 0.0f;
                }

                if (direction.Z == 0.0f && Math.Abs(this.currentVelocity.Z) < 1e-6f)
                {
                    displacement.Z = 0.0f;
                }

                this.Move(displacement.X, displacement.Y, displacement.Z);
            }

            // Continuously update the camera's velocity vector even if the
            // camera hasn't moved during this call. When the camera is no
            // longer being moved the camera is decelerating back to its
            // stationary state.

            this.UpdateVelocity(ref direction, elapsedTimeSec);
        }

        /// <summary>
        ///     Updates the state of the camera based on player input.
        /// </summary>
        /// <param name="gameTime">Elapsed game time.</param>
        private void UpdateCamera(float elapsedTimeSec)
        {
            Vector3 direction = new Vector3();

            this.GetMovementDirection(out direction);

            float dx = 0.0f;
            float dy = 0.0f;
            float dz = 0.0f;

            switch (this.camera.CurrentBehavior)
            {
                case Camera.Behavior.FirstPerson:
                case Camera.Behavior.Spectator:
                    dx = this.smoothedMouseMovement.X;
                    dy = this.smoothedMouseMovement.Y;

                    this.RotateSmoothly(dx, dy, 0.0f);
                    this.UpdatePosition(ref direction, elapsedTimeSec);
                    break;

                case Camera.Behavior.Flight:
                    dy = -this.smoothedMouseMovement.Y;
                    dz = this.smoothedMouseMovement.X;

                    this.RotateSmoothly(0.0f, dy, dz);

                    if ((dx = direction.X * this.flightYawSpeed * elapsedTimeSec) != 0.0f)
                    {
                        this.camera.Rotate(dx, 0.0f, 0.0f);
                    }

                    direction.X = 0.0f; // ignore yaw motion when updating camera's velocity
                    this.UpdatePosition(ref direction, elapsedTimeSec);
                    break;

                case Camera.Behavior.Orbit:
                    dx = -this.smoothedMouseMovement.X;
                    dy = -this.smoothedMouseMovement.Y;

                    this.RotateSmoothly(dx, dy, 0.0f);

                    if (!this.camera.PreferTargetYAxisOrbiting)
                    {
                        if ((dz = direction.X * this.orbitRollSpeed * elapsedTimeSec) != 0.0f)
                        {
                            this.camera.Rotate(0.0f, 0.0f, dz);
                        }
                    }

                    if ((dz = this.GetMouseWheelDirection() * this.mouseWheelSpeed) != 0.0f)
                    {
                        this.camera.Zoom(dz, this.camera.OrbitMinZoom, this.camera.OrbitMaxZoom);
                    }

                    break;

                default:
                    break;
            }
        }

        /// <summary>
        ///     Property to get and set the camera's acceleration.
        /// </summary>
        public Vector3 Acceleration { get { return this.acceleration; } set { this.acceleration = value; } }

        /// <summary>
        ///     Property to get and set the mouse rotation behavior.
        ///     The default is false which will immediately rotate the camera
        ///     as soon as the mouse is moved. If this property is set to true
        ///     camera rotations only occur when the mouse button is held down and
        ///     the mouse dragged (i.e., clicking-and-dragging the mouse).
        /// </summary>
        public bool ClickAndDragMouseRotation
        {
            get { return this.clickAndDragMouseRotation; }

            set
            {
                this.clickAndDragMouseRotation = value;

                if (value == false)
                {
                    this.ResetMouse();
                }
            }
        }

        /// <summary>
        ///     Property to get and set the camera's behavior.
        /// </summary>
        public Camera.Behavior CurrentBehavior
        {
            get { return this.camera.CurrentBehavior; }
            set { this.camera.CurrentBehavior = value; }
        }

        /// <summary>
        ///     Property to get the camera's current velocity.
        /// </summary>
        public Vector3 CurrentVelocity { get { return this.currentVelocity; } }

        /// <summary>
        ///     Property to get and set the flight behavior's yaw speed.
        /// </summary>
        public float FlightYawSpeed { get { return this.flightYawSpeed; } set { this.flightYawSpeed = value; } }

        /// <summary>
        ///     Property to get and set the sensitivity value used to smooth
        ///     mouse movement.
        /// </summary>
        public float MouseSmoothingSensitivity
        {
            get { return this.mouseSmoothingSensitivity; }
            set { this.mouseSmoothingSensitivity = value; }
        }

        /// <summary>
        ///     Property to get and set the speed of the mouse wheel.
        ///     This is used to zoom in and out when the camera is orbiting.
        /// </summary>
        public float MouseWheelSpeed { get { return this.mouseWheelSpeed; } set { this.mouseWheelSpeed = value; } }

        /// <summary>
        ///     Property to get and set the max orbit zoom distance.
        /// </summary>
        public float OrbitMaxZoom { get { return this.camera.OrbitMaxZoom; } set { this.camera.OrbitMaxZoom = value; } }

        /// <summary>
        ///     Property to get and set the min orbit zoom distance.
        /// </summary>
        public float OrbitMinZoom { get { return this.camera.OrbitMinZoom; } set { this.camera.OrbitMinZoom = value; } }

        /// <summary>
        ///     Property to get and set the distance from the target when orbiting.
        /// </summary>
        public float OrbitOffsetDistance
        {
            get { return this.camera.OrbitOffsetDistance; }
            set { this.camera.OrbitOffsetDistance = value; }
        }

        /// <summary>
        ///     Property to get and set the orbit behavior's rolling speed.
        ///     This only applies when PreferTargetYAxisOrbiting is set to false.
        ///     Orbiting with PreferTargetYAxisOrbiting set to true will ignore
        ///     any camera rolling.
        /// </summary>
        public float OrbitRollSpeed { get { return this.orbitRollSpeed; } set { this.orbitRollSpeed = value; } }

        /// <summary>
        ///     Property to get and set the camera orbit target position.
        /// </summary>
        public Vector3 OrbitTarget { get { return this.camera.OrbitTarget; } set { this.camera.OrbitTarget = value; } }

        /// <summary>
        ///     Property to get and set the flag to force the camera
        ///     to orbit around the orbit target's Y axis rather than the camera's
        ///     local Y axis.
        /// </summary>
        public bool PreferTargetYAxisOrbiting
        {
            get { return this.camera.PreferTargetYAxisOrbiting; }
            set { this.camera.PreferTargetYAxisOrbiting = value; }
        }

        /// <summary>
        ///     Property to get and set the mouse rotation speed.
        /// </summary>
        public float RotationSpeed { get { return this.rotationSpeed; } set { this.rotationSpeed = value; } }

        /// <summary>
        ///     Property to get and set the camera's velocity.
        /// </summary>
        public Vector3 Velocity { get { return this.velocity; } set { this.velocity = value; } }

        /// <summary>
        ///     Property to get and set the camera orientation.
        /// </summary>
        public Quaternion Orientation
        {
            get { return this.camera.Orientation; }
            set { this.camera.Orientation = value; }
        }

        /// <summary>
        ///     Property to get and set the camera position.
        /// </summary>
        public Vector3 Position { get { return this.camera.Position; } set { this.camera.Position = value; } }

        /// <summary>
        ///     Property to get the perspective projection matrix.
        /// </summary>
        public Matrix ProjectionMatrix { get { return this.camera.ProjectionMatrix; } }

        /// <summary>
        ///     Property to get the viewing direction vector.
        /// </summary>
        public Vector3 ViewDirection { get { return this.camera.ViewDirection; } }

        /// <summary>
        ///     Property to get the view matrix.
        /// </summary>
        public Matrix ViewMatrix { get { return this.camera.ViewMatrix; } }

        /// <summary>
        ///     Property to get the concatenated view-projection matrix.
        /// </summary>
        public Matrix ViewProjectionMatrix { get { return this.camera.ViewProjectionMatrix; } }

        /// <summary>
        ///     Property to get the camera's local X axis.
        /// </summary>
        public Vector3 XAxis { get { return this.camera.XAxis; } }

        /// <summary>
        ///     Property to get the camera's local Y axis.
        /// </summary>
        public Vector3 YAxis { get { return this.camera.YAxis; } }

        /// <summary>
        ///     Property to get the camera's local Z axis.
        /// </summary>
        public Vector3 ZAxis { get { return this.camera.ZAxis; } }
    }
}
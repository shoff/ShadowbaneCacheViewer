using System;
using CacheViewer.Domain.Extensions;
using SlimDX;

namespace CacheViewer.Controls.Viewer
{
    /// <summary>
    ///     A general purpose 6DoF (six degrees of freedom) quaternion based
    ///     camera. This camera class supports 4 different behaviors: first
    ///     person mode, spectator mode, flight mode, and orbit mode. First
    ///     person mode only allows 5DOF (x axis movement, y axis movement,
    ///     z axis movement, yaw, and pitch) and movement is always parallel
    ///     to the world x-z (ground) plane. Spectator mode is similar to first
    ///     person mode only movement is along the direction the camera is
    ///     pointing. Flight mode supports 6DoF. This is the camera class'
    ///     default behavior. Orbit mode rotates the camera around a target
    ///     position. This mode can be used to simulate a third person camera.
    /// </summary>
    public class Camera : ICamera
    {
        public enum Behavior
        {
            FirstPerson,
            Spectator,
            Flight,
            Orbit
        };

        public const float DefaultFovx = 90.0f;
        public const float DefaultZnear = 0.1f;
        public const float DefaultZfar = 1000.0f;
        public const float DefaultOrbitMinZoom = DefaultZnear + 1.0f;
        public const float DefaultOrbitMaxZoom = DefaultZfar*0.5f;

        public const float DefaultOrbitOffsetLength =
            DefaultOrbitMinZoom + (DefaultOrbitMaxZoom - DefaultOrbitMinZoom)*0.25f;

        private static readonly Vector3 worldXAxis = new Vector3(1.0f, 0.0f, 0.0f);
        private static readonly Vector3 worldYAxis = new Vector3(0.0f, 1.0f, 0.0f);
        private static Vector3 worldZAxis = new Vector3(0.0f, 0.0f, 1.0f);
        private readonly float zfar;
        private readonly float znear;
        private float accumPitchDegrees;
        private float aspectRatio;
        private Behavior behavior;

        private Vector3 eye;
        private float firstPersonYOffset;
        private float fovx;
        private float orbitOffsetLength;
        private Quaternion orientation;
        private bool preferTargetYAxisOrbiting;
        private Matrix projMatrix;
        private float savedAccumPitchDegrees;
        private Vector3 savedEye;
        private Quaternion savedOrientation;
        private Vector3 target;
        private Vector3 targetYAxis;
        private Vector3 viewDir;
        private Matrix viewMatrix;
        private Vector3 xAxis;
        private Vector3 yAxis;
        private Vector3 zAxis;

        /// <summary>
        ///     Constructs a new instance of the camera class. The camera will
        ///     have a flight behavior, and will be initially positioned at the
        ///     world origin looking down the world negative z axis.
        /// </summary>
        public Camera()
        {
            this.behavior = Behavior.Flight;
            this.preferTargetYAxisOrbiting = true;

            this.fovx = DefaultFovx;
            this.znear = DefaultZnear;
            this.zfar = DefaultZfar;

            this.accumPitchDegrees = 0.0f;
            this.OrbitMinZoom = DefaultOrbitMinZoom;
            this.OrbitMaxZoom = DefaultOrbitMaxZoom;
            this.orbitOffsetLength = DefaultOrbitOffsetLength;
            this.firstPersonYOffset = 0.0f;

            this.eye = Vector3.Zero;
            this.target = Vector3.Zero;
            this.targetYAxis = Vector3.UnitY;
            this.xAxis = Vector3.UnitX;
            this.yAxis = Vector3.UnitY;
            this.zAxis = Vector3.UnitZ;

            this.orientation = Quaternion.Identity;
            this.viewMatrix = Matrix.Identity;

            this.savedEye = this.eye;
            this.savedOrientation = this.orientation;
            this.savedAccumPitchDegrees = 0.0f;
        }

        /// <summary>
        ///     Gets or sets the current behavior.
        /// </summary>
        /// <value>
        ///     The current behavior.
        /// </value>
        public Behavior CurrentBehavior
        {
            get { return this.behavior; }
            set { ChangeBehavior(value); }
        }

        /// <summary>
        ///     Gets or sets the orbit maximum zoom.
        /// </summary>
        /// <value>
        ///     The orbit maximum zoom.
        /// </value>
        public float OrbitMaxZoom { get; set; }

        /// <summary>
        ///     Gets or sets the orbit minimum zoom.
        /// </summary>
        /// <value>
        ///     The orbit minimum zoom.
        /// </value>
        public float OrbitMinZoom { get; set; }

        /// <summary>
        ///     Gets or sets the orbit offset distance.
        /// </summary>
        /// <value>
        ///     The orbit offset distance.
        /// </value>
        public float OrbitOffsetDistance
        {
            get { return this.orbitOffsetLength; }
            set { this.orbitOffsetLength = value; }
        }

        /// <summary>
        ///     Gets or sets the orbit target.
        /// </summary>
        /// <value>
        ///     The orbit target.
        /// </value>
        public Vector3 OrbitTarget
        {
            get { return this.target; }
            set { this.target = value; }
        }

        /// <summary>
        ///     Property to get and set the flag to force the camera
        ///     to orbit around the orbit target's Y axis rather than the camera's
        ///     local Y axis.
        /// </summary>
        public bool PreferTargetYAxisOrbiting
        {
            get { return this.preferTargetYAxisOrbiting; }

            set
            {
                this.preferTargetYAxisOrbiting = value;

                if (this.preferTargetYAxisOrbiting)
                {
                    this.UndoRoll();
                }
            }
        }

        /// <summary>
        ///     Builds a look at style viewing matrix.
        /// </summary>
        /// <param name="target">The target position to look at.</param>
        public void LookAt(Vector3 target)
        {
            this.LookAt(this.eye, target, this.yAxis);
        }

        /// <summary>
        ///     Builds a look at style viewing matrix.
        /// </summary>
        /// <param name="eye">The camera position.</param>
        /// <param name="target">The target position to look at.</param>
        /// <param name="up">The up direction.</param>
        public void LookAt(Vector3 eye, Vector3 target, Vector3 up)
        {
            this.eye = eye;
            this.target = target;

            this.zAxis = eye - target;
            this.zAxis.Normalize();

            this.viewDir.X = -this.zAxis.X;
            this.viewDir.Y = -this.zAxis.Y;
            this.viewDir.Z = -this.zAxis.Z;

            Vector3.Cross(ref up, ref this.zAxis, out this.xAxis);
            this.xAxis.Normalize();

            Vector3.Cross(ref this.zAxis, ref this.xAxis, out this.yAxis);
            this.yAxis.Normalize();
            this.xAxis.Normalize();

            this.viewMatrix.M11 = this.xAxis.X;
            this.viewMatrix.M21 = this.xAxis.Y;
            this.viewMatrix.M31 = this.xAxis.Z;
            this.viewMatrix.M41 = -Vector3.Dot(this.xAxis, eye);

            this.viewMatrix.M12 = this.yAxis.X;
            this.viewMatrix.M22 = this.yAxis.Y;
            this.viewMatrix.M32 = this.yAxis.Z;
            this.viewMatrix.M42 = -Vector3.Dot(this.yAxis, eye);

            this.viewMatrix.M13 = this.zAxis.X;
            this.viewMatrix.M23 = this.zAxis.Y;
            this.viewMatrix.M33 = this.zAxis.Z;
            this.viewMatrix.M43 = -Vector3.Dot(this.zAxis, eye);

            this.viewMatrix.M14 = 0.0f;
            this.viewMatrix.M24 = 0.0f;
            this.viewMatrix.M34 = 0.0f;
            this.viewMatrix.M44 = 1.0f;

            this.accumPitchDegrees = ((float) Math.Asin(this.viewMatrix.M23)).ToDegrees();
            this.orientation = Quaternion.RotationMatrix(this.viewMatrix);
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
            if (this.behavior == Behavior.Orbit)
            {
                // Orbiting camera is always positioned relative to the target
                // position. See UpdateViewMatrix().
                return;
            }

            Vector3 forwards;

            if (this.behavior == Behavior.FirstPerson)
            {
                // Calculate the forwards direction. Can't just use the
                // camera's view direction as doing so will cause the camera to
                // move more slowly as the camera's view approaches 90 degrees
                // straight up and down.

                forwards = Vector3.Normalize(Vector3.Cross(worldYAxis, this.xAxis));
            }
            else
            {
                forwards = this.viewDir;
            }

            this.eye += this.xAxis*dx;
            this.eye += worldYAxis*dy;
            this.eye += forwards*dz;

            this.Position = this.eye;
        }

        /// <summary>
        ///     Moves the camera the specified distance in the specified direction.
        /// </summary>
        /// <param name="direction">Direction to move.</param>
        /// <param name="distance">How far to move.</param>
        public void Move(Vector3 direction, Vector3 distance)
        {
            if (this.behavior == Behavior.Orbit)
            {
                // Orbiting camera is always positioned relative to the target
                // position. See UpdateViewMatrix().
                return;
            }

            this.eye.X += direction.X*distance.X;
            this.eye.Y += direction.Y*distance.Y;
            this.eye.Z += direction.Z*distance.Z;

            this.UpdateViewMatrix();
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
            /*
            this.fovx = fovx;
            this.aspectRatio = aspect;
            this.znear = znear;
            this.zfar = zfar;

            float aspectInv = 1.0f / aspect;
            float e = 1.0f / (float)Math.Tan(MathHelper.ToRadians(fovx) / 2.0f);
            float fovy = 2.0f * (float)Math.Atan(aspectInv / e);
            float xScale = 1.0f / (float)Math.Tan(0.5f * fovy);
            float yScale = xScale / aspectInv;

            projMatrix.M11 = xScale;
            projMatrix.M12 = 0.0f;
            projMatrix.M13 = 0.0f;
            projMatrix.M14 = 0.0f;

            projMatrix.M21 = 0.0f;
            projMatrix.M22 = yScale;
            projMatrix.M23 = 0.0f;
            projMatrix.M24 = 0.0f;

            projMatrix.M31 = 0.0f;
            projMatrix.M32 = 0.0f;
            projMatrix.M33 = (zfar + znear) / (znear - zfar);
            projMatrix.M34 = -1.0f;

            projMatrix.M41 = 0.0f;
            projMatrix.M42 = 0.0f;
            projMatrix.M43 = (2.0f * zfar * znear) / (znear - zfar);
            projMatrix.M44 = 0.0f;
             
            */
            this.projMatrix = Matrix.PerspectiveFovRH((float) Math.PI/4, 16.0f/9.0f, 1, 1000);
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
            headingDegrees = -headingDegrees;
            pitchDegrees = -pitchDegrees;
            rollDegrees = -rollDegrees;

            switch (this.behavior)
            {
                case Behavior.FirstPerson:

                case Behavior.Spectator:
                    this.RotateFirstPerson(headingDegrees, pitchDegrees);
                    break;

                case Behavior.Flight:
                    this.RotateFlight(headingDegrees, pitchDegrees, rollDegrees);
                    break;

                case Behavior.Orbit:
                    this.RotateOrbit(headingDegrees, pitchDegrees, rollDegrees);
                    break;
            }

            this.UpdateViewMatrix();
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
            if (this.behavior == Behavior.Orbit)
            {
                Vector3 offset = this.eye - this.target;

                this.orbitOffsetLength = offset.Length();
                offset.Normalize();
                this.orbitOffsetLength += zoom;
                this.orbitOffsetLength = Math.Min(Math.Max(this.orbitOffsetLength, minZoom), maxZoom);
                offset *= this.orbitOffsetLength;
                this.eye = offset + this.target;
                this.UpdateViewMatrix();
            }
            else
            {
                zoom = Math.Min(Math.Max(zoom, minZoom), maxZoom);
                this.Perspective(zoom, this.aspectRatio, this.znear, this.zfar);
            }
        }

        /// <summary>
        ///     Property to get and set the camera orientation.
        /// </summary>
        public Quaternion Orientation
        {
            get { return this.orientation; }
            set { this.ChangeOrientation(value); }
        }

        /// <summary>
        ///     Property to get and set the camera position.
        /// </summary>
        public Vector3 Position
        {
            get { return this.eye; }

            set
            {
                this.eye = value;
                this.UpdateViewMatrix();
            }
        }

        /// <summary>
        ///     Property to get the perspective projection matrix.
        /// </summary>
        public Matrix ProjectionMatrix
        {
            get { return this.projMatrix; }
        }

        /// <summary>
        ///     Property to get the viewing direction vector.
        /// </summary>
        public Vector3 ViewDirection
        {
            get { return this.viewDir; }
        }

        /// <summary>
        ///     Property to get the view matrix.
        /// </summary>
        public Matrix ViewMatrix
        {
            get { return this.viewMatrix; }
        }

        /// <summary>
        ///     Property to get the concatenated view-projection matrix.
        /// </summary>
        public Matrix ViewProjectionMatrix
        {
            get { return this.viewMatrix*this.projMatrix; }
        }

        /// <summary>
        ///     Property to get the camera's local X axis.
        /// </summary>
        public Vector3 XAxis
        {
            get { return this.xAxis; }
        }

        /// <summary>
        ///     Property to get the camera's local Y axis.
        /// </summary>
        public Vector3 YAxis
        {
            get { return this.yAxis; }
        }

        /// <summary>
        ///     Property to get the camera's local Z axis.
        /// </summary>
        public Vector3 ZAxis
        {
            get { return this.zAxis; }
        }

        /// <summary>
        ///     Undo any camera rolling by leveling the camera. When the camera is
        ///     orbiting this method will cause the camera to become level with the
        ///     orbit target.
        /// </summary>
        public void UndoRoll()
        {
            if (this.behavior == Behavior.Orbit)
            {
                this.LookAt(this.eye, this.target, this.targetYAxis);
            }
            else
            {
                this.LookAt(this.eye, this.eye + this.ViewDirection, worldYAxis);
            }
        }

        /// <summary>
        ///     Change to a new camera behavior.
        /// </summary>
        /// <param name="newBehavior">The new camera behavior.</param>
        private void ChangeBehavior(Behavior newBehavior)
        {
            Behavior prevBehavior = this.behavior;

            if (prevBehavior == newBehavior)
            {
                return;
            }

            this.behavior = newBehavior;

            switch (newBehavior)
            {
                case Behavior.FirstPerson:
                    switch (prevBehavior)
                    {
                        case Behavior.Flight:
                        case Behavior.Spectator:
                            this.eye.Y = this.firstPersonYOffset;
                            this.UpdateViewMatrix();
                            break;

                        case Behavior.Orbit:
                            this.eye.X = this.savedEye.X;
                            this.eye.Z = this.savedEye.Z;
                            this.eye.Y = this.firstPersonYOffset;
                            this.orientation = this.savedOrientation;
                            this.accumPitchDegrees = this.savedAccumPitchDegrees;
                            this.UpdateViewMatrix();
                            break;
                    }

                    this.UndoRoll();
                    break;

                case Behavior.Spectator:
                    switch (prevBehavior)
                    {
                        case Behavior.Flight:
                            this.UpdateViewMatrix();
                            break;

                        case Behavior.Orbit:
                            this.eye = this.savedEye;
                            this.orientation = this.savedOrientation;
                            this.accumPitchDegrees = this.savedAccumPitchDegrees;
                            this.UpdateViewMatrix();
                            break;
                    }

                    this.UndoRoll();
                    break;

                case Behavior.Flight:
                    if (prevBehavior == Behavior.Orbit)
                    {
                        this.eye = this.savedEye;
                        this.orientation = this.savedOrientation;
                        this.accumPitchDegrees = this.savedAccumPitchDegrees;
                        this.UpdateViewMatrix();
                    }
                    else
                    {
                        this.savedEye = this.eye;
                        this.UpdateViewMatrix();
                    }
                    break;

                case Behavior.Orbit:
                    if (prevBehavior == Behavior.FirstPerson)
                    {
                        this.firstPersonYOffset = this.eye.Y;
                    }

                    this.savedEye = this.eye;
                    this.savedOrientation = this.orientation;
                    this.savedAccumPitchDegrees = this.accumPitchDegrees;

                    this.targetYAxis = this.yAxis;

                    Vector3 newEye = this.eye + this.zAxis*this.orbitOffsetLength;

                    this.LookAt(newEye, this.eye, this.targetYAxis);
                    break;
            }
        }

        /// <summary>
        ///     Sets a new camera orientation.
        /// </summary>
        /// <param name="newOrientation">The new orientation.</param>
        private void ChangeOrientation(Quaternion newOrientation)
        {
            //Matrix m = Matrix.CreateFromQuaternion(newOrientation);
            Matrix m = Matrix.RotationQuaternion(newOrientation);

            // Store the pitch for this new orientation.
            // First person and spectator behaviors limit pitching to
            // 90 degrees straight up and down.

            float pitch = (float) Math.Asin(m.M23);

            this.accumPitchDegrees = pitch.ToDegrees();

            // First person and spectator behaviors don't allow rolling.
            // Negate any rolling that might be encoded in the new orientation.

            this.orientation = newOrientation;

            if (this.behavior == Behavior.FirstPerson || this.behavior == Behavior.Spectator)
            {
                this.LookAt(this.eye, this.eye + Vector3.Negate(this.zAxis), worldYAxis);
            }

            this.UpdateViewMatrix();
        }

        /// <summary>
        ///     Rotates the camera for first person and spectator behaviors.
        ///     Pitching is limited to 90 degrees straight up and down.
        /// </summary>
        /// <param name="headingDegrees">Y axis rotation angle.</param>
        /// <param name="pitchDegrees">X axis rotation angle.</param>
        private void RotateFirstPerson(float headingDegrees, float pitchDegrees)
        {
            this.accumPitchDegrees += pitchDegrees;

            if (this.accumPitchDegrees > 90.0f)
            {
                pitchDegrees = 90.0f - (this.accumPitchDegrees - pitchDegrees);
                this.accumPitchDegrees = 90.0f;
            }

            if (this.accumPitchDegrees < -90.0f)
            {
                pitchDegrees = -90.0f - (this.accumPitchDegrees - pitchDegrees);
                this.accumPitchDegrees = -90.0f;
            }

            float heading = headingDegrees.ToRadians();
            float pitch = pitchDegrees.ToRadians();
            Quaternion rotation = Quaternion.Identity;

            // Rotate the camera about the world Y axis.
            if (heading != 0.0f)
            {
                rotation = Quaternion.RotationAxis(worldYAxis, heading);
                this.orientation = Quaternion.Multiply(rotation, this.orientation);
            }

            // Rotate the camera about its local X axis.
            if (pitch != 0.0f)
            {
                rotation = Quaternion.RotationAxis(worldXAxis, pitch);
                this.orientation = Quaternion.Multiply(this.orientation, rotation);
            }
        }

        /// <summary>
        ///     Rotates the camera for flight behavior.
        /// </summary>
        /// <param name="headingDegrees">Y axis rotation angle.</param>
        /// <param name="pitchDegrees">X axis rotation angle.</param>
        /// <param name="rollDegrees">Z axis rotation angle.</param>
        private void RotateFlight(float headingDegrees, float pitchDegrees, float rollDegrees)
        {
            this.accumPitchDegrees += pitchDegrees;

            if (this.accumPitchDegrees > 360.0f)
            {
                this.accumPitchDegrees -= 360.0f;
            }

            if (this.accumPitchDegrees < -360.0f)
            {
                this.accumPitchDegrees += 360.0f;
            }

            float heading = headingDegrees.ToRadians();
            float pitch = pitchDegrees.ToRadians();
            float roll = rollDegrees.ToRadians();

            Quaternion rotation = Quaternion.RotationYawPitchRoll(heading, pitch, roll);
            this.orientation = Quaternion.Multiply(this.orientation, rotation);
        }

        /// <summary>
        ///     Rotates the camera for orbit behavior. Rotations are either about
        ///     the camera's local y axis or the orbit target's y axis. The property
        ///     PreferTargetYAxisOrbiting controls which rotation method to use.
        /// </summary>
        /// <param name="headingDegrees">Y axis rotation angle.</param>
        /// <param name="pitchDegrees">X axis rotation angle.</param>
        /// <param name="rollDegrees">Z axis rotation angle.</param>
        private void RotateOrbit(float headingDegrees, float pitchDegrees, float rollDegrees)
        {
            float heading = headingDegrees.ToRadians();
            float pitch = pitchDegrees.ToRadians();

            if (this.preferTargetYAxisOrbiting)
            {
                Quaternion rotation = Quaternion.Identity;

                if (Math.Abs(heading) > 0.001f)
                {
                    rotation = Quaternion.RotationAxis(this.targetYAxis, heading);
                    this.orientation = Quaternion.Multiply(rotation, this.orientation);
                }

                if (Math.Abs(pitch) > 0.001f)
                {
                    rotation = Quaternion.RotationAxis(worldXAxis, pitch);
                    this.orientation = Quaternion.Multiply(this.orientation, rotation);
                }
            }
            else
            {
                float roll = rollDegrees.ToRadians();
                Quaternion rotation = Quaternion.RotationYawPitchRoll(heading, pitch, roll);
                this.orientation = Quaternion.Multiply(this.orientation, rotation);
            }
        }

        /// <summary>
        ///     Rebuild the view matrix.
        /// </summary>
        private void UpdateViewMatrix()
        {
            this.viewMatrix = Matrix.RotationQuaternion(this.orientation);

            this.xAxis.X = this.viewMatrix.M11;
            this.xAxis.Y = this.viewMatrix.M21;
            this.xAxis.Z = this.viewMatrix.M31;

            this.yAxis.X = this.viewMatrix.M12;
            this.yAxis.Y = this.viewMatrix.M22;
            this.yAxis.Z = this.viewMatrix.M32;

            this.zAxis.X = this.viewMatrix.M13;
            this.zAxis.Y = this.viewMatrix.M23;
            this.zAxis.Z = this.viewMatrix.M33;

            if (this.behavior == Behavior.Orbit)
            {
                // Calculate the new camera position based on the current
                // orientation. The camera must always maintain the same
                // distance from the target. Use the current offset vector
                // to determine the correct distance from the target.

                this.eye = this.target + this.zAxis*this.orbitOffsetLength;
            }

            this.viewMatrix.M41 = -Vector3.Dot(this.xAxis, this.eye);
            this.viewMatrix.M42 = -Vector3.Dot(this.yAxis, this.eye);
            this.viewMatrix.M43 = -Vector3.Dot(this.zAxis, this.eye);

            this.viewDir.X = -this.zAxis.X;
            this.viewDir.Y = -this.zAxis.Y;
            this.viewDir.Z = -this.zAxis.Z;
        }
    }
}
using SlimDX;

namespace CacheViewer.Controls.Viewer
{
    public interface ICamera
    {
        /// <summary>
        ///     Property to get and set the camera's orientation.
        /// </summary>
        Quaternion Orientation { get; set; }

        /// <summary>
        ///     Property to get and set the camera's position.
        /// </summary>
        Vector3 Position { get; set; }

        /// <summary>
        ///     Property to get the camera's perspective projection matrix.
        /// </summary>
        Matrix ProjectionMatrix { get; }

        /// <summary>
        ///     Property to get the camera's viewing direction.
        /// </summary>
        Vector3 ViewDirection { get; }

        /// <summary>
        ///     Property to get the camera's view matrix.
        /// </summary>
        Matrix ViewMatrix { get; }

        /// <summary>
        ///     Property to get the camera's concatenated view-projection matrix.
        /// </summary>
        Matrix ViewProjectionMatrix { get; }

        /// <summary>
        ///     Property to get the camera's local x axis.
        /// </summary>
        Vector3 XAxis { get; }

        /// <summary>
        ///     Property to get the camera's local y axis.
        /// </summary>
        Vector3 YAxis { get; }

        /// <summary>
        ///     Property to get the camera's local z axis.
        /// </summary>
        Vector3 ZAxis { get; }

        /// <summary>
        ///     Builds a look at style viewing matrix using the camera's current
        ///     world position, and its current local y axis.
        /// </summary>
        /// <param name="target">The target position to look at.</param>
        void LookAt(Vector3 target);

        /// <summary>
        ///     Builds a look at style viewing matrix.
        /// </summary>
        /// <param name="eye">The camera position.</param>
        /// <param name="target">The target position to look at.</param>
        /// <param name="up">The up direction.</param>
        void LookAt(Vector3 eye, Vector3 target, Vector3 up);

        /// <summary>
        ///     Moves the camera by dx world units to the left or right; dy
        ///     world units upwards or downwards; and dz world units forwards
        ///     or backwards.
        /// </summary>
        /// <param name="dx">Distance to move left or right.</param>
        /// <param name="dy">Distance to move up or down.</param>
        /// <param name="dz">Distance to move forwards or backwards.</param>
        void Move(float dx, float dy, float dz);

        /// <summary>
        ///     Moves the camera the specified distance in the specified direction.
        /// </summary>
        /// <param name="direction">Direction to move.</param>
        /// <param name="distance">How far to move.</param>
        void Move(Vector3 direction, Vector3 distance);

        /// <summary>
        ///     Builds a perspective projection matrix based on a horizontal field
        ///     of view.
        /// </summary>
        /// <param name="fovx">Horizontal field of view in degrees.</param>
        /// <param name="aspect">The viewport's aspect ratio.</param>
        /// <param name="znear">The distance to the near clip plane.</param>
        /// <param name="zfar">The distance to the far clip plane.</param>
        void Perspective(float fovx, float aspect, float znear, float zfar);

        /// <summary>
        ///     Rotates the camera. Positive angles specify counter clockwise
        ///     rotations when looking down the axis of rotation towards the
        ///     origin.
        /// </summary>
        /// <param name="headingDegrees">Y axis rotation in degrees.</param>
        /// <param name="pitchDegrees">X axis rotation in degrees.</param>
        /// <param name="rollDegrees">Z axis rotation in degrees.</param>
        void Rotate(float headingDegrees, float pitchDegrees, float rollDegrees);

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
        void Zoom(float zoom, float minZoom, float maxZoom);
    }
}
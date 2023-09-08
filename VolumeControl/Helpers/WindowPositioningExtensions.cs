﻿using System.ComponentModel;
using System.Windows;
using VolumeControl.TypeExtensions;

namespace VolumeControl.Helpers
{
    /// <summary>
    /// Extension methods for the <see cref="Window"/> class and other helper functions for manipulating screen space coordinates and positioning windows.
    /// </summary>
    public static class WindowPositioningExtensions
    {
        #region Get/Set Pos
        /// <summary>
        /// Gets the position of the window.
        /// </summary>
        /// <param name="wnd">(Implicit) A <see cref="Window"/> instance.</param>
        /// <returns>A <see cref="Point"/> containing the x/y coordinates of the window's origin point (top-left corner).</returns>
        public static Point GetPos(this Window wnd)
            => new(wnd.Left, wnd.Top);
        /// <summary>
        /// Sets the position of the window to a given point.
        /// </summary>
        /// <param name="wnd">(Implicit) A <see cref="Window"/> instance.</param>
        /// <param name="point">A <see cref="Point"/> specifying the x/y coordinates to position the window's origin point (top-left corner) at.</param>
        public static void SetPos(this Window wnd, Point point)
        {
            wnd.Left = point.X;
            wnd.Top = point.Y;
        }
        #endregion Get/Set Pos

        #region Get/Set PosAtCenterPoint
        /// <summary>
        /// Gets the centerpoint of the window.
        /// </summary>
        /// <param name="wnd">(Implicit) A <see cref="Window"/> instance.</param>
        /// <returns>A <see cref="Point"/> containing the x/y coordinates of the window's centerpoint.</returns>
        public static Point GetPosAtCenterPoint(this Window wnd)
            => new(wnd.Left + (wnd.Width / 2), wnd.Top + (wnd.Height / 2));
        /// <summary>
        /// Sets the position of the window, using the window's center as an origin point.
        /// </summary>
        /// <param name="wnd">(Implicit) A <see cref="Window"/> instance.</param>
        /// <param name="point">The absolute x/y coordinates of the target position.</param>
        public static void SetPosAtCenterPoint(this Window wnd, Point point)
        {
            wnd.Left = point.X - wnd.Width / 2;
            wnd.Top = point.Y - wnd.Height / 2;
        }
        #endregion Get/Set CenterPoint

        #region Get/Set PosAtCorner
        /// <summary>
        /// Gets the position of the window at the specified <paramref name="corner"/>.
        /// </summary>
        /// <param name="wnd">(Implicit) A <see cref="Window"/> instance.</param>
        /// <param name="corner">The corner to check</param>
        /// <returns>The position of the window at the specified <paramref name="corner"/>.</returns>
        /// <exception cref="InvalidEnumArgumentException">Invalid <see cref="Core.Helpers.ScreenCorner"/> enumeration.</exception>
        public static Point GetPosAtCorner(this Window wnd, Core.Helpers.ScreenCorner corner)
        {
            var compositionTarget = PresentationSource.FromVisual(wnd)?.CompositionTarget;
            return corner switch
            {
                Core.Helpers.ScreenCorner.TopLeft => compositionTarget?.TransformToDevice.Transform(new Point(wnd.Left, wnd.Top)) ?? new Point(wnd.Left, wnd.Top),
                Core.Helpers.ScreenCorner.TopRight => compositionTarget?.TransformToDevice.Transform(new Point(wnd.Left + wnd.ActualWidth, wnd.Top)) ?? new Point(wnd.Left + wnd.ActualWidth, wnd.Top),
                Core.Helpers.ScreenCorner.BottomLeft => compositionTarget?.TransformToDevice.Transform(new Point(wnd.Left, wnd.Top + wnd.ActualHeight)) ?? new Point(wnd.Left, wnd.Top + wnd.ActualHeight),
                Core.Helpers.ScreenCorner.BottomRight => compositionTarget?.TransformToDevice.Transform(new Point(wnd.Left + wnd.ActualWidth, wnd.Top + wnd.ActualHeight)) ?? new Point(wnd.Left + wnd.ActualWidth, wnd.Top + wnd.ActualHeight),
                _ => throw new InvalidEnumArgumentException(nameof(corner), (int)corner, typeof(Core.Helpers.ScreenCorner)),
            };
        }
        /// <summary>
        /// Sets the position of the window, using the specified <paramref name="corner"/> as an origin point.
        /// </summary>
        /// <param name="wnd">(Implicit) A <see cref="Window"/> instance.</param>
        /// <param name="corner">The corner of the window to use as an origin point when setting the window's position.</param>
        /// <param name="point">The absolute x/y coordinates of the target position.</param>
        public static void SetPosAtCorner(this Window wnd, Core.Helpers.ScreenCorner corner, Point point)
        {
            switch (corner)
            {
            case Core.Helpers.ScreenCorner.TopLeft:
                wnd.Left = point.X;
                wnd.Top = point.Y;
                break;
            case Core.Helpers.ScreenCorner.TopRight:
                wnd.Left = point.X + wnd.Width;
                wnd.Top = point.Y;
                break;
            case Core.Helpers.ScreenCorner.BottomLeft:
                wnd.Left = point.X;
                wnd.Top = point.Y - wnd.Height;
                break;
            case Core.Helpers.ScreenCorner.BottomRight:
                wnd.Left = point.X + wnd.Width;
                wnd.Top = point.Y - wnd.Height;
                break;
            }
        }
        #endregion Get/Set AtCorner

        #region GetPosAtCurrentCorner
        /// <summary>
        /// Gets the position of the corner of the window that is closest to the screen edge.
        /// </summary>
        /// <param name="wnd">(Implicit) A <see cref="Window"/> instance.</param>
        /// <returns>A <see cref="Point"/> containing the x/y coordinates of the corner of the window that is closest to the screen edge.</returns>
        public static Point GetPosAtCurrentCorner(this Window wnd)
            => wnd.GetPosAtCorner(wnd.GetCurrentScreenCorner());
        #endregion GetPosAtCurrentCorner

        #region GetCurrentScreen
        /// <summary>
        /// Gets the screen that contains the centerpoint of the window.
        /// </summary>
        /// <param name="wnd">(Implicit) A <see cref="Window"/> instance.</param>
        /// <returns>The <see cref="System.Windows.Forms.Screen"/> instance that the window's centerpoint is found on.</returns>
        public static System.Windows.Forms.Screen GetCurrentScreen(this Window wnd)
            => GetScreenFromPoint(wnd.GetPosAtCenterPoint());
        #endregion GetCurrentScreen

        #region GetCurrentScreenCenterPoint
        /// <summary>
        /// Gets the centerpoint of the screen where the window is currently located.
        /// </summary>
        /// <param name="wnd">(Implicit) A <see cref="Window"/> instance.</param>
        /// <returns>A <see cref="Point"/> containing the x/y coordinates of the current screen's centerpoint.</returns>
        public static Point GetCurrentScreenCenterPoint(this Window wnd)
            => GetScreenCenterPoint(wnd.GetCurrentScreen());
        #endregion GetCurrentScreenCenterPoint

        #region GetCurrentScreenCorner
        /// <summary>
        /// Gets the closest <see cref="Core.Helpers.ScreenCorner"/> to the window's centerpoint.
        /// </summary>
        /// <param name="wnd">(Implicit) A <see cref="Window"/> instance.</param>
        /// <returns>The closest <see cref="Core.Helpers.ScreenCorner"/> to the window's centerpoint.</returns>
        public static Core.Helpers.ScreenCorner GetCurrentScreenCorner(this Window wnd)
            => GetClosestScreenCornerFromPoint(wnd.GetPosAtCenterPoint());
        #endregion GetCurrentScreenCorner

        #region GetScreenFromPoint
        /// <summary>
        /// Gets the screen that contains the specified point.
        /// </summary>
        /// <param name="point">A <see cref="Point"/> specifying the x/y coordinate of a point on the desktop.</param>
        /// <returns>The <see cref="System.Windows.Forms.Screen"/> instance that contains the specified <paramref name="point"/>.</returns>
        public static System.Windows.Forms.Screen GetScreenFromPoint(Point point)
            => System.Windows.Forms.Screen.FromPoint(new((int)point.X, (int)point.Y));
        #endregion GetScreenFromPoint

        #region GetScreenCenterPoint
        /// <summary>
        /// Gets the centerpoint of the specified <paramref name="screen"/>.
        /// </summary>
        /// <param name="screen">A <see cref="System.Windows.Forms.Screen"/> instance to get the centerpoint of.</param>
        /// <returns>A <see cref="Point"/> containing the absolute x/y coordinates of the specified <paramref name="screen"/>'s centerpoint.</returns>
        public static Point GetScreenCenterPoint(System.Windows.Forms.Screen screen)
            => new(screen.WorkingArea.Left + (screen.WorkingArea.Width / 2), screen.WorkingArea.Top + (screen.WorkingArea.Height / 2));
        #endregion GetScreenCenterPoint

        #region GetClosestScreenCornerFromPoint
        /// <summary>
        /// Gets the corner of the given <paramref name="screen"/> that is closest to the specified <paramref name="point"/>.
        /// </summary>
        /// <param name="screen">The <see cref="System.Windows.Forms.Screen"/> instance to use.</param>
        /// <param name="point">An x/y coordinate specifying a point on the <paramref name="screen"/>.</param>
        /// <returns>The <see cref="Core.Helpers.ScreenCorner"/> representing the corner of the <paramref name="screen"/> that is closest to the given <paramref name="point"/>.</returns>
        public static Core.Helpers.ScreenCorner GetClosestScreenCornerFromPoint(System.Windows.Forms.Screen screen, Point point)
        {
            // automatic corner selection is enabled:
            // get the centerpoint of this window
            (double cx, double cy) = GetScreenCenterPoint(screen);

            // figure out which corner is the closest & use that
            bool left = point.X < cx;
            bool top = point.Y < cy;

            if (left && top)
                return Core.Helpers.ScreenCorner.TopLeft;
            else if (!left && top)
                return Core.Helpers.ScreenCorner.TopRight;
            else if (left && !top)
                return Core.Helpers.ScreenCorner.BottomLeft;
            else if (!left && !top)
                return Core.Helpers.ScreenCorner.BottomRight;

            return Core.Helpers.ScreenCorner.TopLeft;
        }
        /// <inheritdoc cref="GetClosestScreenCornerFromPoint(System.Windows.Forms.Screen, Point)"/>
        /// <remarks>
        /// This calls <see cref="GetClosestScreenCornerFromPoint(System.Windows.Forms.Screen, Point)"/> internally by automatically determining the screen to use based on the given <paramref name="pos"/>.
        /// </remarks>
        public static Core.Helpers.ScreenCorner GetClosestScreenCornerFromPoint(Point pos)
            => GetClosestScreenCornerFromPoint(GetScreenFromPoint(pos), pos);
        #endregion GetClosestScreenCornerFromPoint
    }
}
﻿using HotkeyLib;
using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using VolumeControl.Core;
using VolumeControl.Core.HotkeyActions;
using VolumeControl.WPF;

namespace VolumeControl.Helpers
{
    public class VolumeControlSettings : INotifyPropertyChanged, INotifyCollectionChanged, IDisposable
    {
        public VolumeControlSettings()
        {
            _audioAPI = new();
            _hWndMixer = WindowHandleGetter.GetWindowHandle();
            _hotkeyManager = new(new HotkeyActionManager(new AudioAPIActions(_audioAPI), new WindowsAPIActions(_hWndMixer)));

            ShowIcons = Settings.ShowIcons;

            Log.Debug($"{nameof(VolumeControlSettings)} finished initializing settings from all assemblies.");


            _audioAPI.PropertyChanged += (s, e) => OnPropertyChanged($"AudioAPI.{e.PropertyName}");
        }

        public event NotifyCollectionChangedEventHandler? CollectionChanged
        {
            add => ((INotifyCollectionChanged)_hotkeyManager).CollectionChanged += value;
            remove => ((INotifyCollectionChanged)_hotkeyManager).CollectionChanged -= value;
        }

        ~VolumeControlSettings()
        {
            Dispose(disposing: true);
        }

        #region Events
        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "") => PropertyChanged?.Invoke(this, new(propertyName));
        #endregion Events

        #region Fields
        private bool _showIcons;
        private bool disposedValue;
        private readonly AudioAPI _audioAPI;
        private readonly HotkeyManager _hotkeyManager;
        private readonly IntPtr _hWndMixer;
        #endregion Fields

        #region Properties
        #region Statics
        /// <summary>Static accessor for <see cref="Properties.Settings.Default"/>.</summary>
        private static Properties.Settings Settings => Properties.Settings.Default;
        /// <summary>Static accessor for <see cref="Core.CoreSettings.Default"/>.</summary>
        private static Core.CoreSettings CoreSettings => CoreSettings.Default;
        /// <summary>Static accessor for <see cref="Log.Properties.Settings.Default"/>.</summary>
        private static Log.Properties.Settings LogSettings => global::VolumeControl.Log.Properties.Settings.Default;
        private static Log.LogWriter Log => global::VolumeControl.Log.FLog.Log;
        #endregion Statics

        #region ParentObjects
        public AudioAPI AudioAPI => _audioAPI;
        public HotkeyManager HotkeyAPI => _hotkeyManager;
        #endregion ParentObjects

        /// <summary>
        /// Gets or sets a boolean that determines whether or not device/session icons are shown in the UI.
        /// </summary>
        public bool ShowIcons
        {
            get => _showIcons;
            set
            {
                _showIcons = value;
                OnPropertyChanged();
            }
        }
        // TODO: Advanced Hotkeys
        // TODO: Run At Startup
        // TODO: Start Minimized
        // TODO: Enable Notifications
        // TODO: Notification Timeout
        #endregion Properties

        #region Methods
        #region SettingsManipulation
        public void ResetHotkeySettings()
        {
            if (MessageBox.Show("Are you sure you want to reset your hotkeys to their default values?\n\nThis cannot be undone!", "Reset Hotkeys?", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.Yes)
            {
                HotkeyAPI.ResetHotkeys();

                Log.Info("Hotkey definitions were reset to default.");
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // VolumeControl
                    Settings.ShowIcons = ShowIcons;
                    // save settings
                    Settings.Save();
                    Settings.Reload();

                    // VolumeControl.Core
                    // save coresettings
                    CoreSettings.Save();
                    CoreSettings.Reload();

                    // VolumeControl.Log
                    // save logsettings
                    LogSettings.Save();
                    LogSettings.Reload();

                    Log.Debug($"{nameof(VolumeControlSettings)} finished saving settings from all assemblies.");

                    // Dispose of objects
                    AudioAPI.Dispose();
                    HotkeyAPI.Dispose();
                }

                disposedValue = true;
            }
        }
        /// <inheritdoc/>
        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        #endregion SettingsManipulation
        #endregion Methods
    }
}
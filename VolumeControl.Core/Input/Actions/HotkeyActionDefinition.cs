﻿using System.Reflection;
using System.Windows.Media;
using VolumeControl.Core.Input.Actions.Settings;
using VolumeControl.Log;

namespace VolumeControl.Core.Input.Actions
{
    /// <summary>
    /// Represents the definition of a hotkey action, including metadata and the reflection classes for targeting the method.
    /// </summary>
    public class HotkeyActionDefinition
    {
        #region Constructors
        /// <summary>
        /// Creates a new <see cref="HotkeyActionDefinition"/> instance for a non-static action method.
        /// </summary>
        /// <param name="objectInstance">An instance of the action group class.</param>
        /// <param name="methodInfo">The <see cref="MethodInfo"/> of the action method.</param>
        /// <param name="name">The name of the action.</param>
        /// <param name="description">The description of the action.</param>
        /// <param name="groupName">The group name of the action.</param>
        /// <param name="groupBrush">The group brush of the action.</param>
        /// <param name="actionSettingDefinitions">The action setting definitions for the action.</param>
        public HotkeyActionDefinition(object objectInstance, MethodInfo methodInfo, string name, string? description, string? groupName, Brush? groupBrush, ActionSettingDefinition[] actionSettingDefinitions)
        {
            // reflection
            ActionGroupType = objectInstance.GetType();
            ActionGroupInstance = objectInstance;
            ActionMethodInfo = methodInfo;
            // metadata
            Name = name;
            Description = description;
            GroupName = groupName;
            GroupBrush = groupBrush;
            ActionSettingDefinitions = actionSettingDefinitions;
            Identifier = $"{(GroupName != null ? GroupName + ':' : "")}{Name}";
        }
        /// <summary>
        /// Creates a new <see cref="HotkeyActionDefinition"/> instance for a static action method.
        /// </summary>
        /// <param name="objectType">The <see cref="Type"/> of the action group class.</param>
        /// <param name="methodInfo">The <see cref="MethodInfo"/> of the action method.</param>
        /// <param name="name">The name of the action.</param>
        /// <param name="description">The description of the action.</param>
        /// <param name="groupName">The group name of the action.</param>
        /// <param name="groupBrush">The group brush of the action.</param>
        /// <param name="actionSettingDefinitions">The action setting definitions for the action.</param>
        public HotkeyActionDefinition(Type objectType, MethodInfo methodInfo, string name, string? description, string? groupName, Brush? groupBrush, ActionSettingDefinition[] actionSettingDefinitions)
        {
            // reflection
            ActionGroupType = objectType;
            ActionGroupInstance = null;
            ActionMethodInfo = methodInfo;
            // metadata
            Name = name;
            Description = description;
            GroupName = groupName;
            GroupBrush = groupBrush;
            ActionSettingDefinitions = actionSettingDefinitions;
            Identifier = $"{(GroupName != null ? GroupName + ':' : "")}{Name}";
        }
        #endregion Constructors

        #region Properties

        #region Reflection
        /// <summary>
        /// Gets the class type of the hotkey action group that this action belongs to.
        /// </summary>
        public Type ActionGroupType { get; }
        /// <summary>
        /// Gets the instance of the hotkey action group that this action belongs to.
        /// </summary>
        public object? ActionGroupInstance { get; }
        /// <summary>
        /// Gets the method info for the action method that this action represents.
        /// </summary>
        public MethodInfo ActionMethodInfo { get; }
        #endregion Reflection

        #region Metadata
        /// <summary>
        /// Gets the name of this action.
        /// </summary>
        public string Name { get; }
        /// <summary>
        /// Gets the description of this action.
        /// </summary>
        public string? Description { get; }
        /// <summary>
        /// Gets the group name of this action.
        /// </summary>
        public string? GroupName { get; }
        /// <summary>
        /// Gets the brush for the group name of this action.
        /// </summary>
        public Brush? GroupBrush { get; }
        /// <summary>
        /// Gets the action setting definitions of this action.
        /// </summary>
        public ActionSettingDefinition[] ActionSettingDefinitions { get; }
        /// <summary>
        /// Gets the identifier string of this action.
        /// </summary>
        public string Identifier { get; }
        #endregion Metadata

        #endregion Properties

        #region Methods

        #region CreateInstance
        private IActionSettingInstance[] CreateDefaultActionSettings()
            => ActionSettingDefinitions.Select(def => def.CreateInstance()).ToArray();
        private IActionSettingInstance[] MergeDefaultActionSettings(IActionSettingInstance[] actionSettings)
        {
            List<IActionSettingInstance> l = new();

            foreach (var definition in ActionSettingDefinitions)
            {
                try
                {
                    l.Add(definition.CreateInstance(actionSettings.FirstOrDefault(settingInst => settingInst.Name.Equals(definition.Name, StringComparison.Ordinal))?.Value));
                }
                catch (Exception ex)
                {
                    if (FLog.Log.FilterEventType(Log.Enum.EventType.ERROR))
                        FLog.Log.Error($"Failed to instantiate action setting \"{Name}\" due to an exception:", ex);
                }
            }

            return l.ToArray();
        }
        /// <summary>
        /// Creates a new <see cref="HotkeyActionInstance"/> instance from this definition.
        /// </summary>
        /// <returns>A new <see cref="HotkeyActionInstance"/> object.</returns>
        public HotkeyActionInstance CreateInstance()
            => new(this, CreateDefaultActionSettings());
        /// <summary>
        /// Creates a new <see cref="HotkeyActionInstance"/> instance from this definition and the specified <paramref name="actionSettings"/>.
        /// </summary>
        /// <param name="actionSettings">The action settings to use for the action instance.</param>
        /// <returns>A new <see cref="HotkeyActionInstance"/> object with the specified <paramref name="actionSettings"/>.</returns>
        public HotkeyActionInstance CreateInstance(IActionSettingInstance[] actionSettings)
            => new(this, MergeDefaultActionSettings(actionSettings));
        #endregion CreateInstance

        #region Invoke_Unsafe
        /// <summary>
        /// Directly invokes the method specified by <see cref="ActionMethodInfo"/> with the specified <paramref name="parameters"/>.
        /// </summary>
        /// <remarks>
        /// <b>Does not catch exceptions!</b>
        /// </remarks>
        /// <param name="parameters">Parameters to invoke the method with. These must match the parameters accepted by the actual method.</param>
        /// <returns>The return value from <see cref="ActionMethodInfo"/>.</returns>
        public object? Invoke_Unsafe(params object?[] parameters)
            => ActionMethodInfo.Invoke(ActionGroupInstance, parameters);
        /// <summary>
        /// Directly invokes the method specified by <see cref="ActionMethodInfo"/> with the parameters expected by <see cref="HotkeyActionPressedEventHandler"/>.
        /// </summary>
        /// <inheritdoc cref="Invoke_Unsafe(object?[])"/>
        public object? Invoke_Unsafe(object? sender, IActionSettingInstance[] actionSettings)
            => Invoke_Unsafe(sender, new HotkeyActionPressedEventArgs(actionSettings));
        #endregion Invoke_Unsafe

        #region GetActionSettingDefinition
        /// <summary>
        /// Gets the <see cref="ActionSettingDefinition"/> object with the specified <paramref name="name"/>.
        /// </summary>
        /// <param name="name">The name of the action setting definition to get.</param>
        /// <param name="stringComparison">The comparison type to use for string comparisons.</param>
        /// <returns>The <see cref="ActionSettingDefinition"/> with the specified <paramref name="name"/> if found; otherwise <see langword="null"/>.</returns>
        public ActionSettingDefinition? GetActionSettingDefinition(string name, StringComparison stringComparison = StringComparison.Ordinal)
        {
            for (int i = 0, max = ActionSettingDefinitions.Length; i < max; ++i)
            {
                var def = ActionSettingDefinitions[i];
                if (def.Name.Equals(name, stringComparison))
                    return def;
            }
            return null;
        }
        #endregion GetActionSettingDefinition

        #endregion Methods
    }
}

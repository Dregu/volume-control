﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using VolumeControl.Log.Enum;
using VolumeControl.TypeExtensions;

namespace VolumeControl.WPF.Collections
{
    /// <summary>
    /// Provides the source for a WPF list component's ItemsSource property to expose a readable and writable interface for enum types with <see cref="FlagsAttribute"/>.
    /// </summary>
    public class BindableEventType : ICollection<BindableEventType.BindableEventTypeFlag>, IEnumerable<BindableEventType.BindableEventTypeFlag>, IEnumerable, IList<BindableEventType.BindableEventTypeFlag>, IReadOnlyCollection<BindableEventType.BindableEventTypeFlag>, IReadOnlyList<BindableEventType.BindableEventTypeFlag>, ICollection, IList, INotifyPropertyChanged, INotifyCollectionChanged
    {
        #region SubObject
        /// <summary>
        /// This represents a single flag value of the <see cref="EventType"/> enum.
        /// </summary>
        /// <remarks>This is used by <see cref="BindableEventType"/> to provide a bindable <b>ItemsSource</b> list.</remarks>
        public class BindableEventTypeFlag : INotifyPropertyChanged
        {
            #region Constructor
            /// <inheritdoc cref="BindableEventTypeFlag"/>
            /// <param name="parent">The parent <see cref="BindableEventType"/> container.</param>
            /// <param name="ev">The event type.</param>
            public BindableEventTypeFlag(BindableEventType parent, EventType ev)
            {
                _parent = parent;
                _ev = ev;
                Name = Enum.GetName(typeof(EventType), _ev) ?? string.Empty;
            }
            #endregion Constructor

            #region Fields
            private readonly BindableEventType _parent;
            private readonly EventType _ev;
            #endregion Fields

            #region Events
            /// <summary>Triggered when a property's setting is called, after the value has changed.</summary>
            public event PropertyChangedEventHandler? PropertyChanged;
            /// <summary>Triggers the <see cref="PropertyChanged"/> event.</summary>
            protected virtual void NotifyPropertyChanged([CallerMemberName] string propertyName = "") => PropertyChanged?.Invoke(this, new(propertyName));
            #endregion Events

            #region Properties
            /// <summary>The plaintext name of this enumeration.</summary>
            public string Name { get; }
            /// <summary>Gets or sets whether this flag is set in the underlying bitfield.</summary>
            public bool IsSet
            {
                get => (_parent.Value & _ev) != 0;
                set
                {
                    if (value)
                        _parent.Value |= _ev;
                    else
                        _parent.Value &= ~_ev;
                    NotifyPropertyChanged();
                }
            }
            #endregion Properties
        }
        #endregion SubObject

        #region Constructors
        /// <inheritdoc cref="BindableEventType"/>
        public BindableEventType()
        {
            _value = EventType.NONE;
            InitializeOptions();
            Options.CollectionChanged += ForwardCollectionChanged;
        }
        /// <inheritdoc cref="BindableEventType"/>
        public BindableEventType(EventType ev)
        {
            _value = ev;
            InitializeOptions();
            Options.CollectionChanged += ForwardCollectionChanged;
        }
        #endregion Constructors

        #region Events
        /// <summary>Triggered when a property's setting is called, after the value has changed.</summary>
        public event PropertyChangedEventHandler? PropertyChanged;
        /// <summary>Triggered when the options list is changed.</summary>
        public event NotifyCollectionChangedEventHandler? CollectionChanged;
        /// <summary>Triggers the <see cref="PropertyChanged"/> event.</summary>
        protected virtual void NotifyPropertyChanged([CallerMemberName] string propertyName = "") => PropertyChanged?.Invoke(this, new(propertyName));
        /// <summary>Triggers the <see cref="CollectionChanged"/> event.</summary>
        protected virtual void NotifyCollectionChanged(NotifyCollectionChangedEventArgs e) => CollectionChanged?.Invoke(this, e);
        private void ForwardCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e) => CollectionChanged?.Invoke(sender, e);
        #endregion Events

        #region Indexers
        /// <inheritdoc/>
        public BindableEventTypeFlag this[int index] { get => ((IList<BindableEventTypeFlag>)Options)[index]; set => ((IList<BindableEventTypeFlag>)Options)[index] = value; }
        /// <inheritdoc/>
        object? IList.this[int index] { get => ((IList)Options)[index]; set => ((IList)Options)[index] = value; }
        #endregion Indexers

        #region Fields
        private EventType _value;
        #endregion Fields

        #region Properties
        /// <summary>Gets the formatted name of the current bitfield combination.</summary>
        /// <remarks>This may include <see cref="EventType.NONE"/>, <see cref="EventType.ALL_EXCEPT_DEBUG"/>, <see cref="EventType.ALL"/>, or a combination of other values.</remarks>
        public string Name => $"{Value:G}";
        /// <summary>Gets or sets the bitfield flag value.</summary>
        public EventType Value
        {
            get => _value;
            set
            {
                _value = value;
                NotifyPropertyChanged();
            }
        }
        /// <summary>
        /// Gets the list of bindable enum types.
        /// </summary>
        public ObservableList<BindableEventTypeFlag> Options { get; } = new();

        /// <inheritdoc/>
        public int Count => ((ICollection<BindableEventTypeFlag>)Options).Count;

        /// <inheritdoc/>
        public bool IsReadOnly => ((ICollection<BindableEventTypeFlag>)Options).IsReadOnly;

        /// <inheritdoc/>
        public bool IsSynchronized => ((ICollection)Options).IsSynchronized;

        /// <inheritdoc/>
        public object SyncRoot => ((ICollection)Options).SyncRoot;

        /// <inheritdoc/>
        public bool IsFixedSize => ((IList)Options).IsFixedSize;
        #endregion Properties

        #region Methods
        /// <summary>
        /// This method is responsible for populating the <see cref="Options"/> list, as well as filtering out unwanted event types like <see cref="EventType.NONE"/>, <see cref="EventType.ALL_EXCEPT_DEBUG"/>, and <see cref="EventType.ALL"/>.
        /// </summary>
        private void InitializeOptions()
        {
            foreach (EventType e in (EventType[])Enum.GetValues(typeof(EventType)))
            {
                int eval = (int)e;
                if (eval == 1 || eval != 0 && eval % 2 == 0)
                {
                    double exponent = Math.Log2(eval);
                    if (exponent.EqualsWithin((int)exponent))
                        Options.Add(new(this, e));
                }
            }
        }
        /// <inheritdoc/>
        public void Add(BindableEventTypeFlag item) => ((ICollection<BindableEventTypeFlag>)Options).Add(item);
        /// <inheritdoc/>
        public int Add(object? value) => ((IList)Options).Add(value);
        /// <inheritdoc/>
        public void Clear() => ((ICollection<BindableEventTypeFlag>)Options).Clear();
        /// <inheritdoc/>
        public bool Contains(BindableEventTypeFlag item) => ((ICollection<BindableEventTypeFlag>)Options).Contains(item);
        /// <inheritdoc/>
        public bool Contains(object? value) => ((IList)Options).Contains(value);
        /// <inheritdoc/>
        public void CopyTo(BindableEventTypeFlag[] array, int arrayIndex) => ((ICollection<BindableEventTypeFlag>)Options).CopyTo(array, arrayIndex);
        /// <inheritdoc/>
        public void CopyTo(Array array, int index) => ((ICollection)Options).CopyTo(array, index);
        /// <inheritdoc/>
        public IEnumerator<BindableEventTypeFlag> GetEnumerator() => ((IEnumerable<BindableEventTypeFlag>)Options).GetEnumerator();
        /// <inheritdoc/>
        public int IndexOf(BindableEventTypeFlag item) => ((IList<BindableEventTypeFlag>)Options).IndexOf(item);
        /// <inheritdoc/>
        public int IndexOf(object? value) => ((IList)Options).IndexOf(value);
        /// <inheritdoc/>
        public void Insert(int index, BindableEventTypeFlag item) => ((IList<BindableEventTypeFlag>)Options).Insert(index, item);
        /// <inheritdoc/>
        public void Insert(int index, object? value) => ((IList)Options).Insert(index, value);
        /// <inheritdoc/>
        public bool Remove(BindableEventTypeFlag item) => ((ICollection<BindableEventTypeFlag>)Options).Remove(item);
        /// <inheritdoc/>
        public void Remove(object? value) => ((IList)Options).Remove(value);
        /// <inheritdoc/>
        public void RemoveAt(int index) => ((IList<BindableEventTypeFlag>)Options).RemoveAt(index);
        /// <inheritdoc/>
        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)Options).GetEnumerator();
        #endregion Methods
    }
}

﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace VolumeControl.UnitTests
{
    /// <summary>
    /// Custom &amp; improved assertion methods for unit testing using the <see cref="Assert"/> class.
    /// </summary>
    public static class Assertx
    {
        #region Type
        /// <summary>Asserts that <paramref name="obj"/> is the same type as <typeparamref name="T"/>.</summary>
        public static void Is<T>(object obj, string? msg = null, [CallerLineNumber] int ln = 0, [CallerFilePath] string path = "")
        {
            var type = typeof(T);
            if (!obj.GetType().Equals(type)) Assert.Fail($"{msg}{(msg == null ? "" : "\n")}'{obj.GetType().FullName}' is not the same type as '{type.FullName}'!\n[{path}:{ln}]");
        }
        /// <summary>Asserts that <paramref name="obj"/> is not the same type as <typeparamref name="T"/>.</summary>
        public static void IsNot<T>(object obj, string? msg = null, [CallerLineNumber] int ln = 0, [CallerFilePath] string path = "")
        {
            var type = typeof(T);
            if (obj.GetType().Equals(type)) Assert.Fail($"{msg}{(msg == null ? "" : "\n")}'{obj.GetType().FullName}' is the same type as '{type.FullName}'!\n[{path}:{ln}]");
        }
        /// <summary>Asserts that <paramref name="obj"/> is the same type as <typeparamref name="T"/>.</summary>
        public static void Is<T>(Type type, string? msg = null, [CallerLineNumber] int ln = 0, [CallerFilePath] string path = "")
        {
            if (!typeof(T).GetType().Equals(type)) Assert.Fail($"{msg}{(msg == null ? "" : "\n")}'{typeof(T).FullName}' is not the same type as '{type.FullName}'!\n[{path}:{ln}]");
        }
        /// <summary>Asserts that <paramref name="obj"/> is not the same type as <typeparamref name="T"/>.</summary>
        public static void IsNot<T>(Type type, string? msg = null, [CallerLineNumber] int ln = 0, [CallerFilePath] string path = "")
        {
            if (typeof(T).Equals(type)) Assert.Fail($"{msg}{(msg == null ? "" : "\n")}'{typeof(T).FullName}' is the same type as '{type.FullName}'!\n[{path}:{ln}]");
        }
        #endregion Type

        #region Equate
        /// <summary>Intended to be used with <b>non-numeric</b> types.<br/>Asserts that <paramref name="left"/> is equal to <paramref name="right"/>.</summary>
        public static void Same<T>(T left, T right, string? msg = null, [CallerLineNumber] int ln = 0, [CallerFilePath] string path = "") where T : IEquatable<T>
        {
            if (!left.Equals(right)) Assert.Fail($"{msg}{(msg == null ? "" : "\n")}'{left}' is not the same as '{right}'!\n[{path}:{ln}]");
        }
        public static void Same(bool left, bool right, string? msg = null, [CallerLineNumber] int ln = 0, [CallerFilePath] string path = "")
        {
            if (!(left == right)) Assert.Fail($"{msg}{(msg == null ? "" : "\n")}'{left}' is not the same as '{right}'\n[{path}:{ln}]");
        }
        /// <summary>Intended to be used with <b>non-numeric</b> types.<br/>Asserts that <paramref name="left"/> is equal to <paramref name="right"/>.</summary>
        public static void NotSame<T>(T left, T right, string? msg = null, [CallerLineNumber] int ln = 0, [CallerFilePath] string path = "") where T : IEquatable<T>
        {
            if (!!left.Equals(right)) Assert.Fail($"{msg}{(msg == null ? "" : "\n")}'{left}' is not the same as '{right}'!\n[{path}:{ln}]");
        }
        public static void NotSame(bool left, bool right, string? msg = null, [CallerLineNumber] int ln = 0, [CallerFilePath] string path = "")
        {
            if (!(left != right)) Assert.Fail($"{msg}{(msg == null ? "" : "\n")}'{left}' is not the same as '{right}'\n[{path}:{ln}]");
        }
        #endregion Equate

        #region NumericComparison
        /// <summary>Intended to be used with numeric types.</summary>
        public static void Equal(dynamic left, dynamic right, string? msg = null, [CallerLineNumber] int ln = 0, [CallerFilePath] string path = "")
        {
            if (!(left == right)) Assert.Fail($"{msg}{(msg == null ? "" : "\n")}'{left}' is not equal to '{right}'\n[{path}:{ln}]");
        }
        /// <summary>Intended to be used with numeric types.</summary>
        public static void Less(dynamic number, dynamic threshold, string? msg = null, [CallerLineNumber] int ln = 0, [CallerFilePath] string path = "")
        {
            if (!(number < threshold)) Assert.Fail($"{msg}{(msg == null ? "" : "\n")}'{number}' is not less than '{threshold}'!\n[{path}:{ln}]");
        }
        /// <summary>Intended to be used with numeric types.</summary>
        public static void LessOrEqual(dynamic number, dynamic threshold, string? msg = null, [CallerLineNumber] int ln = 0, [CallerFilePath] string path = "")
        {
            if (!(number <= threshold)) Assert.Fail($"{msg}{(msg == null ? "" : "\n")}'{number}' is not less than or equal to '{threshold}'!\n[{path}:{ln}]");
        }
        /// <summary>Intended to be used with numeric types.</summary>
        public static void Greater(dynamic number, dynamic threshold, string? msg = null, [CallerLineNumber] int ln = 0, [CallerFilePath] string path = "")
        {
            if (!(number > threshold)) Assert.Fail($"{msg}{(msg == null ? "" : "\n")}'{number}' is not greater than '{threshold}'!\n[{path}:{ln}]");
        }
        /// <summary>Intended to be used with numeric types.</summary>
        public static void GreaterOrEqual(dynamic number, dynamic threshold, string? msg = null, [CallerLineNumber] int ln = 0, [CallerFilePath] string path = "")
        {
            if (!(number >= threshold)) Assert.Fail($"{msg}{(msg == null ? "" : "\n")}'{number}' is not greater or equal to '{threshold}'!\n[{path}:{ln}]");
        }
        #endregion NumericComparison

        #region Nullable
        /// <summary>Asserts that the given object is null.</summary>
        public static void Null(object? obj, string? msg = null, [CallerLineNumber] int ln = 0, [CallerFilePath] string path = "")
        {
            if (!(obj == null))
                Assert.Fail($"{msg}{(msg == null ? "" : "\n")}{nameof(obj)} is not null!\n[{path}:{ln}]");
        }
        /// <summary>Asserts that the given object isn't null.</summary>
        public static void NotNull(object? obj, string? msg = null, [CallerLineNumber] int ln = 0, [CallerFilePath] string path = "")
        {
            if (!(obj != null))
                Assert.Fail($"{msg}{(msg == null ? "" : "\n")}{nameof(obj)} is null!\n[{path}:{ln}]");
        }
        #endregion Nullable

        #region Exception
        /// <summary>Asserts that the given action throws any exception.</summary>
        public static void Throws(Action action, string? msg = null, [CallerLineNumber] int ln = 0, [CallerFilePath] string path = "")
        {
            try
            {
                action();
                Assert.Fail($"{msg}{(msg == null ? "" : "\n")}{nameof(action)} didn't throw an exception!\n[{path}:{ln}]");
            }
            catch (AssertFailedException) { throw; } //< rethrow assertion failures instead of throwing a new exception
            catch (Exception) { }
        }
        public static void Throws<T>(Action action, string? msg = null, [CallerLineNumber] int ln = 0, [CallerFilePath] string path = "") where T : Exception
        {
            try
            {
                action();
                Assert.Fail($"{msg}{(msg == null ? "" : "\n")}{nameof(action)} didn't throw an exception!\n[{path}:{ln}]");
            }
            catch (T) { }
            catch (AssertFailedException) { throw; } //< rethrow assertion failures instead of throwing a new exception
            catch (Exception ex)
            {
                Assert.Fail($"{msg}{(msg == null ? "" : "\n")}{nameof(action)} threw an exception of type {ex.GetType().FullName}; expected type {typeof(T).FullName}!\n[{path}:{ln}]");
            }
        }
        /// <summary>Asserts that the given action doesn't throw any exception.</summary>
        public static void NoThrows(Action action, string? msg = null, [CallerLineNumber] int ln = 0, [CallerFilePath] string path = "")
        {
            try
            {
                action();
            }
            catch (AssertFailedException) { throw; } //< rethrow assertion failures instead of throwing a new exception
            catch (Exception ex)
            {
                Assert.Fail($"{msg}{(msg == null ? "" : "\n")}{nameof(action)} threw an exception: '{ex.Message}'\n{ex.StackTrace}\n[{path}:{ln}]");
            }
        }
        /// <summary>Asserts that the given action doesn't throw any exception.</summary>
        public static void NoThrows<T>(Action action, string? msg = null, [CallerLineNumber] int ln = 0, [CallerFilePath] string path = "") where T : Exception
        {
            try
            {
                action();
            }
            catch (T ex)
            {
                Assert.Fail($"{msg}{(msg == null ? "" : "\n")}{nameof(action)} threw an exception: '{ex.Message}'\n{ex.StackTrace}\n[{path}:{ln}]");
            }
            catch (AssertFailedException) { throw; } //< rethrow assertion failures instead of throwing a new exception
            catch (Exception) { }
        }
        #endregion Exception

        #region Boolean
        /// <summary>Asserts that the given boolean expression is true.</summary>
        public static void True(bool expression, string? msg = null, [CallerLineNumber] int ln = 0, [CallerFilePath] string path = "")
        {
            if (!expression)
                Assert.Fail($"{msg}{(msg == null ? "" : "\n")}'{nameof(expression)}' is false!\n[{path}:{ln}]");
        }
        /// <summary>Asserts that the given boolean expression is false.</summary>
        public static void False(bool expression, string? msg = null, [CallerLineNumber] int ln = 0, [CallerFilePath] string path = "")
        {
            if (expression)
                Assert.Fail($"{msg}{(msg == null ? "" : "\n")}'{nameof(expression)}' is true!\n[{path}:{ln}]");
        }
        #endregion Boolean

        #region Enumerable
        /// <summary>Assert that all enumerated objects return true with the given predicate.</summary>
        public static void All<T>(IEnumerable<T> enumerable, Predicate<T> predicate, string? msg = null, [CallerLineNumber] int ln = 0, [CallerFilePath] string path = "")
        {
            int i = 0;
            foreach (T item in enumerable)
            {
                if (!predicate(item))
                    Assert.Fail($"{msg}{(msg == null ? "" : "\n")}Predicate returned false for item {i}:  '{item}'\n[{path}:{ln}]");
                ++i;
            }
        }
        /// <summary>Assert that any enumerated object returns true with the given predicate.</summary>
        public static void Any<T>(IEnumerable<T> enumerable, Predicate<T> predicate, string? msg = null, [CallerLineNumber] int ln = 0, [CallerFilePath] string path = "")
        {
            int i = 0;
            foreach (T item in enumerable)
            {
                if (!predicate(item))
                    Assert.Fail($"{msg}{(msg == null ? "" : "\n")}Predicate returned true for item {i}:  '{item}'\n[{path}:{ln}]");
                ++i;
            }
        }
        /// <summary>Assert that <paramref name="collection"/> is empty.</summary>
        public static void Empty(ICollection collection, string? msg = null, [CallerLineNumber] int ln = 0, [CallerFilePath] string path = "")
        {
            if (!(collection.Count == 0))
                Assert.Fail($"{msg}{(msg == null ? "" : "\n")}{nameof(collection)} is not empty!\n[{path}:{ln}]");
        }
        /// <summary>Assert that <paramref name="collection"/> is not empty.</summary>
        public static void NotEmpty(ICollection collection, string? msg = null, [CallerLineNumber] int ln = 0, [CallerFilePath] string path = "")
        {
            if (!(collection.Count != 0))
                Assert.Fail($"{msg}{(msg == null ? "" : "\n")}{nameof(collection)} is empty!\n[{path}:{ln}]");
        }
        /// <summary>Assert that <paramref name="s"/> is empty.</summary>
        public static void Empty(string s, string? msg = null, [CallerLineNumber] int ln = 0, [CallerFilePath] string path = "")
        {
            if (!(s.Length == 0))
                Assert.Fail($"{msg}{(msg == null ? "" : "\n")}string is not empty!\n[{path}:{ln}]");
        }
        /// <summary>Assert that <paramref name="s"/> is not empty.</summary>
        public static void NotEmpty(string s, string? msg = null, [CallerLineNumber] int ln = 0, [CallerFilePath] string path = "")
        {
            if (!(s.Length != 0))
                Assert.Fail($"{msg}{(msg == null ? "" : "\n")}string is empty!\n[{path}:{ln}]");
        }
        #endregion Enumerable

        #region Event
        /// <summary>Causes an assertion failure if triggered by an event.<br/>This can be used as an event handler for any event with a signature similar to <see cref="EventHandler"/>.</summary>
        /// <param name="s">Sender Parameter.</param>
        /// <param name="e">Event Arguments Parameter.</param>
        public static void OnEvent(object? _, object? _1) => Assert.Fail("Event Triggered!");
        /// <summary>
        /// This event may be used as the source for an event handler to test.<br/>
        /// Use <see cref="NotifyFromEvent"/> to trigger this event.
        /// </summary>
        public static event EventHandler? FromEvent;
        public static void NotifyFromEvent(object? s, EventArgs e) => FromEvent?.Invoke(s, e);
        public static void NotifyFromEvent(object? s) => FromEvent?.Invoke(s, new());
        public static void NotifyFromEvent() => FromEvent?.Invoke(null, new());
        #endregion Event
    }
}
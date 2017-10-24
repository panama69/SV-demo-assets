using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace HP.SOAQ.ServiceVirtualization.Common.Util {
    public static class Validation {

        /// <exception cref="NullReferenceException">
        ///     When value is null.
        /// </exception>
        public static void NotNull<T>(T value) {
            NotNull(value, "Null value not permitted (" + typeof(T) + ")");
        }

        /// <exception cref="NullReferenceException">
        ///     When value is null.
        /// </exception>
        public static void NotNull<T>(T value, String msg) {
            // TODO: There should be 'where T : class' constraint but it seems there is some bug as it cannot accept interfaces!
            if (value == null) {
                throw new NullReferenceException(msg);
            }
        }

        public static void FileExists(String path) {
            NotNull(path, "Path must not be null");
            
            if (!File.Exists(path)) {
                throw new ArgumentException(String.Format("File with path {0} must exists", path));
            }
        }

        public static void PropertyExists(Object obj, String propertyName) {
            PropertyInfo propertyInfo = obj.GetType().GetProperty(propertyName);

            if (propertyInfo == null) {
                throw new ArgumentException("The property with name [" + propertyName 
                    + "] don't exist on [" + obj + "]");
            }
        }

        public static void IsTrue(bool condition) {
            if (condition == false) {
                throw new ArgumentException("The condition must be true.");
            }
        }

        public static void IsTrue(bool condition, String msg) {
            if (condition == false) {
                throw new ArgumentException(msg);
            }
        }

        public static void NotNullOrEmpty(IEnumerable enumerable, String message) {
            NotNull(enumerable, message);
            IEnumerator enumerator = enumerable.GetEnumerator();
            IsTrue(enumerator.MoveNext(), message);
        }

        public static void NotNullOrEmpty<T>(IEnumerable<T> enumerable, String message) {
            NotNull(enumerable, message);
            IEnumerator enumerator = enumerable.GetEnumerator();
            IsTrue(enumerator.MoveNext(), message);
        }

        public static void NotNullOrEmpty(String @string) {
            NotNullOrEmpty(@string, "String must not be null or empty.");
        }

        public static void NotNullOrEmpty(String @string, String message) {
            IsTrue(!string.IsNullOrEmpty(@string), message);
        }
 
    }
}
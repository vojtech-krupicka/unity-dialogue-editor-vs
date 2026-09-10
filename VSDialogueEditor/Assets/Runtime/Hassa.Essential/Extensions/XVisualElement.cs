using System.IO;
using UnityEditor;
using UnityEngine.UIElements;

namespace Hassa.Essentials
{

    /// <summary>
    /// Visual Element Extensions.
    /// </summary>
    public static class XVisualElement
    {
        public static VisualElement CreateChild(this VisualElement parent, params string[] classes)
        {
            var child = new VisualElement();
            child.AddClass(classes).AddTo(parent);
            return child;
        }

        public static T CreateChild<T>(this VisualElement parent, params string[] classes) where T : VisualElement, new()
        {
            var child = new T();
            child.AddClass(classes).AddTo(parent);
            return child;
        }

        public static T AddTo<T>(this T child, VisualElement parent) where T : VisualElement
        {
            parent.Add(child);
            return child;
        }

        public static T WithManipulator<T>(this T ve, IManipulator manipulator) where T : VisualElement
        {
            ve.AddManipulator(manipulator);
            return ve;
        }


        /// <summary>
        /// Add all USS classes to the visual element's class list.
        /// </summary>
        /// <param name="ve">A visual element.</param>
        /// <param name="classNames">List of class names to add to the class list.</param>
        /// <returns>Returns visual element, provides fluent interface.</returns>
        public static T AddClass<T>(this T ve, params string[] classes) where T : VisualElement
        {
            foreach (var cls in classes)
            {
                if (!string.IsNullOrEmpty(cls))
                {
                    ve.AddToClassList(cls);
                }
            }

            return ve;
        }

        /// <summary>
        /// Remove all USS classes from the visual element's class list.
        /// </summary>
        /// <param name="ve">A visual element.</param>
        /// <param name="classNames">List of class names to remove from the class list.</param>
        /// <returns>Returns visual element, provides fluent interface.</returns>
        public static T RemoveClass<T>(this T ve, params string[] classes) where T : VisualElement
        {
            foreach (var cls in classes)
            {
                if (!string.IsNullOrEmpty(cls))
                {
                    ve.RemoveFromClassList(cls);
                }
            }

            return ve;
        }

        /// <summary>
        /// Toggle all USS classes in the visual element's class list.
        /// </summary>
        /// <param name="ve">A visual element.</param>
        /// <param name="classNames">List of class names to toggle in the class list.</param>
        /// <returns>Returns visual element, provides fluent interface.</returns>
        public static T ToggleClass<T>(this T ve, params string[] classes) where T : VisualElement
        {
            foreach (var cls in classes)
            {
                if (!string.IsNullOrEmpty(cls))
                {
                    ve.ToggleInClassList(cls);
                }
            }

            return ve;
        }

        /// <summary>
        /// Add all given stylesheets to the visual element.
        /// </summary>
        /// <param name="ve">A visual element.</param>
        /// <param name="styleSheetsPaths">List of absolute paths.</param>
        /// <returns>Returns visual element, provides fluent interface.</returns>
        public static T AddStyleSheets<T>(this T ve, params string[] styleSheetsPaths) where T : VisualElement
        {
            foreach (var path in styleSheetsPaths) {
                var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>(path);
                if (styleSheet) {
                    ve.styleSheets.Add(styleSheet);
                }
            }

            return ve;
        }

        /// <summary>
        /// Add all given stylesheets relative to rootPath to the visual element.
        /// </summary>
        /// <param name="ve">A visual element.</param>
        /// <param name="rootPath">Root path (i.e. /Assets/Editor)</param>
        /// <param name="styleSheetsPaths">List of paths relative to rootPath (i.e. Resources/Variables.uss).</param>
        /// <returns></returns>
        public static T AddStyleSheets<T>(this T ve, string rootPath, string[] styleSheetsPaths) where T : VisualElement
        {
            foreach (var path in styleSheetsPaths) {
                var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>(Path.Combine(rootPath, path));
                if (styleSheet) {
                    ve.styleSheets.Add(styleSheet);
                }
            }

            return ve;
        }



    }
}

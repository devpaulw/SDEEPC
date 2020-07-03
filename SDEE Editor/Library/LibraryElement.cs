using System.Windows;
using System.Windows.Controls;
using System;

namespace SDEE_Editor
{
    /// <summary>
    /// Object allowing to get a new <seealso cref="FrameworkElement"/> each time we pick it from the library.
    /// </summary>
    /// <remarks>
    /// It must be placed into a Toolbox Library object (e.g. in a <seealso cref="TreeViewItem"/> Tag).<br/>
    /// </remarks>
    public class LibraryElement
    {
        private readonly Func<FrameworkElement> _instanceElement;

        public string ElementName { get; }

        public LibraryElement(string elementName, Func<FrameworkElement> instanceElement)
        {
            ElementName = elementName.Length > 0 ? elementName : throw new ArgumentException(nameof(elementName), "The element name is too small.");
            _instanceElement = instanceElement ?? throw new ArgumentNullException(nameof(instanceElement));
        }

        /// <summary>
        /// It is similar as invoking a <seealso cref="Func{TResult}"/> with <seealso cref="FrameworkElement"/> but more intuitive.
        /// </summary>
        /// <returns>A new constructed FrameworkElement.</returns>
        public FrameworkElement GetElement()
        {
            FrameworkElement instancedElement = _instanceElement.Invoke();
            instancedElement.Name = ElementName;
            return instancedElement;
        }
    }
}
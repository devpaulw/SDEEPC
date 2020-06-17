using System.Windows;
using System.Windows.Controls;
using System;

namespace SDEE_Editor
{
    /// <summary>
    /// Delegate allowing to get a new FrameworkElement each time we pick it from the library.
    /// </summary>
    /// <remarks>
    /// It must be placed into a Toolbox Library object (e.g. in a <seealso cref="TreeViewItem"/> Tag).<br/>
    /// It is similar as <seealso cref="Func{TResult}"/> with <seealso cref="EditorElement"/> but more intuitive.
    /// </remarks>
    /// <returns>A new constructed FrameworkElement.</returns>
    public delegate EditorElement LibraryElement();
}
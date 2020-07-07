using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Markup;
using System.Xml;

namespace SDEE_Editor.Miscellaneous
{
    static class FrameworkElementExtension
    {
        public static T Clone<T>(this T frameworkElement) where T : UIElement
        {
            if (frameworkElement == null)
                throw new ArgumentNullException(nameof(frameworkElement));

            string elemValXaml = XamlWriter.Save(frameworkElement);

            StringReader stringReader = new StringReader(elemValXaml);
            XmlReader xmlReader = XmlReader.Create(stringReader);

            T clonedElement = (T)XamlReader.Load(xmlReader);

            return clonedElement;
        }
    }
}

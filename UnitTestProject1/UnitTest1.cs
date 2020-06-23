using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SDEE_Editor;
using System.Windows.Controls;

namespace UnitTestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestInstanceEditorElement_TooSmallName()
        {
            EditorElement eElem;

            Assert.ThrowsException<ArgumentOutOfRangeException>(() => eElem = new EditorElement("", new Button()));
            
            Assert.ThrowsException<ArgumentNullException>(() => eElem = new EditorElement("lol", null));
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using TestStack.White;
using TestStack.White.UIItems.WindowItems;
using TestStack.White.UIItems;
using TestStack.White.UIItems.WindowItems;
using TestStack.White.UIItems.WindowStripControls;
using TestStack.White.UIItems.MenuItems;
using TestStack.White.UIItems.TreeItems;
using TestStack.White.UIItems.Finders;
using TestStack.White.WebBrowser;
using TestStack.White.WebBrowser.Silverlight;


namespace Golem.Framework
{
    public class Silverlight
    {
        [Test]
        public void Test()
        {
            InternetExplorerWindow browserWindow = InternetExplorer.Launch("http://localhost/white.testsilverlight/TestSilverlightApplicationTestPage.aspx", "FooApp Title - Windows Internet Explorer");
            SilverlightDocument document = browserWindow.SilverlightDocument;
            Button button = document.Get<Button>("buton");
            Label label = document.Get<Label>("status");
        }
    }
}

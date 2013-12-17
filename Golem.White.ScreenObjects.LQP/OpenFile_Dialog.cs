using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestStack.White.UIItems;
using TestStack.White.UIItems.Finders;
using TestStack.White.UIItems.ListBoxItems;
using TestStack.White.UIItems.WindowItems;

namespace Golem.White.ScreenObjects.LQP
{
    public class OpenFile_Dialog
    {
        private Window baseWindow;
        private Window OpenFileWindow;
        private Component<Button> OpenButton;
       

        public OpenFile_Dialog(String windowName)
        {
            baseWindow = WhiteTestBase._app.GetWindow(windowName);
            OpenFileWindow = baseWindow.ModalWindow(SearchCriteria.ByText("Open"));
            OpenButton = new Component<Button>(OpenFileWindow, "OpenButton", SearchCriteria.ByText("Open"));
            
            
        }

        public void OpenaFile(String filepathtoOpen)
        {
            var FileName = OpenFileWindow.Get<ComboBox>(SearchCriteria.ByText("File name:"));
            FileName.EditableText = filepathtoOpen;
            OpenButton.Click();
        }

        public void Cancel()
        {
            var CancelButton = OpenFileWindow.Get<Button>(SearchCriteria.ByText("Cancel"));
            CancelButton.Click();
        }

    }
}

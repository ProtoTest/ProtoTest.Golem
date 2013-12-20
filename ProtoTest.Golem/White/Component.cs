using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using Gallio.Framework;
using ProtoTest.Golem.Tests;
using ProtoTest.Golem.White;
using TestStack.White.UIItems;
using TestStack.White.UIItems.Finders;
using TestStack.White.UIItems.ListBoxItems;
using TestStack.White.UIItems.MenuItems;
using TestStack.White.UIItems.PropertyGridItems;
using TestStack.White.UIItems.WindowItems;
using TestStack.White.UIItems.WPFUIItems;

namespace ProtoTest.Golem.White
{
    public class Component<T> where T : UIItem
    {
        private String _cname;
        private Window _window;

        private String _ComponentType;
        private UIItem _thething;
        private UIItem _rootItem;
        private SearchCriteria _search;

        public UIItem thething
        {
            get
            {
                if (_rootItem != null)
                {
                    _thething = _rootItem.Get<T>(_search);
                }
                else
                {
                    _thething = _window.Get<T>(_search);
                }
                return _thething;
            }
            set { _thething = value; }
        }


        public Component(String window, String title, SearchCriteria search)
        {
            _window = WhiteTestBase.app.GetWindow(window);
            _cname = title;
            _search = search;
        }

        public Component(Component<Menu> comp, String title, SearchCriteria search)
        {
            _window = comp.getWindow();
            _cname = title;
            _search = search;
            _rootItem = comp.thething;
            
        }

        public Component(Component<Panel> comp, String title, SearchCriteria search)
        {
            _window = comp.getWindow();
            _cname = title;
            _search = search;
            _rootItem = comp.thething;
        }
        public Component(Component<GroupBox> comp, String title, SearchCriteria search)
        {
            _window = comp.getWindow();
            _cname = title;
            _search = search;
            _rootItem = comp.thething;
        }

        public Component(Window window, String title, SearchCriteria search)
        {
            _window = window;
            _cname = title;
            _search = search;
        }

        public void Click()
        {
            if (thething != null)
            {
                thething.Click();
            }

        }

        public void setText(String text)
        {
            if (thething != null)
            {
                var whatisthis = thething.GetType();
                DiagnosticLog.WriteLine("***********" + whatisthis.ToString());
                if (whatisthis == typeof(ComboBox))
                {
                    ComboBox comboBox = (ComboBox)thething;
                    comboBox.EditableText = text;
                }
                
                if (whatisthis == typeof(TextBox))
                {
                    TextBox textbox = (TextBox) thething;
                    textbox.Text = text;
                }
            }
        }

        public Window getWindow()
        {
            return _window;
        }

        public UIItem getItem()
        {
            return thething;
        }



    }
}

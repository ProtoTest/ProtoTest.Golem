using System;
using System.Collections.Generic;
using System.Linq;

using System.Text;
using TestStack.White.UIItems;
using TestStack.White.UIItems.Finders;
using TestStack.White.UIItems.WindowItems;

namespace ProtoTest.Golem.White
{
    class Component
    {
        private String _cname;
        private Window _window;
        private List<SearchCriteria> _searchs;
        private String _ComponentType;
        

        public Component(Window baseWindow, String title, String type, SearchCriteria search)
        {
            _window = baseWindow;
            _cname = title;
            _searchs = new List<SearchCriteria>();
            _searchs.Add(search);
            _ComponentType = type;

        }

        public void addSearch(SearchCriteria search)
        {
            if (_searchs != null)
            {
                _searchs.Add(search);
            }
        }

        private void discoverType()
        {
            UIItem thing = _window.Get<Button>(_searchs[0]);
        }

    }
}

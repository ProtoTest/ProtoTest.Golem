using TestStack.White.UIItems.Finders;
using TestStack.White.UIItems.WindowItems;

namespace ProtoTest.Golem.White.Elements
{
    public class Button : TestStack.White.UIItems.Button
    {
        public BaseItem<TestStack.White.UIItems.Button> item;

        public Button(string name, SearchCriteria criteria, Window window = null)
        {
            if (window == null) window = WhiteTestBase.window;
            item = new BaseItem<TestStack.White.UIItems.Button>(name, criteria, window);
        }
    }
}
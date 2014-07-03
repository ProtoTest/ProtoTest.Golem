using System.Windows.Automation;

namespace ProtoTest.Golem.Purple.PurpleElements
{
    public class PurpleTable : PurpleElementBase
    {
        private int _rowCount = -1;
        private int _colCount = -1;

        public int RowCount
        {
            get
            {
                if (_rowCount.Equals(-1))
                {
                    SetCounts();
                }
                return _rowCount;
            }
        }

        public int ColumnCount
        {
            get
            {
                if (_colCount.Equals(-1))
                {
                    SetCounts();
                }
                return _colCount;
            }
        }

        public PurpleTable(string name, string locatorPath) : base(name, locatorPath)
        {

        }

        public string GetValue(int row, int column)
        {
            string somenonsense = "";
            object basePattern;
            if (PurpleElement.TryGetCurrentPattern(TablePattern.Pattern, out basePattern))
            {
                TablePattern gridItem = (BasePattern) basePattern as TablePattern;
                if (gridItem != null)
                {
                    var tableitem = gridItem.GetItem(row, column);
                    somenonsense = tableitem.Current.Name;
                }

            }
            return somenonsense;
        }

        private void SetCounts()
        {
            object basePattern;
            if (PurpleElement.TryGetCurrentPattern(TablePattern.Pattern, out basePattern))
            {
                TablePattern gridItem = (BasePattern)basePattern as TablePattern;
                if (gridItem != null)
                {
                    _rowCount = gridItem.Current.RowCount;
                    _colCount = gridItem.Current.ColumnCount;
                }
            } 
        }
        

    }
}

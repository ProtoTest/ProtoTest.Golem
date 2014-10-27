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
        public PurpleTable(string name, AutomationElement element) :base(name,element){}

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

        // Changes for the sake of speeding up the code - Start
        
        private bool PElementPattern;

        private object basePattern2;
        public void EvaluatePattern()
        {
            if (PurpleElement.TryGetCurrentPattern(TablePattern.Pattern, out basePattern2))
            {
                PElementPattern = true;
            }
            else
            {
                PElementPattern = false;
            }
        }

        public string GetValueNew(int row, int column)
        {
            string somenonsense = "";
            if (PElementPattern)
            {
                TablePattern gridItem = (BasePattern)basePattern2 as TablePattern;
                if (gridItem != null)
                {
                    var tableitem = gridItem.GetItem(row, column);
                    somenonsense = tableitem.Current.Name;
                }

            }
            return somenonsense;
        }

        // Changes for the sake of speeding up the code - End

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

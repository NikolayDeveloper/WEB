using System.Collections.Generic;

namespace Iserv.Niis.Report
{
    public class ReportData
    {
        public List<Row> Rows { get; set; }
    }

    public class Row
    {
        public List<Cell> Cells { get; set; }

        /// <summary>
        /// Указывает что это строка является заголовком и будет отцентрирована и отрисована соотвествующим образом
        /// </summary>
        public bool IsHeader { get; set; }
    }

    public class Cell
    {
        public dynamic Value { get; set; }

        private int _collSpan { get; set; }
        /// <summary>
        /// Забирает количество ячеек вправо
        /// </summary>
        public int CollSpan
        {
            get
            {
                return _collSpan > 1 ? _collSpan : 0;
            }
            set
            {
                _collSpan = value;

            }
        }

        private int _rowSpan { get; set; }
        /// <summary>
        /// Забирает количество ячеек вниз
        /// </summary>
        public int RowSpan
        {
            get
            {
                return _rowSpan > 1 ? _rowSpan : 0;
            }
            set
            {
                _rowSpan = value;

            }
        }

        public bool IsBold { get; set; }

        public bool IsTextAlignCenter { get; set; }
    }
}

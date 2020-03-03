using System.Collections.Generic;

namespace Iserv.Niis.Report.Chart
{
    public class ChartDataBase
    {
        public ChartType ChartType { get; set; }
        public List<string[]> Labels { get; set; }
        public ChartDataset[] Datasets { get; set; }
    }
}

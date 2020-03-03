using System;
using System.Collections.Generic;
using Iserv.Niis.Report.Chart;
using Iserv.Niis.Report.ReportBusinessLogic;

namespace Iserv.Niis.Report.Reports
{
    /// <summary>
    /// Отчет Сведения по поступившим в РГП «НИИС» заявкам на выдачу охранных документов на объекты промышленной собственности за год
    /// </summary>
    internal class ReceivedRequestReport : ReportBase<GetReportDataReceivedRequestReportQuery>
    {
        public ReceivedRequestReport(ReportConditionData rportConditionData) : base(rportConditionData)
        {
            rportConditionData.DateFrom = rportConditionData.DateFrom ?? new DateTimeOffset(new DateTime(DateTime.Now.Year, 1, 1));
            rportConditionData.DateTo = rportConditionData.DateTo ?? DateTimeOffset.Now;

            rportConditionData.DateFrom = rportConditionData.DateFrom.Value.ToLocalTime();
            rportConditionData.DateTo = rportConditionData.DateTo.Value.ToLocalTime();
        }

        protected override string ReportTemplate => "ReceivedRequestReportTemplate.xlsx";

        protected override int RowStart => 1;
        protected override int ColumnStart => 1;

        public override ChartDataBase GetChartData()
        {
            var data = GetData();

            var chartData = new ChartDataBase { ChartType = ChartType.Bar };

            var labels = new List<string[]>();
            var chartDataSet = new List<BarDataSet>();

            var residents = new BarDataSet { Label = "Национальные заявители", Data = new double[data.Rows.Count], BackgroundColor = "#2c8ebb", BorderColor = "#2c8ebb" };
            var nonResidents = new BarDataSet { Label = "Иностранне заявители", Data = new double[data.Rows.Count], BackgroundColor = "#b56b2c", BorderColor = "#b56b2c" };

            var dataNumber = 0;
            for (int i = 0; i < data.Rows.Count - 1; i++)
            {
                var row = data.Rows[i];
                if (row.IsHeader == false)
                {
                    residents.Data[dataNumber] = row.Cells[2].Value;
                    nonResidents.Data[dataNumber] = row.Cells[3].Value;

                    dataNumber++;

                    string label = row.Cells[1].Value.ToString();

                    labels.Add(label.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries));
                }
            }
            chartDataSet.Add(residents);
            chartDataSet.Add(nonResidents);

            chartData.Labels = labels;
            chartData.Datasets = chartDataSet.ToArray();

            return chartData;
        }

        protected override List<Row> GetReportHeader()
        {
            var header = new List<Row>
            {
                new Row
                {
                    IsHeader = true,

                    Cells = new List<Cell>
                    {
                        new Cell{ Value = "№", RowSpan = 2 },
                        new Cell{ Value =
                            $"Сведения по заявкам с {ReportConditionData.DateFrom.Value.ToLocalTime().Date:dd.MM.yyyy} по {ReportConditionData.DateTo.Value.ToLocalTime().Date:dd.MM.yyyy}", RowSpan = 2 },

                        new Cell{ Value = "Количество поступивших заявок", CollSpan = 3 }
                    }
                },
                new Row
                {
                    IsHeader = true,

                    Cells = new List<Cell>
                    {
                        new Cell{ },
                        new Cell{ },
                        new Cell{ Value = "От национальных заявителей" },
                        new Cell{ Value = "От иностранных заявителей" },
                        new Cell{ Value = "Всего"  },
                    }
                }
            };

            header.Reverse();

            return header;
        }
    }
}

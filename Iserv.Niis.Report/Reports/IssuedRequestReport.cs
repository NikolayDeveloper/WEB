using Iserv.Niis.Report.Chart;
using Iserv.Niis.Report.ReportBusinessLogic;
using System;
using System.Collections.Generic;

namespace Iserv.Niis.Report.Reports
{
    /// <summary>
    /// Сведения по выданным охранным документам на объекты промышленной собственности РГП «НИИС» за указанный период
    /// </summary>
    internal class IssuedProtectionDocumentsReport : ReportBase<GetReportDataIssuedProtectionDocumentsReportQuery>
    {
        public IssuedProtectionDocumentsReport(ReportConditionData rportConditionData) : base(rportConditionData)
        {
            rportConditionData.DateFrom = rportConditionData.DateFrom ?? new DateTimeOffset(new DateTime(DateTime.Now.Year, 1, 1));
            rportConditionData.DateTo = rportConditionData.DateTo ?? DateTimeOffset.Now;

            rportConditionData.DateFrom = rportConditionData.DateFrom.Value.ToLocalTime();
            rportConditionData.DateTo = rportConditionData.DateTo.Value.ToLocalTime();
        }

        protected override string ReportTemplate => "IssuedProtectionDocumentsReportTemplate.xlsx";

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
            var sum = new BarDataSet { Label = "Всего", Data = new double[data.Rows.Count], BackgroundColor = "#cccccc", BorderColor = "#cccccc" };

            var dataNumber = 0;
            for (int i = 0; i < data.Rows.Count; i++)
            {
                //Пропускаем общее количество
                if (i == 10)
                {
                    continue;
                }

                var row = data.Rows[i];

                //workaround для общеизвестных, отображал некорректно
                if (i == 11)
                {
                    residents.Data[dataNumber] = row.Cells[3].Value ?? 0;
                    nonResidents.Data[dataNumber] = row.Cells[4].Value ?? 0;
                    sum.Data[dataNumber] = residents.Data[dataNumber] + nonResidents.Data[dataNumber];

                    dataNumber++;

                    string label = row.Cells[1].Value.ToString();

                    labels.Add(label.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries));
                    continue;
                }
                //остальные типы
                if (row.IsHeader == false)
                {
                    if (dataNumber < 5)
                    {
                        residents.Data[dataNumber] = row.Cells[2].Value ?? 0;
                        nonResidents.Data[dataNumber] = row.Cells[3].Value ?? 0;
                    }
                    else
                    {
                        try
                        {
                            residents.Data[dataNumber] = row.Cells[4].Value ?? 0;
                            nonResidents.Data[dataNumber] = row.Cells[5].Value ?? 0;
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                    }
                    sum.Data[dataNumber] = residents.Data[dataNumber] + nonResidents.Data[dataNumber];

                    dataNumber++;

                    string label = row.Cells[1].Value.ToString();

                    labels.Add(label.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries));
                }
            }
            chartDataSet.Add(residents);
            chartDataSet.Add(nonResidents);
            chartDataSet.Add(sum);

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
                        new Cell{ Value = "Объекты промышленной собственности", RowSpan = 2 },
                        new Cell{ Value = "Кол-во выданных охранных документов (патентов)", CollSpan = 2 },
                        new Cell{ Value = "Кол-во  выданных охранных документов (свидетельств, выписок)", CollSpan = 2 },
                        new Cell{ Value = "Всего", RowSpan = 2 }
                    }
                },
                new Row
                {
                    IsHeader = true,

                    Cells = new List<Cell>
                    {
                        new Cell{ },
                        new Cell{ },
                        new Cell{ Value = "Национальным заявителям" },
                        new Cell{ Value = "Иностранным заявителям" },
                        new Cell{ Value = "Национальным заявителям" },
                        new Cell{ Value = "Иностранным заявителям" },
                    }
                }
            };

            header.Reverse();

            return header;
        }
    }
}

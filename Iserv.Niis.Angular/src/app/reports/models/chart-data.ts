export class ChartData {
    labels: string[][];
    datasets: ChartDataset[];
    chartType: ChartType;
}

export abstract class ChartDataset {
    data: number[];
}

export class PieChartDataset extends ChartDataset {
    backgroundColor: string[];
    hoverBackgroundColor: string[];
}

export class LineChartDataset extends ChartDataset {
    label: string;
    fill: boolean;
    borderColor: string;
}

export class BarChartDataset extends ChartDataset {
    label: string;
    backgroundColor: string;
    borderColor: string;
}

export class RadarChartDataset extends ChartDataset {
    label: string;
    backgroundColor: string;
    borderColor: string;
    pointBackgroundColor: string;
    pointBorderColor: string;
    pointHoverBackgroundColor: string;
    pointHoverBorderColor: string;
}

export class PolarAreaChartDataset extends ChartDataset {
    label: string;
    backgroundColor: string[];
}

export class DoughnutChartDataset extends ChartDataset {
    backgroundColor: string[];
    hoverBackgroundColor: string[];
}

export enum ChartType {
    None = 0,
    Doughnut = 1,
    PolarArea = 2,
    Radar = 3,
    Bar = 4,
    Line = 5,
    Pie = 6
}

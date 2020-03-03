export class ReportData {
    rows: Array<Row>;
}
export class Row {
    cells: Array<Cell>;

    /// <summary>
    /// Указывает что это строка является заголовком и будет отцентрирована и отрисована соотвествующим образом
    /// </summary>
    isHeader: boolean;
}

export class Cell {
    value: any;

    /// <summary>
    /// Забирает количество ячеек вправо
    /// </summary>
    collSpan: number;

    /// <summary>
    /// Забирает количество ячеек вниз
    /// </summary>
    cowSpan: number;

    isBold: boolean;
}
/**Обертка над ответом для получения общего количества записей из заголовков ответа */
export class PageResponse<T> {
    constructor(public total: number,
                public items: T[]) { }
}

import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
    name: 'filterByParams',
    pure: false
})
export class FilterByParamsPipe implements PipeTransform {
    /**
     * Производит фильтрацию массива объектов `items`
     * @param items Массив объектов для фильтрации
     * @param filter Объект с ключами для фильтрации
     * @return Отфильтрованный массив объектов
     */
    transform(items: any[], filter: Object): any[] {
        if (!items || !filter) {
            return items;
        }

        try {
            return items.filter(item => Object.keys(filter).some(key => {
                if (item.routeCodeFiltering[key]) {
                    return item.routeCodeFiltering[key] === filter[key];
                }
            }));
        } catch (error) {
            return items;
        }
    }
}

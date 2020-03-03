import { Pipe, PipeTransform } from '@angular/core';
@Pipe({ name: 'keys' })
export class KeysPipe implements PipeTransform {
    transform(value, args: string[]): any {
        const keys = [];
        Object.keys(value).forEach(key => {
            if (!isNaN(parseInt(key, 10)) && value[key] !== 'NONE') {
                keys.push({ key: key, value: value[key] });
            }
        });
        return keys;
    }
}

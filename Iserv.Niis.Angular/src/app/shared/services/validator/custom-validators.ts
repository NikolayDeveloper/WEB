import { AbstractControl, ValidatorFn } from '@angular/forms';

export function emailValidator(control: AbstractControl): { [key: string]: any } {

    const emailRegex = /[A-Z0-9._%-]+@[A-Z0-9.-]+\.[A-Z]{2,4}/i;
    const value = control.value;

    if (!value) {
        return null;
    }

    const result = emailRegex.test(value);

    if (result) {
        return null;
    } else {
        return { 'emailValidator': { value } }
    }
}

export function rangeValidator(minValue: number, maxValue: number): ValidatorFn {
    return (control: AbstractControl): { [key: string]: any } => {
        const value = control.value;
        const numValue: number = +value;

        if (isNaN(numValue)) {
            return { 'rangeValidator': { value } };
        } else if (numValue < minValue || numValue > maxValue) {
            return { 'rangeValidator': { value } };
        } else {
            return null;
        }
    }
}

/**
 * Проверяет корректность ИИН
 *
 * @export
 * @param {AbstractControl} c контрол для проверки
 * @returns {{ [key: string]: any }} | null возвращает объект в случае ошибки и null если проверка успешна
 */
export function xinValidator(c: AbstractControl): { [key: string]: any } {
    if (!c.value) { return null; }
    const xinRegex = /(\d{2}((0[0-9])|(1[0-2]))\d{8})/;
    const value = c.value.toString();
    if (value.length < 12
        || isNaN(value)
        || !(+(value) > 0)
        || !xinRegex.test(value)) {
        return { 'xin': { value: value } }
    }
    let checkSum =
        (+value[0] * 1
            + +value[1] * 2
            + +value[2] * 3
            + +value[3] * 4
            + +value[4] * 5
            + +value[5] * 6
            + +value[6] * 7
            + +value[7] * 8
            + +value[8] * 9
            + +value[9] * 10
            + +value[10] * 11) % 11;

    if (checkSum !== 10) { return null };
    checkSum =
        (+value[0] * 3
            + +value[1] * 4
            + +value[2] * 5
            + +value[3] * 6
            + +value[4] * 7
            + +value[5] * 8
            + +value[6] * 9
            + +value[7] * 10
            + +value[8] * 11
            + +value[9] * 1
            + +value[10] * 2) % 11
    if (checkSum === 10) {
        return { 'xin': { value: value } }
    }

    return null;
}

export function requestNumValidator(c: AbstractControl): { [key: string]: any } {
    if (!c.value) { return null; }
    const value = c.value.toString().split(/\W/);
    let invalid = false;
    value.forEach(element => {
        if (isNaN(element)) {
            invalid = true;
        }
    }
    );
    if (!invalid) {
        return null;
    }
    return { 'requestNum': { value: value } };
}



export function referencedMaxValueValidator(context: any, fieldname: string): ValidatorFn {
    const fieldnameConst = fieldname;

    return (control: AbstractControl): { [key: string]: any } => {
        const value = control.value;

        if (isNaN(value)) {
            return { 'maxValueValidator': { value } };
        } else if (value > context[fieldnameConst]) {
            return { 'maxValueValidator': { value } };
        } else {
            return null;
        }
    }
}

export const phoneMask: (string | RegExp)[] =
    ['+', /[1-9]/, ' ', '(', /[1-9]/, /\d/, /\d/, ')', ' ', /\d/, /\d/, /\d/, '-', /\d/, /\d/, '-', /\d/, /\d/];

export const nmptNumberMask: (string | RegExp)[] = [/[0-9]/, /[0-9]/, '-', /[0-9]/, /[0-9]/];

export function phoneValidator(control: AbstractControl): { [key: string]: any } {
    const value = control.value;
    if (!value) {
        return null;
    }

    if (value.replace(/\_/g, '').length === phoneMask.length) {
        return null;
    } else {
        return { 'phoneValidator': { value } };
    }
}
export function numberMask(strNumber: string) {
    const digitPattern = /\d/;

    return strNumber
        .split('')
        .map((char, index) =>
            digitPattern.test(char)
                ? digitPattern
                : []);
}

export function positiveNumberValidator(control: AbstractControl): { [key: string]: any } {
    const value = control.value;
    if (!value) {
        return null;
    }
    if (!isNaN(value) && value > 0) {
        return null;
    } else {
        return { 'positiveNumberValidator': { value } };
    }
}

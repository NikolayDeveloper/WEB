/**
 * Класс описания комбинации.
 * @author Alexander Munko <seasle98@gmail.com>
 * @class
 */
export class Combination {
  EQUALS: Set<string|number>;
  NOT_EQUALS: Set<string|number>;
  ANY: boolean;
  EMPTY: boolean;

  /**
   * Создает экземпляр с наборами данных и флагами.
   */
  constructor() {
    this.EQUALS = new Set();
    this.NOT_EQUALS = new Set();
    this.ANY = false;
    this.EMPTY = false;
  }

  /**
   * Добавляет значения в набор «равных» значений.
   * @param {string|number|string[]|number[]} values Набор значений.
   * @return {Combination} Экземпляр класса.
   */
  equals(...values) {
    for (let value of values) {
      this.EQUALS.add(value);
    }

    return this;
  }

  /**
   * Добавляет значения в набор «не равных» значений.
   * @param {string|number|string[]|number[]} values Набор значений.
   * @return {Combination} Экземпляр класса.
   */
  notEquals(...values) {
    for (let value of values) {
      this.NOT_EQUALS.add(value);
    }

    return this;
  }

  /**
   * Устанавливает флаг «любой».
   * @return {Combination} Экземпляр класса.
   */
  any() {
    this.ANY = true;

    return this;
  }

  /**
   * Устанавливает флаг «пустой».
   * @return {Combination} Экземпляр класса.
   */
  empty() {
    this.EMPTY = true;

    return this;
  }

  /**
   * Мягкая проверка значений в условиях.
   * Возвращает `false` при любом условии «не равно» или если не переданы аргументы.
   * Возвращает всегда `true` если установлен флаг «любой».
   * Возвращает `true` в том случае, если хоть одно условие типа «равно» совпало.
   * @param {string|number|string[]|number[]} values Набор значений.
   * @return {boolean} Значение проверки.
   */
  some(...values) {
    if (!this.EMPTY && !values.length) {
      return false;
    }

    if (values.some((value) => this.NOT_EQUALS.has(value))) {
      return false;
    }

    if (this.ANY) {
      return true;
    }

    if (this.EMPTY && !values.length) {
      return true;
    }

    return values.some((value) => this.EQUALS.has(value));
  }

  /**
   * Строгая проверка значений в условиях.
   * Возвращает `false` при любом условии «не равно» или если не переданы аргументы.
   * Если установлен флаг «любой» проверяет совпадение всех значений из набора «равно» в наборе переданных значений.
   * Если флаг «любой» не установлен проверяет совпадение всех переданных значений в наборе «равно»
   * @param {string|number|string[]|number[]} values Набор значений.
   * @return {boolean} Значение проверки.
   */
  every(...values) {
    if (!this.EMPTY && !values.length) {
      return false;
    }

    if (values.some((value) => this.NOT_EQUALS.has(value))) {
      return false;
    }

    if (this.ANY) {
      // Лучший вариант `[...this.EQUALS]`, но необходимо трогать компилятор TS.
      const equals = Array.from(this.EQUALS.values());

      return equals.every((value) => values.includes(value));
    } else {
      return values.length === this.EQUALS.size && values.every((value) => this.EQUALS.has(value));
    }
  }
}

import { CombineOperatorEnum } from './combine-operator.enum';
export const Delimiter = '_';

export const Operators = {
  sort: Delimiter + 'sort',
  order: Delimiter + 'order',
  like: Delimiter + 'like',
  greaterThan: Delimiter + 'gt',
  greaterThanEqual: Delimiter + 'gte',
  equal: Delimiter + 'eq',
  lessThan: Delimiter + 'lt',
  lessThanEqual: Delimiter + 'lte',
  fullTextSearch: Delimiter + 'q',
  in: Delimiter + 'in',
  contains: Delimiter + 'contains',
  containsDateRange: Delimiter + 'crange',
  1: 'and' + Delimiter,
  2: 'or' + Delimiter,
  3: 'nand' + Delimiter,
  4: 'nor' + Delimiter,
  commonAnd: 'cand' + Delimiter,
  filterFields: Delimiter + 'filterfields'
};

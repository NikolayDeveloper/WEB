export enum CustomerRole {
  /**
   * Заявитель
   */
  Declarant = '1',
  /**
   * Автор
   */
  Author = '2',
  /**
   * Патентный поверенный
   */
  PatentAttorney = '4',
  /**
   * Контактное лицо
   */
  Contact = 'CORRESPONDENCE',
  /**
   * Доверенное лицо
   */
  Confidant = '6',
  /**
   * Адресат для переписки
   */
  CorrespondingRecipient = 'CORRESPONDENCE',
  /**
   * Владелец
   */
  Owner = 'OWNER',
  /**
   * Адрес заявителя
   */
  DeclarantAddress = '',
  /**
   * Сторона 1
   */
  SideOne = '7',
  /**
   * Сторона 2
   */
  SideTwo = '8',
  /**
   * Адресат
   */
  Addressee = 'Addressee'
}

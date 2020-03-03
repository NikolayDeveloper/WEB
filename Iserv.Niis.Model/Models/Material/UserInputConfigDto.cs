using System.Collections.Generic;
using System.Linq;

namespace Iserv.Niis.Model.Models.Material
{
    /// <summary>
    /// Конфигурация для построения формы ввода пользовательских данных для шаблона
    /// </summary>
    public class UserInputConfigDto
    {
        /// <summary>
        /// Требуется ввод данных пользователем
        /// </summary>
        public bool RequireUserInput => FieldsConfig.Any();

        /// <summary>
        /// Конфигурация полей ввода
        /// </summary>
        public List<UserInputFieldConfig> FieldsConfig { get; set; }

        /// <summary>
        /// Конфигурация для постороения одного поля формы ввода пользовательских данных для шаблона
        /// </summary>
        public class UserInputFieldConfig
        {
            /// <summary>
            /// Идентификатор поля в шаблоне word
            /// </summary>
            public string Key { get; set; }

            /// <summary>
            /// Название поля
            /// </summary>
            public string Label { get; set; }

            /// <summary>
            /// Предзаполненное значение поля
            /// </summary>
            public string Value { get; set; }

            /// <summary>
            /// Поле сожержит html
            /// </summary>
            public bool RichInput { get; set; }

            /// <summary>
            /// Обязательное поле
            /// </summary>
            public bool Required { get; set; }
        }
    }
}

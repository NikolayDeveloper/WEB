using System;
using FluentValidation;

namespace Iserv.Niis.Features.Helpers
{
    public static class FluentValidators
    {

        public static IRuleBuilderOptions<T, string> IsCorrectXinFormat<T>(this IRuleBuilder<T, string> builder)
        {
            return builder.NotNull()
                .Matches("(\\d{2}((0[0-9])|(1[0-2]))\\d{8})")
                .Must(ValidateXinFunc);
        }

        private static bool ValidateXinFunc(string input)
        {
            if (!long.TryParse(input, out long numberValueResult) || !(numberValueResult > 0))
            {
                return false;
            }
            var checkSum =
            (Convert.ToInt32(input[0]) * 1
             + Convert.ToInt32(input[1]) * 2
             + Convert.ToInt32(input[2]) * 3
             + Convert.ToInt32(input[3]) * 4
             + Convert.ToInt32(input[4]) * 5
             + Convert.ToInt32(input[5]) * 6
             + Convert.ToInt32(input[6]) * 7
             + Convert.ToInt32(input[7]) * 8
             + Convert.ToInt32(input[8]) * 9
             + Convert.ToInt32(input[9]) * 10
             + Convert.ToInt32(input[10]) * 11) % 11;

            if (checkSum != 10) return true;
            checkSum =
            (Convert.ToInt32(input[0]) * 3
             + Convert.ToInt32(input[1]) * 4
             + Convert.ToInt32(input[2]) * 5
             + Convert.ToInt32(input[3]) * 6
             + Convert.ToInt32(input[4]) * 7
             + Convert.ToInt32(input[5]) * 8
             + Convert.ToInt32(input[6]) * 9
             + Convert.ToInt32(input[7]) * 10
             + Convert.ToInt32(input[8]) * 11
             + Convert.ToInt32(input[9]) * 1
             + Convert.ToInt32(input[10]) * 2) % 11;
            return checkSum != 10;
        }
    }
}
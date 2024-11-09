using System;
using System.ComponentModel;

namespace Core.Extensions;

public static class EnumExtensions
{
    public static string GetDescription(this Enum enumValue)
    {
        var fieldInfo = enumValue.GetType().GetField(enumValue.ToString());

        var attributes =
            (DescriptionAttribute[])fieldInfo!.GetCustomAttributes(
                typeof(DescriptionAttribute),
                false);

        return attributes.Length > 0
            ? attributes[0].Description
            : enumValue.ToString();
    }
}
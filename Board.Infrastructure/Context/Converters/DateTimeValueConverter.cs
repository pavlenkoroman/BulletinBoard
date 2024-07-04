using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Board.Infrastructure.Context.Converters;

internal sealed class DateTimeValueConverter() : ValueConverter<DateTime, DateTime>(clrValue =>
        clrValue.Kind == DateTimeKind.Utc
            ? clrValue
            : clrValue.ToUniversalTime(),
    dbValue => DateTime.SpecifyKind(dbValue, DateTimeKind.Utc));

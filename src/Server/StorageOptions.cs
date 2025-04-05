

using Femur;
using FluentValidation;

namespace Server;

public class StorageOptions : IStandardOptions<StorageOptions>
{
    public static string SectionName => "Storage";

    public required string BaseUrl { get; set; }

    public static void SetupValidator(AbstractValidator<StorageOptions> validator)
    {
    }
}

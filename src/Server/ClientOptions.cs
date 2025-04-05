

using Femur;
using FluentValidation;

namespace Server;

public class ClientOptions : IStandardOptions<ClientOptions>
{
    public static string SectionName => "Client";

    public required string BaseUrl { get; set; }

    public static void SetupValidator(AbstractValidator<ClientOptions> validator)
    {
    }
}

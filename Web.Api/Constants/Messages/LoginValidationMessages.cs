using Microsoft.Extensions.Localization;

namespace Web.Api.Constants.Messages;

public class LoginValidationMessages(IStringLocalizer<LoginValidationMessages> localize)
{
    public string EmailRequired => localize["EmailRequired"];
    public string EmailInvalid => localize["EmailInvalid"];
    public string PasswordRequired => localize["PasswordRequired"];
}
using static GeekShop.IdentityServer.Pages.Login.ViewModel;

namespace GeekShop.IdentityServer.Pages.Account.Register;

public class ViewModel
{
    public bool AllowRememberLogin { get; set; } = true;
    public bool EnableLocalLogin { get; set; } = true;

    public IEnumerable<ExternalProvider> ExternalProviders { get; set; } = Enumerable.Empty<ExternalProvider>();
    public IEnumerable<ExternalProvider> VisibleExternalProviders => ExternalProviders.Where(x => !String.IsNullOrWhiteSpace(x.DisplayName));
}
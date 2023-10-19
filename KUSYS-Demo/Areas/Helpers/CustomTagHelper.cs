using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Razor.TagHelpers;
using KUSYS_Demo.Entities.Concrete;

namespace KUSYS_Demo.Areas.Helper
{
    [HtmlTargetElement("td", Attributes = "user-role")]
    [HtmlTargetElement("label", Attributes = "user-label-role")]
    public class CustomTagHelper : TagHelper
    {
        public UserManager<AppUser> UserManager { get; set; }

        public CustomTagHelper(UserManager<AppUser> userManager)
        {
            UserManager = userManager;
        }

        [HtmlAttributeName("user-role")]
        public string UserId { get; set; }

        public override async Task ProcessAsync(TagHelperContext context, TagHelperOutput output)
        {
            AppUser userSelector = await UserManager.FindByIdAsync(UserId);
            IList<string> roles = await UserManager.GetRolesAsync(userSelector);

            string html = string.Empty;
            roles.ToList().ForEach(x =>
            {
                html += $"<span class='badge badge-info'> {x} </span>";
            });

            output.Content.SetHtmlContent(html);

        }
    }
}

using Microsoft.AspNetCore.Identity;

namespace NewServer.Data
{
    public class ApplicationUser : IdentityUser
    {
        // 資訊擴充使用

        /// <summary>
        /// Gets or sets the provider name.
        /// </summary>
        public string? ProviderName { get; set; }

        /// <summary>
        /// Gets or sets the provider subject identifier.
        /// </summary>
        public string? ProviderSubjectId { get; set; }
    }
}



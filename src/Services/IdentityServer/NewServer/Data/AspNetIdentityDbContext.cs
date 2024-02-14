using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace NewServer.Data
{
	public class AspNetIdentityDbContext : IdentityDbContext<ApplicationUser, IdentityRole,string>
	{
		public AspNetIdentityDbContext(DbContextOptions<AspNetIdentityDbContext> options)
		  : base(options)
		{
		}
	}
}

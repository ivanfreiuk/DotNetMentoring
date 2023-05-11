using System.Data.Entity;

namespace ProfileSample.DAL
{
    public class ProfileSampleDbContext : DbContext
    {
        public ProfileSampleDbContext()
            : base("name=ProfileSampleDbContext")
        {
        }


        public virtual DbSet<ImgSource> ImgSources { get; set; }
    }
}
using Microsoft.EntityFrameworkCore;
using MigrationReview.Model;

namespace EFCoreReview.App
{
    public class Migration
    {
        public void RunMigration()
        {
            using (var context = new MyDbContext())
            {
                context.Database.Migrate();
            }
        }
    }
}

using Iserv.Niis.DataAccess.EntityFramework.Abstract;
using Iserv.Niis.Domain.Entities.Settings;
using Microsoft.EntityFrameworkCore;

namespace Iserv.Niis.DataAccess.EntityFramework.Mappings.Settings
{
    public class UserSettingsMap : IMapBuilder
    {
        public void Build(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserSetting>()
                .ToTable("UserSettings")
                .HasKey(us => us.Id);

            modelBuilder.Entity<UserSetting>()
                .HasOne(us => us.User)
                .WithMany(au => au.UserSettings)
                .HasForeignKey(us => us.UserId)
                .IsRequired();
        }
    }
}

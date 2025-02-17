using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Zust.Core.Entities;
using Zust.Core.Enums;
using Zust.DAL.Settings;

namespace Zust.DAL.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder
            .HasKey(x => x.Id);

        builder
            .HasIndex(x => x.UserName)
            .IsUnique();

        builder
            .HasIndex(x => x.Email)
            .IsUnique();

        builder
            .HasOne(x => x.Occupation)
            .WithMany(o => o.Users)
            .HasForeignKey(x => x.OccupationId);

        builder
            .HasOne(x => x.Gender)
            .WithMany(g => g.Users)
            .HasForeignKey(x => x.GenderId);

        builder
            .HasOne(x => x.RelationStatus)
            .WithMany(r => r.Users)
            .HasForeignKey(x => x.RelationStatusId);

        builder
            .HasOne(x => x.BloodGroup)
            .WithMany(b => b.Users)
            .HasForeignKey(x => x.BloodGroupId);

        builder
            .HasOne(x => x.Language)
            .WithMany(l => l.Users)
            .HasForeignKey(x => x.LanguageId);

        builder
            .Property(x => x.Role)
            .HasDefaultValue((int)Roles.Member)
            .IsRequired();

        builder
            .Property(x => x.CreatedAt)
            .HasDefaultValueSql("GETDATE()");

        builder
            .Property(x => x.ProfileImageUrl)
            .HasDefaultValue("https://finalprojectolympus.blob.core.windows.net/images/defaultuserimage.jpg")
            .IsRequired(false);

        builder
            .Property(x => x.CoverImageUrl)
            .HasDefaultValue("https://finalprojectolympus.blob.core.windows.net/images/defaultusercover.jpg")
            .IsRequired(false);

        builder
            .Property(x => x.FirstName)
            .HasMaxLength(UserSetting.FirstNameLength)
            .IsRequired(true);

        builder
            .Property(x => x.LastName)
            .HasMaxLength(UserSetting.LastNameLength)
            .IsRequired(true);

        builder
            .Property(x => x.UserName)
            .HasMaxLength(UserSetting.UserNameLength)
            .IsRequired(true);

        builder
            .Property(x => x.Email)
            .HasMaxLength(UserSetting.EmailLength) //maximum length in gmail.com
            .IsRequired(true);

        builder
            .Property(x => x.BackupEmail)
            .HasMaxLength(UserSetting.EmailLength); //maximum length in gmail.com

        builder
            .Property(x => x.IsDeleted)
            .HasDefaultValue(false);

        builder
            .Property(x => x.IsEmailConfirmed)
            .HasDefaultValue(false);

        builder
            .Property(x => x.IsPrivate)
            .HasDefaultValue(false);
    }
}

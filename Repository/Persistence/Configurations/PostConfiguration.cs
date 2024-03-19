using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Repository.Entities;

namespace Repository.Persistence.Configurations;

public class PostConfiguration : IEntityTypeConfiguration<Post>
{
    public void Configure(EntityTypeBuilder<Post> builder)
    {
        builder.HasKey(x => x.PostId);

        builder.Property(x => x.PostId).ValueGeneratedOnAdd();

        builder.HasOne(x => x.AppUser)
            .WithMany(x => x.Posts)
            .HasForeignKey("AuthorId");

        builder.HasOne(x => x.PostCategory)
            .WithMany(x => x.Posts)
            .HasForeignKey("CategoryId");
    }
}
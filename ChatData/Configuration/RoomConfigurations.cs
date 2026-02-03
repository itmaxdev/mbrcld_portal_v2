using ChatData.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Linq;

namespace ChatData.Configuration
{
    public class RoomConfigurations : IEntityTypeConfiguration<Room>
    {
        public static readonly Guid[] Ids =
        {
            new Guid("11111111-1111-1111-1111-111111111111"),
            new Guid("22222222-2222-2222-2222-222222222222"),
            new Guid("33333333-3333-3333-3333-333333333333")
        };

        public void Configure(EntityTypeBuilder<Room> builder)
        {
            builder.Property(p => p.Name).IsRequired();

            var data = Ids.Select((p, i) => CreateModel(i)).ToList();
            builder.HasData(data);
        }

        private Room CreateModel(int index)
        {
            var number = (index + 1).ToString();

            return new Room
            {
                Id = Ids[index],
                Name = "Room " + number,
                Description = "Room " + number + " description",
                Image = "",
                RoomType = RoomTypes.Module,
            };
        }
    }
}

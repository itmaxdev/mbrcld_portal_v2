using ChatData.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Linq;

namespace ChatData.Configuration
{
    public class MessageConfiguration : IEntityTypeConfiguration<Message>
    {
        public static readonly Guid[] Ids =
        {
            new Guid("11111111-1111-1111-1111-111111111111"),
            new Guid("22222222-2222-2222-2222-222222222222"),
            new Guid("33333333-3333-3333-3333-333333333333"),
        };

        public void Configure(EntityTypeBuilder<Message> builder)
        {
            builder
                .HasOne(p => p.Room)
                .WithMany(p => p.Messages)
                .HasForeignKey(p => p.RoomId);

            builder
                .HasOne(p => p.File)
                .WithOne(p => p.Message)
                .HasForeignKey<File>(p => p.MessageId);

            var data = Ids.Select((p, i) => CreateModel(i, ParticipantsConfiguration.UserIds[0])).ToList();

            builder.HasData(data);
        }

        private Message CreateModel(int index, Guid userId)
        {
            var number = (index + 1).ToString();

            return new Message
            {
                Id = Ids[index],
                Text = "Message" + number,
                Time = DateTime.UtcNow,
                RoomId = RoomConfigurations.Ids[index],
                UserId = userId,
            };
        }
    }
}

using ChatData.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;

namespace ChatData.Configuration
{
    class ParticipantsConfiguration : IEntityTypeConfiguration<Participants>
    {
        public static readonly Guid[] Ids =
        {
            new Guid("11111111-1111-1111-1111-111111111111"),
            new Guid("22222222-2222-2222-2222-222222222222"),
            new Guid("33333333-3333-3333-3333-333333333333"),
            new Guid("44444444-4444-4444-4444-444444444444"),
            new Guid("55555555-5555-5555-5555-555555555555"),
            new Guid("66666666-6666-6666-6666-666666666666"),
        };

        public static readonly Guid[] UserIds =
        {
            new Guid("5E78F76E-3BB5-41B8-8DF9-1B951C6570F7"),
            new Guid("e8854ae6-b79d-4d22-a680-e7be40c2c694"),
        };

        public void Configure(EntityTypeBuilder<Participants> builder)
        {
            builder
                .HasOne(p => p.Room)
                .WithMany(p => p.Participants)
                .HasForeignKey(p => p.RoomId);

            builder
                .Property(p => p.LastMessageId)
                .IsRequired(false);

            builder
                .Property(p => p.LastMessageDate)
                .IsRequired(false);

            var data = new List<Participants>
            {
                CreateModel(Ids[0], RoomConfigurations.Ids[0], UserIds[0], MessageConfiguration.Ids[0]),
                CreateModel(Ids[1], RoomConfigurations.Ids[1], UserIds[0], MessageConfiguration.Ids[1]),
                CreateModel(Ids[2], RoomConfigurations.Ids[2], UserIds[0], MessageConfiguration.Ids[2]),
                CreateModel(Ids[3], RoomConfigurations.Ids[0], UserIds[1], MessageConfiguration.Ids[0]),
                CreateModel(Ids[4], RoomConfigurations.Ids[1], UserIds[1], MessageConfiguration.Ids[1]),
                CreateModel(Ids[5], RoomConfigurations.Ids[2], UserIds[1], MessageConfiguration.Ids[2]),
            };

            builder.HasData(data);
        }

        private Participants CreateModel(Guid id, Guid roomId, Guid userId, Guid messageId)
        {
            return new Participants
            {
                Id = id,
                RoomId = roomId,
                UserId = userId,
                LastMessageId = messageId
            };
        }
    }
}

using System;
using Xunit;
using ShortageApp.Models;
using ShortageApp.Services;

namespace ShortageApp.Tests
{
    public class ShortageServiceTests
    {
        [Fact]
        public void AddShortage_ShouldAddShortage()
        {
            ShortageService service = new ShortageService();

            var shortage = new Shortage
            {
                Title = "Projector",
                Name = "user1",
                Room = Room.MeetingRoom,
                Category = Category.Electronics,
                Priority = 5,
                CreatedOn = DateTime.Now
            };

            service.AddShortage(shortage);
            var shortages = service.GetShortages("user1", true);

            Assert.Single(shortages, s => s.Title == "Projector" && s.Room == Room.MeetingRoom);
        }

         [Fact]
        public void DeleteShortage_ShouldRemoveShortage_WhenUserIsOwnerOrAdmin()
        {
            ShortageService service = new ShortageService();

            var shortage = new Shortage
            {
                Title = "Coffee",
                Name = "user2",
                Room = Room.Kitchen,
                Category = Category.Food,
                Priority = 7,
                CreatedOn = DateTime.Now
            };

            service.AddShortage(shortage);

            // Act
            service.DeleteShortage("Coffee", Room.Kitchen, "user2", true);
            var shortages = service.GetShortages("user2", true);

            // Assert
            Assert.DoesNotContain(shortages, s => s.Title == "Coffee" && s.Room == Room.Kitchen);
        }
    }
}
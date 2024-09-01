using System;
using Xunit;
using ShortageApp.Models;
using ShortageApp.Services;

namespace ShortageApp.Tests
{
    public class ShortageServiceTests
    {

        private static ShortageService _service = new ShortageService();

        [Fact]
        public void AddShortage_ShouldAddShortage()
        {
            var shortage = new Shortage
            {
                Title = "Projector",
                Name = "user1",
                Room = Room.MeetingRoom,
                Category = Category.Electronics,
                Priority = 5,
                CreatedOn = DateTime.Now
            };

            _service.AddShortage(shortage);
            var shortages = _service.GetShortages("user1", true);

            Assert.Single(shortages, s => s.Title == "Projector" && s.Room == Room.MeetingRoom);
        }

         [Fact]
        public void DeleteShortage_ShouldRemoveShortage_WhenUserIsOwnerOrAdmin()
        {
            var shortage = new Shortage
            {
                Title = "Coffee",
                Name = "user2",
                Room = Room.Kitchen,
                Category = Category.Food,
                Priority = 7,
                CreatedOn = DateTime.Now
            };

            _service.AddShortage(shortage);

            _service.DeleteShortage("Coffee", Room.Kitchen, "user2", true);
            var shortages = _service.GetShortages("user2", true);

            Assert.DoesNotContain(shortages, s => s.Title == "Coffee" && s.Room == Room.Kitchen);
        }
    }
}
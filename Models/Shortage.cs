using System;

namespace ShortageApp.Models
{

    public enum Room
    {
        MeetingRoom, 
        Kitchen, 
        Bathroom
    }

    public enum Category 
    {
        Electronics,
        Food,
        Other
    }

    public class Shortage
    {
        public string Title { get; set; }
        public string Name { get; set; }
        public Room Room { get; set; }
        public Category Category { get; set; }
        public int Priority { get; set; }
        public DateTime CreatedOn { get; set; }

        public override string ToString()
        {
            return $"Title: {Title}, Name: {Name}, Room: {Room}, Category: {Category}, Priority: {Priority}, CreatedOn: {CreatedOn.ToShortDateString()}";
        }
    }
}
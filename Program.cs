using System;

using ShortageApp.Models;
using ShortageApp.Services;

namespace ShortageApp
{
    public class Program
    {
        private static ShortageService _service = new ShortageService();

        public static void Main(string[] args)
        {
            // Note:
            // This would normally be obtained through user input or authentication, but due to time constraints I didn't get around to it
            string userName = "admin"; 

            // Note:
            ///This would also normally be determined through authentication
            bool isAdmin = true;

            while (true)
            {
                Console.WriteLine("Select an option: ");
                Console.WriteLine("1. Register a new shortage");
                Console.WriteLine("2. Delete a shortage");
                Console.WriteLine("3. List all shortages");
                Console.WriteLine("4. Filter shortages");
                Console.WriteLine("5. Exit");

                var input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        RegisterShortage(userName);
                        break;
                    case "2":
                        DeleteShortage(userName, isAdmin);
                        break;
                    case "3":
                        ListShortages(userName, isAdmin);
                        break;
                    case "4":
                        FilterShortages();
                        break;
                    case "5":
                        return;
                    default:
                        Console.WriteLine("Invalid option.");
                        break;
                }
            }
        }

        private static void RegisterShortage(string userName)
        {
            Console.WriteLine("Enter title: ");
            string? title = Console.ReadLine();

            Console.WriteLine("Select room (1. Meeting Room, 2. Kitchen, 3. Bathroom): ");
            Room room = (Room)(int.Parse(Console.ReadLine()) - 1);

            Console.WriteLine("Select category (1. Electronics, 2. Food, 3. Other): ");
            Category category = (Category)(int.Parse(Console.ReadLine()) - 1);

            Console.WriteLine("Enter priority (1-10): ");
            int priority = int.Parse(Console.ReadLine());

            var shortage = new Shortage
            {
                Title = title,
                Name = userName,
                Room = room,
                Category = category,
                Priority = priority,
                CreatedOn = DateTime.Now
            };

            _service.AddShortage(shortage);
        }

        private static void DeleteShortage(string userName, bool isAdmin)
        {
            Console.WriteLine("Enter title: ");
            string? title = Console.ReadLine();

            Console.WriteLine("Select room (1. Meeting Room, 2. Kitchen, 3. Bathroom): ");
            Room room = (Room)(int.Parse(Console.ReadLine()) - 1);

            _service.DeleteShortage(title, room, userName, isAdmin);
        }

        private static void ListShortages(string userName, bool isAdmin)
        {
            var shortages = _service.GetShortages(userName, isAdmin);

            foreach (var shortage in shortages)
            {
                Console.WriteLine(shortage);
            }
        }

        private static void FilterShortages()
        {
            Console.WriteLine("Enter title to filter (optional): ");
            string? title = Console.ReadLine();

            Console.WriteLine("Enter start date (yyyy-mm-dd, optional): ");
            DateTime? startDate = DateTime.TryParse(Console.ReadLine(), out var start) ? start : (DateTime?)null;

            Console.WriteLine("Enter end date (yyyy-mm-dd, optional): ");
            DateTime? endDate = DateTime.TryParse(Console.ReadLine(), out var end) ? end : (DateTime?)null;

            Console.WriteLine("Select category (1. Electronics, 2. Food, 3. Other, 0. All): ");
            int categoryInput = int.Parse(Console.ReadLine());
            Category? category = categoryInput == 0 ? (Category?)null : (Category)(categoryInput - 1);

            Console.WriteLine("Select room (1. Meeting Room, 2. Kitchen, 3. Bathroom, 0. All): ");
            int roomInput = int.Parse(Console.ReadLine());
            Room? room = roomInput == 0 ? (Room?)null : (Room)(roomInput - 1);

            var shortages = _service.FilterShortages(title, startDate, endDate, category, room);

            foreach (var shortage in shortages)
            {
                Console.WriteLine(shortage);
            }
        }
    }
}
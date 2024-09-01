using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Linq;

using ShortageApp.Models;

namespace ShortageApp.Services
{
    public class ShortageService
    {
        private readonly string _filePath = "./data.json";
        private List<Shortage> _shortages;

        public ShortageService()
        {
            LoadShortages();
        }

        private void LoadShortages()
        {
            if (File.Exists(_filePath))
            {
                var json = File.ReadAllText(_filePath);
                _shortages = JsonSerializer.Deserialize<List<Shortage>>(json) ?? new List<Shortage>();
            }
            else
            {
                _shortages = new List<Shortage>();
            }
        }

        private void SaveData()
        {
            var json = JsonSerializer.Serialize(_shortages);
            File.WriteAllText(_filePath, json);
        }

        public void AddShortage(Shortage shortage)
        {
            var existingShortage = _shortages.FirstOrDefault(s => s.Title == shortage.Title && s.Room == shortage.Room);

            if (existingShortage != null)
            {
                if (shortage.Priority > existingShortage.Priority)
                {
                    _shortages.Remove(existingShortage);
                    _shortages.Add(shortage);

                    SaveData();

                    Console.WriteLine("Existing shortage updated with higher priority.");
                }
                else
                {
                    Console.WriteLine("A shortage with the same title and room already exists with equal or higher priority.");
                }
            }
            else
            {
                _shortages.Add(shortage);
                SaveData();
                Console.WriteLine("Shortage added successfully.");
            }
        }

        public void DeleteShortage(string title, Room room, string userName, bool isAdmin)
        {
            var shortage = _shortages.FirstOrDefault(s => s.Title == title && s.Room == room);

            if (shortage != null)
            {
                if (isAdmin || shortage.Name == userName)
                {
                    _shortages.Remove(shortage);
                    SaveData();
                    Console.WriteLine("Shortage deleted successfully.");
                }
                else
                {
                    Console.WriteLine("Only the creator or an administrator can delete this shortage.");
                }
            }
            else
            {
                Console.WriteLine("Shortage not found.");
            }
        }

        public List<Shortage> GetShortages(string userName, bool isAdmin)
        {
            return isAdmin ? _shortages : _shortages.Where(s => s.Name == userName).ToList();
        }

        public List<Shortage> FilterShortages(string title, DateTime? startDate, DateTime? endDate, Category? category, Room? room)
        {
            return _shortages
                .Where(s => (string.IsNullOrEmpty(title) || s.Title.Contains(title, StringComparison.OrdinalIgnoreCase)) &&
                            (!startDate.HasValue || s.CreatedOn >= startDate) &&
                            (!endDate.HasValue || s.CreatedOn <= endDate) &&
                            (!category.HasValue || s.Category == category) &&
                            (!room.HasValue || s.Room == room))
                .OrderByDescending(s => s.Priority)
                .ToList();
        }
    }
}
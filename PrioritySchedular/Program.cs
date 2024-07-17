using System;
using System.Collections.Generic;

class Program
{
    static void Main(string[] args)
    {
        List<Event> events = new List<Event>
        {
            new Event { Id = 1, StartTime = TimeSpan.Parse("10:00"), EndTime = TimeSpan.Parse("12:00"), Location = "A", Priority = 50 },
            new Event { Id = 2, StartTime = TimeSpan.Parse("10:00"), EndTime = TimeSpan.Parse("11:00"), Location = "B", Priority = 30 },
            new Event { Id = 3, StartTime = TimeSpan.Parse("11:30"), EndTime = TimeSpan.Parse("12:30"), Location = "A", Priority = 40 },
            new Event { Id = 4, StartTime = TimeSpan.Parse("14:30"), EndTime = TimeSpan.Parse("16:00"), Location = "C", Priority = 70 },
            new Event { Id = 5, StartTime = TimeSpan.Parse("14:25"), EndTime = TimeSpan.Parse("15:30"), Location = "B", Priority = 60 },
            new Event { Id = 6, StartTime = TimeSpan.Parse("13:00"), EndTime = TimeSpan.Parse("14:00"), Location = "D", Priority = 80 }
        };

        List<LocationTransition> transitions = new List<LocationTransition>
        {
            new LocationTransition { From = "A", To = "B", DurationMinutes = 15 },
            new LocationTransition { From = "A", To = "C", DurationMinutes = 20 },
            new LocationTransition { From = "A", To = "D", DurationMinutes = 10 },
            new LocationTransition { From = "B", To = "C", DurationMinutes = 5 },
            new LocationTransition { From = "B", To = "D", DurationMinutes = 25 },
            new LocationTransition { From = "C", To = "D", DurationMinutes = 25 }
        };

        Scheduler scheduler = new Scheduler(events, transitions);
        var result = scheduler.ScheduleEvents();

        Console.WriteLine($"Katılınabilecek Maksimum Etkinlik Sayısı: {result.maxEvents}");
        Console.WriteLine($"Katılınabilecek Etkinliklerin ID'leri: {string.Join(", ", result.eventIds)}");
        Console.WriteLine($"Toplam Değer: {result.totalValue}");
    }
}

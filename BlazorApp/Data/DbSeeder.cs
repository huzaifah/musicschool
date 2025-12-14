using BlazorApp.Data.Entities;
using BlazorApp.Data.Enums;

namespace BlazorApp.Data;

public static class DbSeeder
{
    public static void Seed(ApplicationDbContext context)
    {
        if (context.Instruments.Any())
        {
            return;
        }

        var instruments = new List<Instrument>
        {
            new() { Name = "Piano", Description = "Classical keyboard instrument", Category = "Keyboard" },
            new() { Name = "Guitar", Description = "Six-stringed instrument", Category = "String" },
            new() { Name = "Violin", Description = "Bowed string instrument", Category = "String" },
            new() { Name = "Drums", Description = "Percussion instrument set", Category = "Percussion" },
            new() { Name = "Saxophone", Description = "Woodwind instrument", Category = "Wind" },
            new() { Name = "Flute", Description = "Wind instrument", Category = "Wind" },
            new() { Name = "Cello", Description = "Large bowed string instrument", Category = "String" }
        };
        context.Instruments.AddRange(instruments);

        var instructors = new List<Instructor>
        {
            new()
            {
                Name = "Sarah Johnson",
                Email = "sarah.johnson@musicschool.com",
                Phone = "+1-555-0101",
                Bio = "Professional pianist with 15 years of teaching experience. Specializes in classical and jazz piano. Graduated from Juilliard School of Music.",
                Specialization = "Piano, Music Theory",
                HourlyRate = 75.00m,
                IsActive = true
            },
            new()
            {
                Name = "Michael Chen",
                Email = "michael.chen@musicschool.com",
                Phone = "+1-555-0102",
                Bio = "Expert guitarist with a passion for rock, blues, and acoustic styles. Toured with several bands before focusing on teaching.",
                Specialization = "Guitar, Bass Guitar",
                HourlyRate = 65.00m,
                IsActive = true
            },
            new()
            {
                Name = "Emma Rodriguez",
                Email = "emma.rodriguez@musicschool.com",
                Phone = "+1-555-0103",
                Bio = "Classically trained violinist and cellist. Former member of the city symphony orchestra. Loves teaching students of all ages.",
                Specialization = "Violin, Cello",
                HourlyRate = 80.00m,
                IsActive = true
            },
            new()
            {
                Name = "David Thompson",
                Email = "david.thompson@musicschool.com",
                Phone = "+1-555-0104",
                Bio = "Professional drummer and percussionist with 20 years of experience in jazz, rock, and world music.",
                Specialization = "Drums, Percussion",
                HourlyRate = 70.00m,
                IsActive = true
            },
            new()
            {
                Name = "Lisa Martinez",
                Email = "lisa.martinez@musicschool.com",
                Phone = "+1-555-0105",
                Bio = "Saxophonist and flutist specializing in jazz and classical music. Berklee College of Music graduate.",
                Specialization = "Saxophone, Flute",
                HourlyRate = 72.00m,
                IsActive = true
            }
        };
        context.Instructors.AddRange(instructors);
        context.SaveChanges();

        var random = new Random(42);
        var musicClasses = new List<MusicClass>();
        var instrumentNames = new[] { "Piano", "Guitar", "Violin", "Drums", "Saxophone", "Flute", "Cello" };
        var skillLevels = new[] { SkillLevel.Beginner, SkillLevel.Intermediate, SkillLevel.Advanced };
        var classId = 1;

        foreach (var instructor in instructors)
        {
            var instructorInstruments = instructor.Specialization.Split(',').Select(s => s.Trim()).ToArray();

            for (int day = 1; day <= 30; day++)
            {
                var classDate = DateTime.Today.AddDays(day);

                var timeSlots = new[] { 9, 11, 14, 16, 18 };
                var selectedSlots = timeSlots.OrderBy(_ => random.Next()).Take(random.Next(2, 4));

                foreach (var hour in selectedSlots)
                {
                    var instrument = instructorInstruments[random.Next(instructorInstruments.Length)];
                    if (instrument.Contains("Theory")) continue;

                    musicClasses.Add(new MusicClass
                    {
                        Id = classId++,
                        InstructorId = instructor.Id,
                        Instrument = instrument,
                        Level = skillLevels[random.Next(skillLevels.Length)],
                        ScheduledDateTime = classDate.AddHours(hour),
                        DurationMinutes = 60,
                        Price = instructor.HourlyRate,
                        Description = $"{skillLevels[random.Next(skillLevels.Length)]} level {instrument} lesson focusing on technique and repertoire.",
                        Status = ClassStatus.Available
                    });
                }
            }
        }

        var classesToAdd = musicClasses.Take(100).ToList();
        context.MusicClasses.AddRange(classesToAdd);
        context.SaveChanges();

        var bookings = new List<Booking>
        {
            new()
            {
                MusicClassId = classesToAdd[0].Id,
                StudentName = "John Smith",
                StudentEmail = "john.smith@email.com",
                StudentPhone = "+1-555-1001",
                Notes = "Beginner student, first time learning piano",
                BookedAt = DateTime.UtcNow.AddDays(-2),
                Status = BookingStatus.Confirmed
            },
            new()
            {
                MusicClassId = classesToAdd[5].Id,
                StudentName = "Emily Davis",
                StudentEmail = "emily.davis@email.com",
                StudentPhone = "+1-555-1002",
                Notes = "Intermediate guitarist looking to improve solo techniques",
                BookedAt = DateTime.UtcNow.AddDays(-1),
                Status = BookingStatus.Confirmed
            },
            new()
            {
                MusicClassId = classesToAdd[10].Id,
                StudentName = "Robert Wilson",
                StudentEmail = "robert.wilson@email.com",
                StudentPhone = "+1-555-1003",
                Notes = null,
                BookedAt = DateTime.UtcNow.AddHours(-12),
                Status = BookingStatus.Confirmed
            },
            new()
            {
                MusicClassId = classesToAdd[15].Id,
                StudentName = "Jennifer Brown",
                StudentEmail = "jennifer.brown@email.com",
                StudentPhone = "+1-555-1004",
                Notes = "Preparing for a recital",
                BookedAt = DateTime.UtcNow.AddHours(-6),
                Status = BookingStatus.Confirmed
            },
            new()
            {
                MusicClassId = classesToAdd[20].Id,
                StudentName = "William Garcia",
                StudentEmail = "william.garcia@email.com",
                StudentPhone = "+1-555-1005",
                Notes = "Advanced student, interested in jazz improvisation",
                BookedAt = DateTime.UtcNow.AddHours(-3),
                Status = BookingStatus.Confirmed
            },
            new()
            {
                MusicClassId = classesToAdd[25].Id,
                StudentName = "Jessica Miller",
                StudentEmail = "jessica.miller@email.com",
                StudentPhone = "+1-555-1006",
                Notes = "Learning as a hobby",
                BookedAt = DateTime.UtcNow.AddHours(-1),
                Status = BookingStatus.Confirmed
            }
        };

        context.Bookings.AddRange(bookings);

        foreach (var booking in bookings)
        {
            var musicClass = classesToAdd.First(c => c.Id == booking.MusicClassId);
            musicClass.Status = ClassStatus.Booked;
        }

        context.SaveChanges();
    }
}

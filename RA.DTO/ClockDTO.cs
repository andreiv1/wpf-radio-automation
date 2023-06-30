using RA.Database.Models;

namespace RA.DTO
{
    public class ClockDTO
    {
        public int Id { get; set; }
        public String Name { get; set; }

        public ClockDTO()
        {
            Name = string.Empty;    
        }

        public ClockDTO(string name)
        {
            Name = name;
        }

        public static ClockDTO FromEntity(Clock clock)
        {
            return new ClockDTO { Id = clock.Id, Name = clock.Name, };
        }

        public static Clock ToEntity(ClockDTO dto)
        {
            return new Clock { Id = dto.Id, Name = dto.Name, };
        }
    }
}

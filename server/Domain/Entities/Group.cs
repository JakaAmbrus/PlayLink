using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class Group
    {
        public Group()
        {

        }

        public Group(string name)
        {
            Name = Name;
        }

        [Key]
        public string Name { get; set; }
        public ICollection<Connection> Connections { get; set; } = new List<Connection>();
    }
}

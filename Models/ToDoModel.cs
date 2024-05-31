using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ToDoList.Models
{
    public class ToDoModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        private int id { get; set; }

        
        private string description { get; set; } = null!;

        private bool status;

        private string priority { get; set; } = null!;

        private DateTime dueDate;

        private string? category;

        public int Id { get => this.id; set => this.id = value; }
        public string Description { get => this.description; set => this.description = value; }
        public bool Status { get => this.status; set => this.status = value; }
        public string Priority { get => this.priority; set => this.priority = value; }

        public DateTime DueDate { get => this.dueDate; set => this.dueDate = value; }

        public string? Category { get => this.category; set => this.category = value; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace TaskManagementSystem.Models
{
    public class UserModel
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Username { get; set; } 

        [Required]
        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; } 

        public DateTime CreatedAt { get; set; } 

        public DateTime UpdatedAt { get; set; } 
    }
}

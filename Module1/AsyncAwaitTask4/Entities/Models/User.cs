using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Entities.Models
{
    [Table("Users")]
    public class User : IEntity
    {
        [Key]
        [Column("UserId")]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(60, ErrorMessage = "FirstName can't be longer than 60 characters")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(60, ErrorMessage = "LastName can't be longer than 60 characters")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Age is required")]
        [Range(1,120, ErrorMessage = "Age must be from 1 t")]
        public int Age { get; set; }

    }
}

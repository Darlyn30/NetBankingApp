using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetBanking.Core.Application.ViewModels.User
{
    public class SaveUserViewModel
    {
        public string? Id { get; set; }

        [Required(ErrorMessage = "You must enter a name.")]
        [DataType(DataType.Text)]
        public string Name { get; set; }

        [Required(ErrorMessage = "You must enter a lastname.")]
        [DataType(DataType.Text)]
        public string LastName { get; set; }

        [Required(ErrorMessage = "You mus enter a Username.")]
        [DataType(DataType.Text)]
        public string Username { get; set; }

        [Required(ErrorMessage = "You must enter an email.")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [MinLength(8, ErrorMessage = "La contraseña debe tener al menos 8 caracteres.")]
        [RegularExpression(@"^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]+$",
            ErrorMessage = "The password must contain at least one letter, one number, and one special character.")]
        [DataType(DataType.Password)]
        public string? Password { get; set; }

        [Compare(nameof(Password), ErrorMessage = "The password must match")]
        [DataType(DataType.Password)]
        public string? ConfirmPassword { get; set; }

        [Required(ErrorMessage = "You must enter an ID.")]
        [RegularExpression(@"^\d{3}\-\d{7}-\d{1}$", ErrorMessage = "Your Id must be with the following format: ###-#######-#.")]
        [DataType(DataType.Text)]
        public string IdentificationNumber { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "You must select a role.")]
        public int Role { get; set; }

        [DataType(DataType.Currency)]
        public double? AccountAmount { get; set; }

        public bool HasError { get; set; }
        public string? Error { get; set; }
    }
}

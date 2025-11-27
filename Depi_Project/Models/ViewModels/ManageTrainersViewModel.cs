using System.ComponentModel.DataAnnotations;

namespace Depi_Project.Models.ViewModels
{
    public class ManageTrainersViewModel
    {
        public Gym Gym { get; set; }
        public List<Trainer> Trainers { get; set; }
        public CreateTrainerViewModel NewTrainer { get; set; }
    }

    public class CreateTrainerViewModel
    {
        [Required(ErrorMessage = "Trainer name is required")]
        [StringLength(100, MinimumLength = 2)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Bio is required")]
        [StringLength(500, MinimumLength = 10)]
        public string Bio { get; set; }

        public int GymId { get; set; }
    }

    public class EditTrainerViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Name { get; set; }

        [Required]
        [StringLength(500, MinimumLength = 10)]
        public string Bio { get; set; }

        public int GymId { get; set; }
    }
}


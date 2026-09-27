using System.ComponentModel.DataAnnotations;

namespace TARge25shop.Models.Kindergarten;

public class KindergartenFormViewModel
{
    public Guid? Id { get; set; }
    [Required(ErrorMessage = "Sisesta rühma nimi.")]
    [StringLength(200)]
    [Display(Name = "Rühma nimi")]
    public string GroupName { get; set; } = string.Empty;
    [Range(0, 1000, ErrorMessage = "Laste arv peab olema vahemikus 0–1000.")]
    [Display(Name = "Laste arv")]
    public int ChildrenCount { get; set; }
    [Required(ErrorMessage = "Sisesta lasteaia nimi.")]
    [StringLength(200)]
    [Display(Name = "Lasteaia nimi")]
    public string KindergartenName { get; set; } = string.Empty;
    [Required(ErrorMessage = "Sisesta õpetaja nimi.")]
    [StringLength(200)]
    [Display(Name = "Õpetaja nimi")]
    public string TeacherName { get; set; } = string.Empty;
}

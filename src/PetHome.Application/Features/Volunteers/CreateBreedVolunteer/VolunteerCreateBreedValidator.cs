using FluentValidation;
using PetHome.Application.Validator;
using PetHome.Domain.PetManagment.PetEntity;

namespace PetHome.Application.Features.Volunteers.CreateBreedVolunteer;
public class VolunteerCreateBreedValidator:AbstractValidator<VolunteerCreateBreedRequst>
{
	public VolunteerCreateBreedValidator()
	{
		RuleForEach(v => v.Breeds)
			.MustBeValueObject(b => Breed.Create(b, SpeciesId.CreateEmpty().Value));
	}
}

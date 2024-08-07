using AutoMapper;
using Entities;
using Models.Domain;

namespace Mapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<CalculationModel, Calculation>();
            CreateMap<Calculation, CalculationModel>();

                    // CreateRequest -> User
        CreateMap<CreateRequest, User>();

        // UpdateRequest -> User
        CreateMap<UpdateRequest, User>()
            .ForAllMembers(x => x.Condition(
                (src, dest, prop) =>
                {
                    // ignore both null & empty string properties
                    if (prop == null) return false;
                    if (prop.GetType() == typeof(string) && string.IsNullOrEmpty((string)prop)) return false;

                    // ignore null role
                    // if (x.DestinationMember.Name == "Role" && src.Role == null) return false;

                    return true;
                }
            ));

        }
    }
}
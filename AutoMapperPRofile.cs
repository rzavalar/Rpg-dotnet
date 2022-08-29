using AutoMapper;
using dotnet__rpg.Dtos.Characters;

namespace dotnet__rpg
{
    //Hereda DE PROFILE OJO
    public class AutoMapperPRofile : Profile
    {
        //Se mapean lo que se quiere convertir en el primero 
        // de character a get character
        public AutoMapperPRofile(){
            CreateMap<Character,GetCharacterDto>();
            CreateMap<AddCharacterDto,Character>();
            CreateMap<UpdateCharacterDto,Character>();
        }
    }
}
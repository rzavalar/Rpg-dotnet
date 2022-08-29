
using dotnet__rpg.Dtos.Characters;

namespace dotnet__rpg.Services.CharacterService
{
    public interface ICharacterService
    {   
        //Se agrego service response para la respuesta del servicio
         Task<ServiceResponse<List<GetCharacterDto>>> GetAllCharacters();

         Task<ServiceResponse<GetCharacterDto>> GetCharacterById (int id);

         Task<ServiceResponse<List<GetCharacterDto>>> AddCharacter(AddCharacterDto newCharacter);

        Task<ServiceResponse<GetCharacterDto>> UpdateCharacter(UpdateCharacterDto updateCharacter);

        Task<ServiceResponse<List<GetCharacterDto>>> DeleteCharacter(int id);

    }
}

using AutoMapper;
using Microsoft.EntityFrameworkCore;
using dotnet__rpg.Data;
using dotnet__rpg.Dtos.Characters;
using System.Security.Claims;

//Al implementar interfaz aparecera error le damos implementar en el ofoc
namespace dotnet__rpg.Services.CharacterService

{
    public class CharacterService : ICharacterService
    {

        private readonly IMapper _mapper;
        private readonly DataContext _context;

        private readonly IHttpContextAccessor _httpContextAccessor;

        //Implementamos data context aqui y mapper con dependenci inyection
        public CharacterService(IMapper mapper, DataContext context, IHttpContextAccessor httpContextAccessor)
        {
            _mapper = mapper;
            _context = context;
            //Accesor para valores de usuario
            _httpContextAccessor = httpContextAccessor;
        }

        private int Getuserid() => int.Parse(_httpContextAccessor.HttpContext.User
        .FindFirstValue(ClaimTypes.NameIdentifier));

        public async Task<ServiceResponse<List<GetCharacterDto>>> AddCharacter(AddCharacterDto newCharacter)
        {
            var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
            Character character = _mapper.Map<Character>(newCharacter);

            character.User = await _context.Users.FirstOrDefaultAsync(u => u.Id == Getuserid());

            //Para agregar un character ocupamos un characer de un addCharacterdto
            _context.Characters.Add(character);

            //Este es el metodo await que esperamos
            await _context.SaveChangesAsync();
            //Ocupamos una lista de characteres DEL USUARIO LOGEADO
            serviceResponse.Data = await _context.Characters
            .Where(u => u.User.Id == Getuserid())
            .Select(c => _mapper.Map<GetCharacterDto>(c))
            .ToListAsync();
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetCharacterDto>> GetCharacterById(int id)
        {
            var serviceResponse = new ServiceResponse<GetCharacterDto>();

            //En esta consulta usamos FiurstOrDefaultAsync por ser asyncrono
            var DbCharacter = await _context.Characters
            .FirstOrDefaultAsync(c => c.ID == id && c.User.Id == Getuserid());

            //Usamos _mapper con el metodo map para mapear dependiendo lo que queremos
            //Entre <> lo que esperamos y () lo que tenemos que mapear
            serviceResponse.Data = _mapper.Map<GetCharacterDto>(DbCharacter);
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> GetAllCharacters()
        {
            var response = new ServiceResponse<List<GetCharacterDto>>();

            //Obtenemos characters de lista para el tolist async usamos directiva microsoft.entityframeworkcore
            var dbCharacters = await _context.Characters
            .Where(x => x.User.Id == Getuserid())
            .ToListAsync();

            response.Data = dbCharacters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToList();

            return response;
            //Forma antigua
            //return new ServiceResponse<List<GetCharacterDto>> {Data = characters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToList()};
        }

        public async Task<ServiceResponse<GetCharacterDto>> UpdateCharacter(UpdateCharacterDto updateCharacter)
        {
            ServiceResponse<GetCharacterDto> response = new ServiceResponse<GetCharacterDto>();

            try
            {
                var character = await _context.Characters
                //Agregamos include para traernos la relacion con user que tiene
                .Include(c => c.User)
                .FirstOrDefaultAsync(c => c.ID == updateCharacter.Id);
                
                if (character.User.Id == Getuserid())
                {
                                 
                //Usamos automaper para mapear el character con el updatecharacter
                _mapper.Map(updateCharacter, character);

                //  character.Name = updateCharacter.Name;
                //  character.HitPoints = updateCharacter.HitPoints;
                //  character.Streingth = updateCharacter.Streingth;
                //  character.Intelligence = updateCharacter.Intelligence;
                //  character.Defense = updateCharacter.Defense;
                //  character.Class = updateCharacter.Class;

                //Usar esto para verificar cambios
                await _context.SaveChangesAsync();
                response.Data = _mapper.Map<GetCharacterDto>(character);
                }
                else
                {
                    response.Succes = false;
                    response.Message ="Character not found";
                }

            }
            catch (Exception ex)
            {
                //Usamos repsonse
                response.Succes = false;
                response.Message = ex.Message;
            }
            return response;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> DeleteCharacter(int id)
        {
            ServiceResponse<List<GetCharacterDto>> response = new ServiceResponse<List<GetCharacterDto>>();

            try
            {
                //Cambiuamos Fisrt por FirstOrDEfaultAsync usamos id de user para solo eliminar los del user
                var character = await _context.Characters.FirstOrDefaultAsync(c => c.ID == id && c.User.Id == Getuserid());
                if (character != null)
                {
                    //Usamos automaper para mapear el character con el updatecharacter
                    _context.Characters.Remove(character);

                    await _context.SaveChangesAsync();

                    response.Data = _context.Characters
                    .Where(u => u.User.Id == Getuserid())
                    .Select(c => _mapper.Map<GetCharacterDto>(c)).ToList();
                }
                else
                {
                    response.Succes = false;
                    response.Message = "Character not found";
                }
            }
            catch (Exception ex)
            {
                //Usamos repsonse
                response.Succes = false;
                response.Message = ex.Message;
            }

            return response;
        }
    }
}

using AutoMapper;
using Microsoft.EntityFrameworkCore;
using dotnet__rpg.Data;
using dotnet__rpg.Dtos.Characters;

//Al implementar interfaz aparecera error le damos implementar en el ofoc
namespace dotnet__rpg.Services.CharacterService

{
    public class CharacterService : ICharacterService
    {
       
        private readonly IMapper _mapper;
        private readonly DataContext _context;

        //Implementamos data context aqui y mapper con dependenci inyection
        public CharacterService(IMapper mapper, DataContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task <ServiceResponse<List<GetCharacterDto>>> AddCharacter(AddCharacterDto newCharacter)
        {
            var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
            Character character = _mapper.Map<Character>(newCharacter);
            
            //Para agregar un character ocupamos un characer de un addCharacterdto
            _context.Characters.Add(character);

            //Este es el metodo await que esperamos
            await _context.SaveChangesAsync();
            //Ocupamos una lista de characteres C 
            serviceResponse.Data = await _context.Characters
            .Select(c => _mapper.Map<GetCharacterDto>(c))
            .ToListAsync();
            return serviceResponse;
        }

        public async Task <ServiceResponse<GetCharacterDto>> GetCharacterById(int id)
        {
            var serviceResponse = new ServiceResponse<GetCharacterDto>();

            //En esta consulta usamos FiurstOrDefaultAsync por ser asyncrono
            var DbCharacter = await _context.Characters.FirstOrDefaultAsync(c=>c.ID==id);

            //Usamos _mapper con el metodo map para mapear dependiendo lo que queremos
            //Entre <> lo que esperamos y () lo que tenemos que mapear
            serviceResponse.Data = _mapper.Map<GetCharacterDto>(DbCharacter);
            return serviceResponse;
        }

        public async Task <ServiceResponse<List<GetCharacterDto>>> GetAllCharacters()
        {
            var response = new ServiceResponse<List<GetCharacterDto>>();

            //Obtenemos characters de lista para el tolist async usamos directiva microsoft.entityframeworkcore
            var dbCharacters = await _context.Characters.ToListAsync();

            response.Data = dbCharacters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToList();

            return response;
            //Forma antigua
            //return new ServiceResponse<List<GetCharacterDto>> {Data = characters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToList()};
        }

        public async Task<ServiceResponse<GetCharacterDto>> UpdateCharacter(UpdateCharacterDto updateCharacter)
        {
            ServiceResponse<GetCharacterDto> response= new ServiceResponse<GetCharacterDto>();

            try{
            var character = await _context.Characters.
            FirstOrDefaultAsync(c =>c.ID == updateCharacter.Id);

            //Usamos automaper para mapear el character con el updatecharacter
            _mapper.Map(updateCharacter,character);

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
            catch(Exception ex){
                //Usamos repsonse
               response.Succes= false;
               response.Message = ex.Message;
            }
           return response; 
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> DeleteCharacter(int id)
        {
            ServiceResponse<List<GetCharacterDto>> response= new ServiceResponse<List<GetCharacterDto>>();

            try{
                //Cambiuamos Fisrt por FirstAsync
            var character = await _context.Characters.FirstAsync(c =>c.ID == id);

            //Usamos automaper para mapear el character con el updatecharacter
            _context.Characters.Remove(character);
            
            await _context.SaveChangesAsync();

            response.Data = _context.Characters.Select(c=> _mapper.Map<GetCharacterDto>(c)).ToList();
            }
            catch(Exception ex){
                //Usamos repsonse
               response.Succes= false;
               response.Message = ex.Message;
            }
            
           return response; 
        }
    }
}
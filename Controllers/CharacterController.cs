
using dotnet__rpg.Services.CharacterService;
using dotnet__rpg.Dtos.Characters;

namespace dotnet__rpg.Controllers
{
// // 1° Agregamos Controller base si es una clase nueva sin snipet y el using del mvc
// // Agregamos Api controller para el routing
// 3° Agregamos el global using  Microsoft.AspNetCore.Mvc; al  program para poder acceder a el por todo el programa
//No olvidar dotnet watch run
//4 ° Tenemos 2 Get metods hay que cambiar los routing atributes Agregamos ejemplo [Route("GetAll")] 
//El single queda con el get por default 
//5° PAra parametros hay que cambiar el routing atributes con llaves de lo que recibe

    [ApiController]
    [Route("api/[controller]")]
    public class CharacterController : ControllerBase
    {
        //Escriber ctor para el construcotr :o aqui abajo esta la injeccion
        //de dependencia 
        //Despues de la inyeccion a cada peticion le asignamos su metodo de la interfaz

        private readonly ICharacterService _characterService;
        public CharacterController(ICharacterService characterService)
        {
            _characterService = characterService;
        }
       //Hasta aqui 
        
        [HttpGet("GetAll")]
        public async Task <ActionResult <ServiceResponse<List<GetCharacterDto>>>> Get(){

            return Ok(await _characterService.GetAllCharacters());

        }
        [HttpGet("{id}")]
        public async Task <ActionResult <ServiceResponse<GetCharacterDto>>> GetSingle(int id){

            return Ok(await _characterService.GetCharacterById(id));
        }

        //Insertamos un character
        [HttpPost]
        public async Task<ActionResult <ServiceResponse<List<GetCharacterDto>>>> AddCharacter (AddCharacterDto newcharacter){

            return Ok(await _characterService.AddCharacter(newcharacter));
        }

        [HttpPut]
        public async Task<ActionResult <ServiceResponse<GetCharacterDto>>> UpdateCharacter (UpdateCharacterDto updatecharacter){

            //Await del servicio
            var response = await _characterService.UpdateCharacter(updatecharacter);
            
            if(response.Data == null)
            {
                return NotFound(response);
            }

            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult <ServiceResponse<List<GetCharacterDto>>>> DeleteCharacter (int id){

            //Await del servicio
            var response = await _characterService.DeleteCharacter(id);
            
            if(response.Data == null)
            {
                return NotFound(response);
            }

            return Ok(response);
        }
    }
}
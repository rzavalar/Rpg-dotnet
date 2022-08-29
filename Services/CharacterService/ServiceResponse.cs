namespace dotnet__rpg.Services.CharacterService
{
    //Se creo este service response para recibir cualquier data por el generic
    //Y en las respuestas dar message o succes para front
    public class ServiceResponse<T>
    {
        public T? Data {get;set;}

        public bool Succes{get;set;} = true;

        public string Message {get;set;}= string.Empty;
    }
}
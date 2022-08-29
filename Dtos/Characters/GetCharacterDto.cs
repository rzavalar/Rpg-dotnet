namespace dotnet__rpg.Dtos.Characters
{
    public class GetCharacterDto
    {
        public int ID{get;set;}
        public string Name {get;set;} = "Frodo";
        public int HitPoints {get;set;} = 100;

        public int Streingth {get;set;} = 10;

        public int  Defense {get;set;} = 10;

        public int Intelligence { get; set; } = 10;

        public RpgClass Class{get;set;} = RpgClass.Knight;
    }
}
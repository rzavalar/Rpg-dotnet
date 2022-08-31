
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using dotnet__rpg.Services.CharacterService;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace dotnet__rpg.Data
{
    public class AuthRepository : IAuthRepository
    {   
        private readonly DataContext _context;
         private readonly IConfiguration _configuration;

        //Inyectamos el data context y el configuration del iconfiguration 
        //Para acceder al key para cifrado JWT
        public AuthRepository(DataContext context,IConfiguration configuration)
        {
             _context = context;
             _configuration = configuration;
        }
        public async Task<ServiceResponse<string>> Login(string username, string password)
        {
           var response = new ServiceResponse<string>();

           var user = await _context.Users
           .FirstOrDefaultAsync(x=>x.UserName.ToLower().Equals(username.ToLower()));

            if(user == null){
                response.Succes= false;
                response.Message = "User Not found";
            }
            else if(!VerifyPasswordHash(password,user.PasswordHash,user.PasswordSalt)){
                response.Succes= false;
                response.Message = "Wrong Password";
            }
            else{
                response.Data = createToken(user);
            }

            return response;

        }

        public async Task<ServiceResponse<int>> Register(User user, string password)
        {       
            ServiceResponse<int> serviceResponse = new ServiceResponse<int>();

            if(await UserExist(user.UserName)){
                serviceResponse.Message="User Exist";
                serviceResponse.Succes = false;
                return serviceResponse;
            }

            
            CreatePasswordHash(password,out byte [] passwrodhash, out byte[] passwordsalt);


            user.PasswordHash = passwrodhash;
            user.PasswordSalt = passwordsalt;
            _context.Users.Add(user);

            await _context.SaveChangesAsync();

            serviceResponse.Data = user.Id;

            return serviceResponse;
        }

        public async Task<bool> UserExist(string username)
        {
            if (await _context.Users.AnyAsync( c => c.UserName.ToLower() == username.ToLower()))
            {
                return true;
            }
                return false;
        }
        
        //Metodo para crear password hash los out parameters nos ayudan a 
        //no regresar nada en void
        public void CreatePasswordHash(string password, out byte[] passwordhash, out byte[] passwordsalt)
        {   //clase usada para usar HMAC
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordsalt= hmac.Key;
                passwordhash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPasswordHash(String password, byte [] passwordHash, byte [] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt)){
                var ComputeHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

                return ComputeHash.SequenceEqual(passwordHash);
            }
        }

        private string createToken(User user){
            //List of claims
            //1° iid identif
            //2° username

            List<Claim> claims = new List<Claim>{
                new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                new Claim(ClaimTypes.Name,user.UserName)
            };

            //Extract key from appsetings
            SymmetricSecurityKey key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.
            GetBytes(_configuration.GetSection("AppSettings:Token").Value));

            //Crea cred con la key creada
            SigningCredentials creds = new SigningCredentials(key,SecurityAlgorithms.HmacSha512Signature);

            //Crea el token descriptor con los claims creados , le ponemos expire un dia y le agregamos cres de key
            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor{
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            //se crea jwt con los datos y se imprime
            JwtSecurityTokenHandler jwt = new JwtSecurityTokenHandler();
            SecurityToken token = jwt.CreateToken(tokenDescriptor);

            return jwt.WriteToken(token);
        }
    }
}
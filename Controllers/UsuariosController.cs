using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using pmv_si_2024_2_pe6_t2_g06_gestao_de_salao_servico_usuario.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.ConstrainedExecution;
using System.Security.Claims;
using System.Text;

namespace pmv_si_2024_2_pe6_t2_g06_gestao_de_salao_servico_usuario.Controllers
{
    [Authorize(Roles = "Administrador")]
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UsuariosController(AppDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            var model = await _context.Usuarios.ToListAsync();
            return Ok(model);
        }
        [HttpPost]
        public async Task<ActionResult> Create(UsuarioDto usuario)
        {
            Usuario novoUsuario = new Usuario()
            {
                Nome = usuario.Nome,
                Email = usuario.Email,
                Senha = BCrypt.Net.BCrypt.HashPassword(usuario.Senha), //criptografia da senha quando salva no banco
                Telefone = usuario.Telefone,
                Endereco = usuario.Email,
                Cidade = usuario.Cidade,
                Estado = usuario.Estado,
                Cep = usuario.Cep,
                DataNascimento = usuario.DataNascimento,
                Genero = usuario.Genero,
                Perfil = usuario.Perfil
            };

            _context.Usuarios.Add(novoUsuario);
            await _context.SaveChangesAsync();
            return Ok(novoUsuario);
            //return CreatedAtAction("GetById", new {id = novoUsuario.Id, novoUsuario});
            //conferir dps
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(int id)
        {
            var model = await _context.Usuarios.FirstOrDefaultAsync(c => c.Id == id);
            if(model == null) NotFound();
            return Ok(model);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, UsuarioDto usuario)
        {
            if(id != usuario.Id) return BadRequest();

            var modeloDb = await _context.Usuarios.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
            if(modeloDb == null) return NotFound();

            modeloDb.Nome = usuario.Nome;
            modeloDb.Email = usuario.Email;
            modeloDb.Senha = BCrypt.Net.BCrypt.HashPassword(usuario.Senha); //criptografia da senha quando salva no banco
            modeloDb.Telefone = usuario.Telefone;
            modeloDb.Endereco = usuario.Email;
            modeloDb.Cidade = usuario.Cidade;
            modeloDb.Estado = usuario.Estado;
            modeloDb.Cep = usuario.Cep;
            modeloDb.DataNascimento = usuario.DataNascimento;
            modeloDb.Genero = usuario.Genero;
            modeloDb.Perfil = usuario.Perfil;


            _context.Usuarios.Update(modeloDb);
            await _context.SaveChangesAsync();
            return NoContent();

        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var model = await _context.Usuarios.FindAsync(id);

            if (model == null) NotFound();

            _context.Usuarios.Remove(model);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<ActionResult> Authenticate(AuthenticateDto model)
        {
            var usuarioDb = await _context.Usuarios.FindAsync(model.Id);

            //se o usuario = null ou a senha nao bate
            if(usuarioDb == null || !BCrypt.Net.BCrypt.Verify(model.Senha, usuarioDb.Senha)) 
                return Unauthorized();

            var jwt = GenerateJwtToken(usuarioDb);

            return Ok(new {jwtToken = jwt});
        }


        private string GenerateJwtToken(Usuario model)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("Ry74cBQva5dThwbwchR9jhbtRFnJxWSZ");
            var claims = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, model.Id.ToString()),
                new Claim(ClaimTypes.Role, model.Perfil.ToString())
            });

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claims,
                Expires = DateTime.UtcNow.AddHours(8),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }






        /*[HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Usuario usuario)
        {
            if (id != usuario.Id) return BadRequest();

            var modeloDb = await _context.Usuarios.AsNoTracking().FirstOrDefaultAsync(c => c.Id == id);
            if (modeloDb == null) return NotFound();

            modeloDb.Nome = usuario.Nome;
            modeloDb.Email = usuario.Email;
            //modeloDb.Senha = BCrypt.Net.BCrypt.HashPassword(usuario.Senha); //criptografia da senha quando salva no banco
            modeloDb.Senha = usuario.Senha;
            modeloDb.Telefone = usuario.Telefone;
            modeloDb.Endereco = usuario.Email;
            modeloDb.Cidade = usuario.Cidade;
            modeloDb.Estado = usuario.Estado;
            modeloDb.Cep = usuario.Cep;
            modeloDb.DataNascimento = usuario.DataNascimento;
            modeloDb.Genero = usuario.Genero;
            modeloDb.Perfil = usuario.Perfil;

            _context.Usuarios.Update(modeloDb);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsuarioExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        private bool UsuarioExists(int id)
        {
            return _context.Usuarios.Any(e => e.Id == id);
        }*/
    }
}

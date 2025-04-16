using Microsoft.AspNetCore.Mvc;
using ParqueaderoAPI.Models;
using System.Text;
using System;
using ParqueaderoAPI.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections;
using System.Security.Cryptography;

[Route("api/[controller]")]
[ApiController]
public class UsuariosController : ControllerBase
{
    private readonly ParqueaderoContext _context;

    public UsuariosController(ParqueaderoContext context)
    {
        _context = context;
    }
    [HttpPost("Login")]
    public async Task<IActionResult> Login([FromBody] Usuario usuario)
    {
        if (usuario == null || string.IsNullOrEmpty(usuario.NombreUsuario) || string.IsNullOrEmpty(usuario.ContraseñaHash))
        {
            return BadRequest("⚠️ Usuario y contraseña requeridos.");
        }

        var usuarioDb = await _context.Usuarios
            .Where(u => u.NombreUsuario == usuario.NombreUsuario)
            .Select(u => new
            {
                u.UsuarioID,
                u.NombreUsuario,
                u.ContraseñaHash,
                Rol = u.Rol ?? "Admin" 
            })
            .FirstOrDefaultAsync();

        if (usuarioDb == null)
        {
            return Unauthorized("❌ Usuario no encontrado.");
        }

        string hashInput = HashContraseña(usuario.ContraseñaHash);

        if (!usuarioDb.ContraseñaHash.Equals(hashInput, StringComparison.OrdinalIgnoreCase))
        {
            return Unauthorized("❌ Contraseña incorrecta.");
        }

        return Ok(new { usuarioDb.UsuarioID, usuarioDb.NombreUsuario, usuarioDb.Rol });
    }


    private string HashContraseña(string contraseña)
    {
        using (System.Security.Cryptography.SHA256 sha256 = System.Security.Cryptography.SHA256.Create())
        {
            byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(contraseña));
            return BitConverter.ToString(hashBytes).Replace("-", "").ToUpper(); 
        }
    }





}

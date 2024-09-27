﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace pmv_si_2024_2_pe6_t2_g06_gestao_de_salao_servico_usuario.Models
{
    [Table("Usuarios")]
    public class Usuario
    {

        [Key]
        //[ScaffoldColumn(false)]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Nome { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [JsonIgnore] //nao vai retornar esse campo
        public string Senha { get; set; }

        [Required]
        [Phone]
        public string Telefone { get; set; }

        [Required]
        [MaxLength(200)]
        public string Endereco { get; set; }

        [Required]
        [MaxLength(200)]
        public string Cidade { get; set; }

        [Required]
        [MaxLength(200)]
        public string Estado { get; set; }

        [Required]
        [MaxLength(200)]
        public string Cep { get; set; }

        // Outros detalhes, como data de nascimento, podem ser adicionados conforme necessário        
        [Required]
        [DataType(DataType.Date)]
        public DateTime? DataNascimento { get; set; }

        [Required]
        public GeneroTipo? Genero { get; set; }
        [Required]
        public Perfil Perfil { get; set; }

    }

    public enum GeneroTipo
    {
        Masculino, //0
        Feminino,  //1
        Outro      //2
    }
    public enum Perfil
    {
        [Display(Name = "Administrador")] //0
        Administrador,
        [Display(Name = "Profissional")]  //1
        Profissional,
        [Display(Name = "Usuario")]       //2
        Usuario
    }
}